using UnityEngine;

public class ThrusterController : MonoBehaviour 
{
    public float ThrusterPower = 5.0f;

    private Rigidbody _rigidbody;

    private void Start() 
	{
	    _rigidbody = GetComponent<Rigidbody>();
    }
	
	private void Update() 
	{
	    var inputHorizontal = Input.GetAxis("Horizontal");
	    var inputVertical = Input.GetAxis("Vertical");

	    if(inputVertical != 0 || inputHorizontal != 0)
	    {
	        Debug.Log("Horizontal: " + inputHorizontal);
	        Debug.Log("Vertical: " + inputVertical);
	        var moveDir = new Vector3(inputHorizontal * ThrusterPower, 0, inputVertical * ThrusterPower);
	        _rigidbody.AddForce(moveDir * ThrusterPower);
        }
        
    }
	
	private void FixedUpdate()
	{
	}
}
