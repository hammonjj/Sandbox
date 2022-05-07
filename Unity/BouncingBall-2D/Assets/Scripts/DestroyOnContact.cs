using System.Collections;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private GameController _gameController;
    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            StartCoroutine(_wait(col));
        }
    }

    private IEnumerator _wait(Collider2D col)
    {
        yield return new WaitForSeconds(2);
        Destroy(col.gameObject);
        _gameController.PlayerWin();
    }
}
