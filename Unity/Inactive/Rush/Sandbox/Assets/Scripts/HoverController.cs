using UnityEngine;

public class HoverController : MonoBehaviour
{
    public float HoverForce = 9.0f;
    public float HoverHeight = 2.0f;
    public float TurnStrength = 10.0f;
    public float BackwardAcceleration = 25.0f;
    public float ForwardAcceleration = 100.0f;

    private int _layerMask;
    private float _currentTurn;
    private float _deadZone = 0.1f;
    private float _currentThrust;
    private Rigidbody _rigidbody;

    // Use this for initialization
    void Start()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update()
    {
	    //Main Thrust
        _currentThrust = 0.0f;
        var accelerationAxis = Input.GetAxis("Vertical");


        if (accelerationAxis > 0)
        {
            _currentThrust = accelerationAxis * ForwardAcceleration;
        }
        else if(accelerationAxis < 0)
        {
            _currentThrust = accelerationAxis * BackwardAcceleration;
        }

        //Turning
        _currentTurn = 0.0f;
        var turnAxis = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        //Forward
        if(Mathf.Abs(_currentThrust) > 0)
        {
            _rigidbody.AddForce(-transform.forward * _currentThrust);
        }
    }
}
