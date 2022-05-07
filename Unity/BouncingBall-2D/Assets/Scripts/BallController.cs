using UnityEngine;

public class BallController : MonoBehaviour
{
    public float Velocity = 10.0f;

    private bool _ballStarted = false;
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>(); 
	}

    private void Update()
    {
        if(!_gameController.GameStarted)
        {
            return;
        }
        else if(_gameController.GameStarted && !_ballStarted)
        {
            _ballStarted = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, Velocity);
        }
        else if(_ballStarted)
        {
            //GetComponent<Rigidbody2D>().velocity.magnitude = Velocity;
        }
    }
}
