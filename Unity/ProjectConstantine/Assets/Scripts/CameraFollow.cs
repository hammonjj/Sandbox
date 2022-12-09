using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ObjectToFollow;

    private transform _transform;

    private float follow_x;
    private float initial_y;
    private float initial_z; //Fixed Camera Height

    private float follow_y;

    void Start()
    {
        _transform = gameObject.transform;

        initial_y = _transform.position.y;
        initial_z = _transform.position.z;

        //Get Current Offsets
        follow_x = ObjectToFollow.transform.position.x;
        initial_y = ObjectToFollow.transform.position.y + _transform.position.y;
    }

    void Update()
    {
        //Move camera to follow player
        _transform.position.z = initial_z;
        _transform.position.x = ObjectToFollow.transform.position.x;
        _transform.position.y = ObjectToFollow.transform.position.y + initial_y;
    }
}
