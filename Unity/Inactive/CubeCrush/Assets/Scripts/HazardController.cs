using UnityEngine;

public class HazardController : MonoBehaviour
{
    public GameObject HazardParticles;
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Game over
            _gameController.PlayerDied();
        }
        else if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Add one to game score
            _gameController.IncreaseScore();
            Instantiate(HazardParticles, transform.localPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
