using UnityEngine;

public class ObjectiveCapture : MonoBehaviour
{
    private GameController _gameController;
    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _gameController.CoinCaptured();
        Destroy(gameObject);
    }
}
