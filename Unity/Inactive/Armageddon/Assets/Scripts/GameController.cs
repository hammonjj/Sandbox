using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float MinMissileSpeed;
    public float MaxMissileSpeed;
    public GameObject EnemyMissile;
    public Vector3 EnemyMissileSource;
    public List<Vector3> CityLocations;
    

    private int _currentLevel = 1;
    private int _citiesAlive = 6;
    private int _missilesPerWave = 10;
    private int _numberOfWaves = 3;
    private int _spawnWait = 10;

    public void CityDestroyed()
    {
        --_citiesAlive;
    }

	void Start()
    {
        StartCoroutine(_SpawnWaves());
    }
	
	void Update()
    {
        if(_citiesAlive == 0)
        {
            //GameOver
        }
	}

    private IEnumerator _SpawnWaves()
    {
        yield return new WaitForSeconds(3);
        while(_numberOfWaves > 0)
        {
            for (var missiles = 0; missiles < _missilesPerWave; ++missiles)
            {
                Instantiate(
                    EnemyMissile,
                    new Vector3(
                        Random.Range(-EnemyMissileSource.x, EnemyMissileSource.x),
                        EnemyMissileSource.y,
                        0),
                    Quaternion.identity);
            }

            yield return new WaitForSeconds(_spawnWait);
        }
    }
}
