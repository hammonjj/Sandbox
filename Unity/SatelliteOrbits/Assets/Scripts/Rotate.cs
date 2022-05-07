using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float RotationSpeed = 10.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    transform.Rotate(Vector3.up, RotationSpeed, Space.World);
    }
}
