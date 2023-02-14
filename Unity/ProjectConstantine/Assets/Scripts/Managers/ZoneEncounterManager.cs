using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneEncounterManager : MonoBehaviourBase
{
    //Add randomness to this by picking a random way to divide the number of enemies by random spawn waves
    public int WavesToSpawn = 3;
    public int EnemiesToSpawn = 5;
    public int TimeBetweenWaves = 3;
    public int InitialTimeBeforeSpawn = 5;

    public GameObject[] BossEnemyPrefabs; //GetAvailableBossEnemies(Zone, Room);
    public GameObject[] EliteEnemyPrefabs; //GetAvailableEliteEnemies(Zone, Room);

    //For future - map all walkable area and use that as an area of spawn points
    public GameObject[] SpawnPoints;

    private int _currentWave;
    private int _currentlyDeadEnemies;
    private EventManager _eventManager;
    private SceneStateManager _sceneManager;
    private ZoneDataContainer _zoneDataContainer;
    private List<ZoneDataContainer.EnemyEntry> _normalEnemyPrefabs;

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

            var chamberData = _zoneDataContainer.GetDataForChamber(_sceneManager.GetCurrentScene(), currentZone);
            EnemiesToSpawn = chamberData == null ? EnemiesToSpawn : chamberData.EnemiesPerWave;

            //Encounters start with enemies already spawned but pause first to let player get their bearings
            StartCoroutine(InstantiateWave(InitialTimeBeforeSpawn));
            //Future: Calculate the following number of waves
        }
    }

    private void Update()
    {
        if(_inZone1Start && _currentlyDeadEnemies >= 1)
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
            StartCoroutine(InstantiateWave(TimeBetweenWaves));
            return;
        }
        else
        {
            _eventManager.OnEncounterEnded();
            Destroy(gameObject);
        }
    }

    private void OnEnemyDeath(int currency)
    {
        _currentlyDeadEnemies += 1;
        LogDebug("onEnemyDied event received");
    }

    private IEnumerator InstantiateWave(int waitTime)
    {
        LogDebug($"Instantiating wave {_currentWave} of {WavesToSpawn}");

        yield return new WaitForSeconds(waitTime);

        SpawnEnemyWave(Constants.Enums.EnemyType.Static);
        SpawnEnemyWave(Constants.Enums.EnemyType.Dynamic);
        /*
        var unusedSpawns = SpawnPoints.Where(
            x => x.GetComponent<SpawnPoint>().Type == SpawnPoint.SpawnType.DynamicEnemy).ToList();

        var usedEnemiesDict = new Dictionary<ZoneDataContainer.EnemyEntry, int>();
        var prefabsCache = _normalEnemyPrefabs;
        for(int i = 0; i < EnemiesToSpawn; i++)
        {
            var spawnPoint = Helper.RandomInclusiveRange(0, unusedSpawns.Count - 1);
            var randomPrefab = Helper.RandomInclusiveRange(0, prefabsCache.Count - 1);

            Instantiate(
                prefabsCache[randomPrefab].Prefab,
                unusedSpawns[spawnPoint].transform.position,
                unusedSpawns[spawnPoint].transform.rotation);

            //Prevent enemies from spawning on top of each other
            unusedSpawns.RemoveAt(spawnPoint);

            //Remove enemies that have been used too much
            if(!usedEnemiesDict.ContainsKey(prefabsCache[randomPrefab]))
            {
                usedEnemiesDict.Add(prefabsCache[randomPrefab], 1);
            }
            else
            {
                usedEnemiesDict[prefabsCache[randomPrefab]] = 
                    usedEnemiesDict[prefabsCache[randomPrefab]] + 1;
            }

            if(usedEnemiesDict[prefabsCache[randomPrefab]] >= prefabsCache[randomPrefab].MaxPerWave)
            {
                prefabsCache.Remove(prefabsCache[randomPrefab]);
            }
        }
        */
        LogDebug($"Finished instantiating wave {_currentWave} of {WavesToSpawn}");
    }

    private void SpawnEnemyWave(Constants.Enums.EnemyType enemyType)
    {
        var unusedSpawns = SpawnPoints
            .Where(x => x.GetComponent<SpawnPoint>().EnemyType == enemyType).ToList();

        var usedEnemiesDict = new Dictionary<ZoneDataContainer.EnemyEntry, int>();
        var prefabsCache = _normalEnemyPrefabs.Where(x => x.EnemyType == enemyType).ToList();
        for(int i = 0; i < EnemiesToSpawn; i++)
        {
            var spawnPoint = Helper.RandomInclusiveRange(0, unusedSpawns.Count - 1);
            var randomPrefab = Helper.RandomInclusiveRange(0, prefabsCache.Count - 1);

            var prefab = prefabsCache[randomPrefab].Prefab;
            unusedSpawns[spawnPoint].GetComponent<SpawnPoint>().SpawnEnemy(prefab);
            
            /*
            Instantiate(
                prefabsCache[randomPrefab].Prefab,
                unusedSpawns[spawnPoint].transform.position,
                unusedSpawns[spawnPoint].transform.rotation);
            */

            //Prevent enemies from spawning on top of each other
            unusedSpawns.RemoveAt(spawnPoint);

            //Remove enemies that have been used too much
            if(!usedEnemiesDict.ContainsKey(prefabsCache[randomPrefab]))
            {
                usedEnemiesDict.Add(prefabsCache[randomPrefab], 1);
            }
            else
            {
                usedEnemiesDict[prefabsCache[randomPrefab]] =
                    usedEnemiesDict[prefabsCache[randomPrefab]] + 1;
            }

            if(usedEnemiesDict[prefabsCache[randomPrefab]] >= prefabsCache[randomPrefab].MaxPerWave)
            {
                prefabsCache.Remove(prefabsCache[randomPrefab]);
            }
        }
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

            Instantiate(
                _normalEnemyPrefabs[randomPrefab].Prefab,
                unusedSpawns[spawnPoint].transform.position,
                unusedSpawns[spawnPoint].transform.rotation);

            //Prevent enemies from spawning on top of each other
            unusedSpawns.RemoveAt(spawnPoint);
        }
    }
}
