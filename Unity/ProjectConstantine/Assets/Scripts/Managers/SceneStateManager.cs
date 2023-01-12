using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviourBase
{
    public bool IsGamePaused { get; private set; }
    public bool AdvanceScenePressed;
    private Constants.Zones CurrentZone;
    public Constants.SceneType CurrentSceneType;
    public Constants.RoomReward CurrentRoomReward;

    private GameObject _pauseMenu;
    private EventManager _eventManager;
    private GameStateManager _gameStateManager;

    public Constants.Zones GetCurrentZone()
    {
        DetermineZone(SceneManager.GetActiveScene().name);
        LogDebug($"SceneName: {SceneManager.GetActiveScene().name} - " +
            $"Current Zone: {CurrentZone}");
        return CurrentZone;
    }

    private void Awake()
    {
        _eventManager = EventManager.GetInstance();
        _eventManager.onPause += PauseOrUnpauseGame;
        _eventManager.onPlayerDeath += OnPlayerDeath;
        _eventManager.onAdvanceScenePressed += OnAdvanceScenePressed;

        _pauseMenu = Extensions.FindGameObjectWithTag(Constants.PauseMenu);
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
        DetermineSceneType(scene);

        _gameStateManager = GameObject.FindGameObjectWithTag(Constants.GameStateManager).GetComponent<GameStateManager>();
        if(_gameStateManager == null)
        {
            LogError("Failed to acquire Game State Manager");
        }

        CurrentRoomReward = _gameStateManager.NextRoomReward;
    }

    public void OnAdvanceScenePressed(bool value)
    {
        LogDebug($"OnAdvanceScenePressed: {value}");
        AdvanceScenePressed = value;
    }

    private void DetermineZone(string name)
    {
        var index = name.IndexOf("_");
        if(index == -1)
        {
            CurrentZone = Constants.Zones.None;
        }
        else
        {
            var zoneName = name.Substring(0, index);
            CurrentZone = (Constants.Zones)Enum.Parse(typeof(Constants.Zones), zoneName);
            LogDebug($"zoneName: {zoneName} - CurrentZone: {CurrentZone}");
        }
    }

    private void DetermineSceneType(string scene)
    {
        var index = scene.IndexOf('_');
        if(index == -1)
        {
            CurrentSceneType = Constants.SceneType.None;
        }
        else
        {
            var sceneName = scene.Substring(index + 1);
            CurrentSceneType = (Constants.SceneType)Enum.Parse(typeof(Constants.SceneType), sceneName);
            LogDebug($"sceneName: {sceneName} - Scene Type: {CurrentSceneType}");
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
    }

    public void AdvanceToScene(Constants.Scenes sceneName, Constants.RoomReward nextRoomReward)
    {
        LogDebug($"Leaving Scene: {SceneManager.GetActiveScene().name} - " +
            $"Loading Scene: {sceneName} - Next Scene Reward: {nextRoomReward}");

        _gameStateManager.NextRoomReward = nextRoomReward;
        SceneManager.LoadScene(sceneName.ToString());
    }
}
