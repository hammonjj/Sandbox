using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourBase
{
    public string FirstLevel;
    public GameObject OptionsScreen;

    public void StartGame()
    {
        LogDebug("Start Game Pressed");
        SceneManager.LoadScene(FirstLevel);
    }

    public void OpenOptions()
    {
        LogDebug("Open Options Pressed");
        OptionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        LogDebug("Close Options Pressed");
        OptionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        LogDebug("Quit Game Pressed");
        Application.Quit();
    }
}
