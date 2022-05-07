using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviourBase
{
    public void StartGame()
    {
        LogDebug("Start Game Pressed");
        SceneManager.LoadScene("StartGame");
    }

    public void Options()
    {
        LogDebug("Options Pressed");
    }

    public void QuitGame()
    {
        LogDebug("Quit Game Pressed");
    }
}
