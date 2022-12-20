using UnityEngine;

public class GameStateManager : MonoBehaviourBase
{
    public GameObject PauseMenu;

    public bool IsGamePaused { get; private set; }

    public void Awake()
    {
        
    }

    public void PauseOrUnpauseGame()
    {
        IsGamePaused = !IsGamePaused;

        Time.timeScale = IsGamePaused ? 0f : 1f;
        AudioListener.pause = IsGamePaused;

        PauseMenu.SetActive(IsGamePaused);
        LogDebug($"Game Pause: {IsGamePaused}");
    }

    public void OnPlayerDeath()
    {
        LogDebug("Player has died");
    }
}
