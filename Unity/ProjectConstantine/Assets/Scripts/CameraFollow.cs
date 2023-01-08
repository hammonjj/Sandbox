using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float offset_x;
    private float offset_y;
    private float offset_z;
    private Transform _transform;
    private GameObject _playerToFollow;

    private void Awake()
    {
        _playerToFollow = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        _transform = gameObject.transform;

        //Need to distance
        offset_z = _transform.position.z;
        offset_x = _playerToFollow.transform.position.x - _transform.position.x;
        offset_y = _playerToFollow.transform.position.y + _transform.position.y;
    }

    private void Update()
    {
        var newPos = new Vector3(
            _playerToFollow.transform.position.x - offset_x,
            _playerToFollow.transform.position.y + offset_y,
            _playerToFollow.transform.position.z + offset_z);

        _transform.position = newPos;
    }
}
