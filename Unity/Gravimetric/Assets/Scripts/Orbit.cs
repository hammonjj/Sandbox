using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float OrbitSpeed = 20.0f;
    public GameObject ObjectToOrbit;

	private void Update() 
	{
	    transform.RotateAround(ObjectToOrbit.transform.position, Vector3.back, OrbitSpeed * Time.deltaTime);
    }
}
