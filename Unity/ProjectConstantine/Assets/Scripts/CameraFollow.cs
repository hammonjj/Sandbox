using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject ObjectToFollow;

    private Transform _transform;

    private float initial_y;
    private float initial_z; //Fixed Camera Height

    void Start()
    {
        _transform = gameObject.transform;

        initial_z = _transform.position.z;
        initial_y = ObjectToFollow.transform.position.y + _transform.position.y;
    }

    void Update()
    {
        var newPos = new Vector3(
            ObjectToFollow.transform.position.x,
            ObjectToFollow.transform.position.y + initial_y,
            ObjectToFollow.transform.position.z + initial_z);

        _transform.position = newPos;
    }
}
