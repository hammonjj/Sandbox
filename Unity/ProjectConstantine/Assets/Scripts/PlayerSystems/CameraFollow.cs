using UnityEngine;
//using static UnityEngine.GraphicsBuffer;
using UnityEngine.ProBuilder;

public class CameraFollow : MonoBehaviour
{
    public float MovementSmoothing = 3f;

    private Vector3 _offset;
    private GameObject _playerToFollow;

    private void Start()
    {
        _playerToFollow = GameObject.FindGameObjectWithTag(Constants.Player);
        _offset = transform.position - _playerToFollow.transform.position;
    }

    void LateUpdate()
    {
        Vector3 targetCamPos = _playerToFollow.transform.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, 5f * Time.deltaTime);
    }
}
