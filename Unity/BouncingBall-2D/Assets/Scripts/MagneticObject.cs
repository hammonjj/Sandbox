using UnityEngine;

public class MagneticObject : MonoBehaviour
{
    public float ForceFactor = 10.0f;
    public float MagnetismRange = 10.0f;
    public Rigidbody2D Magnet;

    private bool _magnetLocked;
    private Rigidbody2D _rigidbody;

	private void Start()
	{
	    _rigidbody = GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate()
	{
	    if(_magnetLocked || 
	        Mathf.Abs((_rigidbody.position - Magnet.position).magnitude) < MagnetismRange)
	    {
	        _magnetLocked = true;
            _rigidbody.AddForce(
	            (Magnet.transform.position - transform.position) * ForceFactor);
	    }
    }
}
