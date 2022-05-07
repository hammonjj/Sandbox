using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float MoveForce = 10.0f;
    private Rigidbody _rigidbody;

    private GameController _gameController;

    void Start()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	    _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void FixedUpdate()
    {
        var inputHorizontal = Input.GetAxis("Horizontal");
        var inputVertical = Input.GetAxis("Vertical");

        var moveDir = new Vector3(inputHorizontal * MoveForce, 0, inputVertical * MoveForce);
        _rigidbody.AddForce(moveDir * MoveForce * Time.fixedDeltaTime);

        if(_rigidbody.velocity.y < -20)
        {
            Destroy(gameObject);
            _gameController.PlayerDied();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hazard"))
        {
            Destroy(gameObject);
        }
    }
}
