using System;
using System.Collections;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviourBase
{
    public bool IsGamePaused { get; private set; }
    public bool AdvanceScenePressed;
    public Constants.Enums.SceneType CurrentSceneType;
    public Constants.Enums.RoomReward CurrentRoomReward;

    private Constants.Enums.Zones CurrentZone;
    private GameObject _pauseMenu;
    private EventManager _eventManager;
    private GameStateManager _gameStateManager;
    private NavMeshSurface _navMeshSurface;

    public Constants.Enums.Zones GetCurrentZone()
    {
        DetermineZone(SceneManager.GetActiveScene().name);
        LogDebug($"SceneName: {SceneManager.GetActiveScene().name} - " +
            $"Current Zone: {CurrentZone}");
        return CurrentZone;
    }

    private void Awake()
    {
        ChooseRoomVariation();

        _pauseMenu = GameObjectExtensions.FindGameObjectWithTag(Constants.Tags.PauseMenu);
        if(_pauseMenu == null)
        {
            LogError("Pause Menu is null");
        }
        else
        {
            _pauseMenu.SetActive(false);
        }

        var scene = SceneManager.GetActiveScene().name;
        LogDebug("===============================================================================");
        LogDebug($"New Scene Loaded: {scene}");
        DetermineZone(scene);

        _gameStateManager = GameObject.FindGameObjectWithTag(Constants.Tags.GameStateManager).GetComponent<GameStateManager>();
        if(_gameStateManager == null)
        {
            LogError("Failed to acquire Game State Manager");
        }

        CurrentSceneType = _gameStateManager.NextSceneType;
        CurrentRoomReward = _gameStateManager.NextRoomReward;
    }

    private void Start()
    {
        _eventManager = EventManager.GetInstance();
        _eventManager.onPause += PauseOrUnpauseGame;
        _eventManager.onPlayerDeath += OnPlayerDeath;
        _eventManager.onEncounterEnded += OnEncounterEnded;
        _eventManager.onAdvanceScenePressed += OnAdvanceScenePressed;
    }

    public void OnEncounterEnded()
    {
        LogDebug("Encounter Ended");
        //Next:
        //  - Spawn chamber reward
        //  - Enable zone doors -> Doors already listen for OnEncounterEnded
    }

    public void OnAdvanceScenePressed(bool value)
    {
        AdvanceScenePressed = value;
    }

    private void ChooseRoomVariation()
    {
        //Decide how many doors this room will have
        //  - Check to see if OneDoor, TwoDoors or ThreeDoors exist in scene
        //  - Pick a number and enable that game object
        //LogDebug("Setting room doors");
        //One door is a given unless it's a boss fight
        LogDebug($"Choosing Room Variation");

        var variations = GameObjectExtensions.FindGameObjectsWithTag(Constants.Tags.RoomVariation);
        LogDebug($"Room variations found: {variations.Count}");

        if(variations.Count == 0 || variations.Count == 1)
        {
            LogDebug("Only one variation present");
            return;
        }

        var chosenVariation = Helper.RandomInclusiveRange(0, variations.Count - 1);
        LogDebug($"Chosen Variation: {chosenVariation}");
        for(int i = 0; i < variations.Count; i++)
        {
            variations[i].SetActive(i == chosenVariation);
        }

        _navMeshSurface =
            GameObject.FindGameObjectWithTag(Constants.Tags.NavMeshSurface).GetComponent<NavMeshSurface>();

        if(_navMeshSurface == null)
        {
            LogError("NavMeshSurface is null");
        }

        var playerObj = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
        playerObj?.SetActive(false);
        _navMeshSurface.BuildNavMesh();
        playerObj?.SetActive(true);
    }

    private void DetermineZone(string name)
    {
        var index = name.IndexOf("_");
        if(index == -1)
        {
            CurrentZone = Constants.Enums.Zones.None;
        }
        else
        {
            var zoneName = name.Substring(0, index);
            CurrentZone = (Constants.Enums.Zones)Enum.Parse(typeof(Constants.Enums.Zones), zoneName);
            LogDebug($"zoneName: {zoneName} - CurrentZone: {CurrentZone}");
        }
    }

    public void PauseOrUnpauseGame()
    {
        IsGamePaused = !IsGamePaused;

        Time.timeScale = IsGamePaused ? 0f : 1f;
        AudioListener.pause = IsGamePaused;

        _pauseMenu.SetActive(IsGamePaused);
        LogDebug($"Game Pause: {IsGamePaused}");
    }

    public void OnPlayerDeath()
    {
        LogDebug("Player has died");
        //All objects needs to save themselves if they have trackable items
        //  - Out of run currency
        //  - States
        //  - etc.
        //Play death animation and add any story elements that need to be run

        //Fire off onGameReset and have the game state manager handle game reset
        //  - Adding short delay for the moment to simulate ending animation
        StartCoroutine(FireGameResetEvent());
    }

    private IEnumerator FireGameResetEvent()
    {
        LogDebug("Starting wait for game reset");
        yield return new WaitForSeconds(2);
        LogDebug("Retruning from wait");
        _eventManager.OnGameReset();
    }

    public void AdvanceToScene(
        Constants.Enums.Scenes sceneName, 
        Constants.Enums.RoomReward nextRoomReward, 
        Constants.Enums.SceneType sceneType)
    {
        LogDebug($"Leaving Scene: {SceneManager.GetActiveScene().name} - " +
            $"Loading Scene: {sceneName} - Next Scene Reward: {nextRoomReward}");

        //Store player data that needs to move to next scene
        EventManager.GetInstance().OnSceneEnding();

        //Remove chambers that have been used so they won't appear again in the same run
        if(CurrentZone != Constants.Enums.Zones.None && 
            (sceneType == Constants.Enums.SceneType.Fight || sceneType == Constants.Enums.SceneType.Elite)) 
        {
            _gameStateManager.AvailableZoneFightChambers.Remove(sceneName);
        }

        _gameStateManager.NextSceneType = sceneType;
        _gameStateManager.NextRoomReward = nextRoomReward;
        
        SceneManager.LoadScene(sceneName.ToString());
    }
}
