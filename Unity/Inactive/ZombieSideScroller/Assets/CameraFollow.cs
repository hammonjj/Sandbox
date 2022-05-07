using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float Smoothing = 5f;

    private Vector3 _offset;

	// Use this for initialization
	void Start()
	{
	    _offset = transform.position - Target.position;
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}

    void FixedUpdate()
    {
        var targetCameraPosition = Target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetCameraPosition, Smoothing); //* Time.deltaTime
    }
}
