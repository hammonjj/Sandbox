using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float[] RingRadius;
    public float[] RingOrbitSpeed;
    public GameObject CenterToOrbit;

    private int _currentRing = 3;
    
	private void Update()
	{
	    if(Input.GetKeyDown("w") || Input.GetKeyDown("s"))
	    {
	        _GetPlayerInput();
	    }
	    else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
	    {
	        _GetPlayerInput();
        }

        transform.RotateAround(
	        CenterToOrbit.transform.position, Vector3.back, RingOrbitSpeed[_currentRing] * Time.deltaTime);
	    var desiredPosition = (transform.position - CenterToOrbit.transform.position).normalized *
	        RingRadius[_currentRing] + CenterToOrbit.transform.position;
	    transform.position = desiredPosition;
    }

    private void _GetPlayerInput()
    {
        //Touch Controls
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.position.x < Screen.height / 2)
            {
                --_currentRing;
            }
            else if (touch.position.x >= Screen.height / 2)
            {
                ++_currentRing;
            }
        }
        else
        {
            //Debug - Unity Editor
            var playerMove = Input.GetAxis("Vertical");
            if (playerMove < 0 && _currentRing > 0)
            {
                --_currentRing;
            }
            else if (playerMove > 0 && _currentRing < 4)
            {
                ++_currentRing;
            }
        }
    }
}
