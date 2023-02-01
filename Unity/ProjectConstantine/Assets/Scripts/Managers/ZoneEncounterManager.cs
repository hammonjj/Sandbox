using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneEncounterManager : MonoBehaviourBase
{
    //Add randomness to this by picking a random way to divide the number of enemies by random spawn waves
    public int WavesToSpawn = 3;
    public int EnemiesToSpawn = 5;
    public int TimeBetweenWaves = 3;
    public float InitialTimeBeforeSpawn = 1500f;

    public GameObject[] BossEnemyPrefabs; //GetAvailableBossEnemies(Zone, Room);
    public GameObject[] EliteEnemyPrefabs; //GetAvailableEliteEnemies(Zone, Room);

    //For future - map all walkable area and use that as an area of spawn points
    public GameObject[] SpawnPoints;

    private int _currentWave;
    private int _currentlyDeadEnemies;
    private EventManager _eventManager;
    private SceneStateManager _sceneManager;
    private ZoneDataContainer _zoneDataContainer;
    private List<GameObject> _normalEnemyPrefabs;

    private bool _inZone1Start = false;
    private void Start()
    {
        _zoneDataContainer = VerifyComponent<ZoneDataContainer>();
        _sceneManager = VerifyComponent<SceneStateManager>(Constants.Tags.SceneStateManager);

        _eventManager = EventManager.GetInstance();
        _eventManager.onEnemyDeath += OnEnemyDeath;
        _eventManager.onSpawnEnemies += OnSpawnEnemies; //For debugging purposes

        var sceneType = _sceneManager.CurrentSceneType;
        var currentZone = _sceneManager.GetCurrentZone();
        LogDebug($"Current Scene Type: {sceneType}");
        if(sceneType == Constants.Enums.SceneType.None &&
            currentZone == Constants.Enums.Zones.Zone1)
        {
            //If we are here it's because we are in Zone 1 start
            //Player needs to kill the first enemy to unlock the door
            _inZone1Start = true;
            return;
        }
        else if(sceneType == Constants.Enums.SceneType.Boss)
        {
            //If we're in a boss fight, need to do something different
        }
        else if(sceneType == Constants.Enums.SceneType.None ||
            sceneType == Constants.Enums.SceneType.Rest ||
            sceneType == Constants.Enums.SceneType.Shop ||
            sceneType == Constants.Enums.SceneType.Story)
        {
            _eventManager.OnEncounterEnded();
            Destroy(gameObject);
            return;
        }
        else
        {
            //We're in a normal or elite fight
            SpawnPoints = GameObject.FindGameObjectsWithTag(Constants.Tags.SpawnPoint);
            if(SpawnPoints == null || SpawnPoints.Length == 0)
            {
                LogError("Unable to Locate Spawn Points");
            }

            _normalEnemyPrefabs = _zoneDataContainer.GetAvailableNormalEnemies(
                currentZone, Constants.Enums.RoomType.Normal);

            if(_normalEnemyPrefabs == null || _normalEnemyPrefabs.Count == 0)
            {
                LogError($"No enemy prefabs for zone/roomType: {currentZone}, Normal");
            }

            //Encounters start with enemies already spawned but pause first to let player get their bearings
            InstantiateWave();
            //Future: Calculate the following number of waves
        }
    }

    private void Update()
    {
        if(_inZone1Start && _currentlyDeadEnemies == 1)
        {
            _inZone1Start = false;
            _eventManager.OnEncounterEnded();
            Destroy(gameObject);
            return;
        }

        if(_currentlyDeadEnemies < EnemiesToSpawn)
        {
            return;
        }

        //Pause for a moment to breathe
        //  - Random number of seconds
        //  - Random encounter it can be continuous

        //Spawn Next Wave
        _currentlyDeadEnemies = 0;

        if(_currentWave < WavesToSpawn)
        {
            _currentWave += 1;
            InstantiateWave();
            return;
        }
        else
        {
            _eventManager.OnEncounterEnded();
            Destroy(gameObject);
        }
    }

    private void OnEnemyDeath()
    {
        _currentlyDeadEnemies += 1;
        LogDebug("onEnemyDied event received");
    }

    private void InstantiateWave()
    {
        LogDebug($"Instantiating wave {_currentWave} of {WavesToSpawn}");

        var unusedSpawns = SpawnPoints.ToList();
        for(int i = 0; i < EnemiesToSpawn; i++)
        {
            var spawnPoint = Helper.RandomInclusiveRange(0, unusedSpawns.Count - 1);
            var randomPrefab = Helper.RandomInclusiveRange(0, _normalEnemyPrefabs.Count - 1);

            //LogDebug($"Spawning Enemy #{EnemiesToSpawn} - SpawnPoint Index: {spawnPoint} - SpawnPointCount: {unusedSpawns.Count}");
            //LogDebug($"Spawning Enemy #{EnemiesToSpawn} - RandomPrefab Index: {randomPrefab} - NormalEnemyPrefabsLength: {NormalEnemyPrefabs.Length}");

            Instantiate(
                _normalEnemyPrefabs[randomPrefab],
                unusedSpawns[spawnPoint].transform.position,
                unusedSpawns[spawnPoint].transform.rotation);

            //Prevent enemies from spawning on top of each other
            unusedSpawns.RemoveAt(spawnPoint);
        }

        LogDebug($"Finished instantiating wave {_currentWave} of {WavesToSpawn}");
    }

    //This comes through the debugging window
    private void OnSpawnEnemies()
    {
        //We're in a normal or elite fight
        SpawnPoints = GameObject.FindGameObjectsWithTag(Constants.Tags.SpawnPoint);
        if(SpawnPoints == null || SpawnPoints.Length == 0)
        {
            LogError("Unable to Locate Spawn Points");
        }

        var unusedSpawns = SpawnPoints.ToList();
        for(int i = 0; i < EnemiesToSpawn; i++)
        {
            var spawnPoint = Helper.RandomInclusiveRange(0, unusedSpawns.Count - 1);
            var randomPrefab = Helper.RandomInclusiveRange(0, _normalEnemyPrefabs.Count - 1);

            //LogDebug($"Spawning Enemy #{EnemiesToSpawn} - SpawnPoint Index: {spawnPoint} - SpawnPointCount: {unusedSpawns.Count}");
            //LogDebug($"Spawning Enemy #{EnemiesToSpawn} - RandomPrefab Index: {randomPrefab} - NormalEnemyPrefabsLength: {NormalEnemyPrefabs.Length}");

            Instantiate(
                _normalEnemyPrefabs[randomPrefab],
                unusedSpawns[spawnPoint].transform.position,
                unusedSpawns[spawnPoint].transform.rotation);

            //Prevent enemies from spawning on top of each other
            unusedSpawns.RemoveAt(spawnPoint);
        }
    }
}
