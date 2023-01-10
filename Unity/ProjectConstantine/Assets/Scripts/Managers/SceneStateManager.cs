using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviourBase
{
    public bool IsGamePaused { get; private set; }
    public bool AdvanceScenePressed;
    public Constants.SceneType CurrentSceneType;
    public Constants.Zones CurrentZone;

    private GameObject _pauseMenu;

    private void Awake()
    {
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
        DetermineZone(scene);
        DetermineSceneType(scene);
    }

    private void DetermineZone(string name)
    {
        var index = name.IndexOf("_");
        var zoneName = name.Substring(0, index);

        LogDebug($"zoneName: {zoneName} - CurrentZone: {CurrentZone}");
    }

    private void DetermineSceneType(string scene)
    {
        var sceneName = scene.Substring(scene.IndexOf('_') + 1);

        CurrentSceneType = (Constants.SceneType)Enum.Parse(typeof(Constants.SceneType), sceneName);
        LogDebug($"sceneName: {sceneName} - Scene Type: {CurrentSceneType}");
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

    public void AdvanceToScene(Constants.Scenes sceneName)
    {
        LogDebug($"Leaving Scene: {SceneManager.GetActiveScene().name} - Loading Scene: {sceneName}");
        SceneManager.LoadScene(sceneName.ToString());
    }
}
