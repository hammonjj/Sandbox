using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool GameStarted;
    public GameObject TapToStartText;

    private int _coinsCaptured;

    void Start()
    {
        SceneData.CoinsCaptured = 0;
        SceneData.CurrentScene = SceneManager.GetActiveScene().buildIndex;
    }
	
	void Update()
	{
	    if(GameStarted)
	    {
	        return;
	    }

	    //Check mouse for editor debugging purposes
        if(Input.GetMouseButtonDown(0))
        {
            GameStarted = true;
            TapToStartText.SetActive(false);
        }
        else if(Input.touchCount >= 1)
        {
            GameStarted = true;
            TapToStartText.SetActive(false);
        }
	}

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayerWin()
    {
        SceneData.CoinsCaptured = _coinsCaptured;
        SceneManager.LoadScene("LevelComplete");
    }

    public void CoinCaptured()
    {
        ++_coinsCaptured;
    }
}
