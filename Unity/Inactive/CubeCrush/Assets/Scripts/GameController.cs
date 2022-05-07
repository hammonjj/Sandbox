using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float SpawnWait;
    public float IncreaseSpawnPointsWait;
    public GameObject Hazard;
    public Vector3 SpawnValues;
    public GUIText ScoreText;
    public GUIText PlayInstructionText;

    private bool _playerAlive = true;
    private int _gameScore;
    private float _spawnPoints;

    public void PlayerDied()
    {
        _playerAlive = false;
    }

    public void IncreaseScore()
    {
        if(!_playerAlive)
        {
            return;
        }

        ++_gameScore;
        ScoreText.text = _gameScore.ToString();
    }

    private void Start()
    {
        ScoreText.text = "0";
        Physics.IgnoreLayerCollision(10, 10); //Hazards don't collide with each other
        StartCoroutine(_SpawnWaves());
        //StartCoroutine(_IncreaseSpawnPoints());   
    }

    private void Update()
    {
        if(!_playerAlive)
        {
            PlayInstructionText.text = "Press any key to replay!";
            PlayInstructionText.enabled = true;
            if (Input.anyKey)
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
    private IEnumerator _SpawnWaves()
    {
        PlayInstructionText.text = "Tilt Phone!";
        yield return new WaitForSeconds(3);
        PlayInstructionText.enabled = false;

        while (_playerAlive)
        {
            for(var spawns = 0; spawns < 1; ++spawns)
            {
                Instantiate(
                    Hazard,
                    new Vector3(
                        Random.Range(-77, 112),
                        SpawnValues.y,
                        Random.Range(-11, 26)),
                    Quaternion.identity);
            }
            
            yield return new WaitForSeconds(SpawnWait);
        }
    }

    private IEnumerator _IncreaseSpawnPoints()
    {
        while(_playerAlive)
        {
            ++_spawnPoints;
            yield return new WaitForSeconds(IncreaseSpawnPointsWait);
        }  
    }
}
