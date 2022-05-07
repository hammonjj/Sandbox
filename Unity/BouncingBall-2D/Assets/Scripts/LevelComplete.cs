using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneData.CurrentScene);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneData.CurrentScene + 1);
    }
}
