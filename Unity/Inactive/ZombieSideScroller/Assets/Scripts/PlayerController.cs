using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float RunSpeed;
    public float WalkSpeed;

    private bool _facingRight;
    private Rigidbody _rigidbody;
    private Animator _animator;

    //For Jumping
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    public float JumpHeight;
    private bool _grounded;
    private Collider[] _groundCollisions;
    private float _groundCheckRadius = 0.2f;
    
	private void Start()
	{
	    _facingRight = true;
	    _rigidbody = GetComponent<Rigidbody>();
	    _animator = GetComponent<Animator>();
	}
	
	private void Update()
    {
		
	}

    private void FixedUpdate()
    {
        //Perform Jump
        if(_grounded && Input.GetAxis("Jump") > 0)
        {
            _grounded = false;
            _animator.SetBool("Grounded", _grounded);
            _rigidbody.AddForce(new Vector3(0, JumpHeight, 0));
        }

        //Check if Grounded
        _groundCollisions = Physics.OverlapSphere(GroundCheck.position, _groundCheckRadius, GroundLayer);
        _grounded = _groundCollisions.Length > 0;

        _animator.SetBool("Grounded", _grounded);

        //Check Speed
        var move = Input.GetAxis("Horizontal");
        _animator.SetFloat("Speed", Mathf.Abs(move));

        //Check Walk
        var sneaking = Input.GetAxisRaw("Fire3");
        _animator.SetFloat("Sneaking", sneaking);

        if(sneaking > 0 && _grounded)
        {
            _rigidbody.velocity = new Vector3(move * WalkSpeed, _rigidbody.velocity.y, 0);
        }
        else
        {
            _rigidbody.velocity = new Vector3(move * RunSpeed, _rigidbody.velocity.y, 0);
        }
        
        if(move > 0 && !_facingRight)
        {
            _Flip();
        }
        else if(move < 0 && _facingRight)
        {
            _Flip();
        }
    }

    private void _Flip()
    {
        _facingRight = !_facingRight;

        var scale = transform.localScale;
        scale.z = scale.z * -1;
        transform.localScale = scale;
    }
}
