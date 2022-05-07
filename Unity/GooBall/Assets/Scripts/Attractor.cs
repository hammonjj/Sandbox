using UnityEngine;

public class Attractor : MonoBehaviourBase
{
    [Header("Attractor")]
    public float DespawnRange = 10.0f;
    public float AttractionRange = 3.0f;
    public float AttractionForce = 5.0f;

    public Transform PlayerTransform;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Check if too far away from player, if so despawn
        if(Vector2.Distance(PlayerTransform.position, _transform.position) > DespawnRange)
        {
            Destroy(gameObject);
        }

        //Don't start pulling until the blobs have come to a stop
        if(_rigidbody.velocity == new Vector2(0, 0) &&
            Vector2.Distance(PlayerTransform.position, _transform.position) < AttractionRange)
        {
            //Add bloblet crawling animation
            var direction = PlayerTransform.position - _transform.position;
            _rigidbody.AddForce(direction.normalized * AttractionForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            //Destroy(gameObject);
        }
    }
}
