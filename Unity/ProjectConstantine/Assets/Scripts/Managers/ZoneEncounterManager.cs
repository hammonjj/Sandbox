using System.Linq;
using UnityEngine;

public class ZoneEncounterManager : MonoBehaviourBase
{
    //Add randomness to this by picking a random way to divide the number of enemies by random spawn waves
    public int WavesToSpawn = 3;
    public int EnemiesToSpawn = 5;
    public int TimeBetweenWaves = 3;

    public GameObject[] BossEnemyPrefabs; //GetAvailableBossEnemies(Zone, Room);
    public GameObject[] EliteEnemyPrefabs; //GetAvailableEliteEnemies(Zone, Room);
    public GameObject[] NormalEnemyPrefabs; //GetAvailableNormalEnemies(Zone, Room);

    //For future - map all walkable area and use that as an area of spawn points
    public GameObject[] SpawnPoints;

    private int _currentWave;
    private int _currentlyDeadEnemies;
    private EventManager _eventManager;
    private SceneStateManager _sceneManager;

    private void Start()
    {
        _sceneManager = GameObject.FindGameObjectWithTag("SceneStateManager").GetComponent<SceneStateManager>();
        if(_sceneManager == null)
        {
            LogError("Failed to Acquire Scene State Manager");
        }

        var sceneType = _sceneManager.CurrentFightType;
        LogDebug($"Current Scene Type: {sceneType}");
        if(sceneType == Constants.Enums.FightType.None)
        {
            Destroy(gameObject);
            return;
        }

        _eventManager = EventManager.GetInstance();
        _eventManager.onEnemyDeath += OnEnemyDeath;

        SpawnPoints = GameObject.FindGameObjectsWithTag(Constants.Tags.SpawnPoint);
        if(SpawnPoints == null || SpawnPoints.Length == 0)
        {
            LogError("Unable to Locate Spawn Points");
        }

        //Encounters start with enemies already spawned
        InstantiateWave();
        //Future: Calculate the following number of waves
    }

    private void Update()
    {
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
            var randomPrefab = Helper.RandomInclusiveRange(0, NormalEnemyPrefabs.Length - 1);

            //LogDebug($"Spawning Enemy #{EnemiesToSpawn} - SpawnPoint Index: {spawnPoint} - SpawnPointCount: {unusedSpawns.Count}");
            //LogDebug($"Spawning Enemy #{EnemiesToSpawn} - RandomPrefab Index: {randomPrefab} - NormalEnemyPrefabsLength: {NormalEnemyPrefabs.Length}");

            Instantiate(
                NormalEnemyPrefabs[randomPrefab],
                unusedSpawns[spawnPoint].transform.localPosition, //Need to add height (Y) to prevent from spawning in floor
                unusedSpawns[spawnPoint].transform.rotation);

            //Prevent enemies from spawning on top of each other
            unusedSpawns.RemoveAt(spawnPoint);
        }

        LogDebug($"Finished instantiating wave {_currentWave} of {WavesToSpawn}");
    }
}
