using UnityEngine;

public class PauseMenu : MonoBehaviourBase
{
    public GameStateManager GameStateManager;

    public void ResumeGame()
    {
        LogDebug("Resume Game Pressed");
        GameStateManager.PauseOrUnpauseGame();
    }

    public void OpenOptionsMenu()
    {
        LogDebug("Options Menu Pressed");
    }

    public void QuitGame()
    {
        LogDebug("Quit Game Pressed");
        Application.Quit();
    }
}
