using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourBase
{
    public string MainMenu;
    public GameObject OptionsScreen;
    public SceneStateManager GameStateManager;

    public void ResumeGame()
    {
        LogDebug("Resume Game Pressed");
        GameStateManager.PauseOrUnpauseGame();
    }

    public void OpenOptionsMenu()
    {
        LogDebug("Options Menu Pressed");
        OptionsScreen.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        LogDebug("Close Options Pressed");
        OptionsScreen.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        LogDebug("Return to Main Menu Pressed");
        SceneManager.LoadScene(MainMenu);
    }

    public void QuitGame()
    {
        LogDebug("Quit Game Pressed");
        Application.Quit();
    }
}
