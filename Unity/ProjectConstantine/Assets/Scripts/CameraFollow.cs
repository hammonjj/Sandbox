using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ObjectToFollow;

    private Transform _transform;

    private float offset_x;
    private float offset_y;
    private float offset_z;

    void Start()
    {
        _transform = gameObject.transform;

        //Need to distance
        offset_z = _transform.position.z;
        offset_x = ObjectToFollow.transform.position.x - _transform.position.x;
        offset_y = ObjectToFollow.transform.position.y + _transform.position.y;
    }

    void Update()
    {
        var newPos = new Vector3(
            ObjectToFollow.transform.position.x - offset_x,
            ObjectToFollow.transform.position.y + offset_y,
            ObjectToFollow.transform.position.z + offset_z);

        _transform.position = newPos;
    }
}
