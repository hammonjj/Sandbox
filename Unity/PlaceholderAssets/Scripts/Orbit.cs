using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject ObjectToOrbit;

	private void Start() 
	{
	}
	
	private void Update() 
	{
	    transform.RotateAround(ObjectToOrbit.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
	
	private void FixedUpdate()
	{
	}
}
