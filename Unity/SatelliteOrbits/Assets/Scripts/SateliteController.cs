using UnityEngine;

public class SateliteController : MonoBehaviour
{
    public float ThrusterPower = 5.0f;
    public float StartingSpeed = 10.0f;

    private Rigidbody _rigidbody;

	void Start()
	{
	    _rigidbody = GetComponent<Rigidbody>();
	    _rigidbody.velocity = new Vector3(0, 0, StartingSpeed);
    }
	
	// Update is called once per frame
	void Update()
	{
#if UNITY_IOS || UNITY_ANDROID
	    if(Input.touches.Length > 0)
	    {
	        _rigidbody.AddForce(ThrusterPower * _rigidbody.velocity);
	    }
#else
        //Add propulsive force when the screen is tapped
        var input = Input.GetAxis("Jump");

	    if(input != 0)
	    {
            _rigidbody.AddForce(ThrusterPower * _rigidbody.velocity);
	    }
#endif
        //Remove fuel
    }
}
