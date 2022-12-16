using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviourBase
{
    public bool IsGamePaused { get; private set; }

    void Start()
    {   
    }

    void Update()
    {
    }

    public void PauseOrUnpauseGame()
    {
        IsGamePaused = !IsGamePaused;

        Time.timeScale = IsGamePaused ? 0f : 1f;
        AudioListener.pause = IsGamePaused;

        LogDebug($"Game Pause: {IsGamePaused}");
    }
}
