using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public int CityPoints = 100;
    public int EnemyMissilePoints = 10;
    public int PlayerMissilePoints = 10;
    public GUIText ScoreText;

    private int _currentScore;

    public void EnemyMissileDestroyed()
    {
        _currentScore += EnemyMissilePoints;
    }

    public void CitySaved()
    {
        _currentScore += CityPoints;
    }

    public void PlayerMissilesSaved(int missiles)
    {
        _currentScore += PlayerMissilePoints * missiles;
    }

    void Start()
    {
		
	}

	void Update()
	{
	    ScoreText.text = _currentScore.ToString();
	}
}
