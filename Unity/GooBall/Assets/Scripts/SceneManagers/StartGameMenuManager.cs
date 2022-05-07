using UnityEngine.SceneManagement;

public class StartGameMenuManager : MonoBehaviourBase
{
    // Start is called before the first frame update
    public void Load(int load)
    {
        GameInformation.CurrentGame = load;
        if(SaveSystem.DoesSaveExist(load))
        {
            LogDebug("Loading: " + load);
            GameInformation.IsNewGame = false;
        }
        else
        {
            LogDebug("Starting New Game");
            GameInformation.IsNewGame = true;
        }

        SceneManager.LoadScene("TesterLevel");
    }
}
