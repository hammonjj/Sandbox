using UnityEngine;
using static Enums;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviourBase
{
    public static PlayerController Instance { get; private set; }

    public GameManager GameManager;
    public GameObject BlobletPrefab;
    public Transform BlobletSpawn;

    public float MaxSpeed = 3.4f;
    public float GravityScale = 1.5f;
    public Camera MainCamera;

    private bool _facingRight = true;
    private bool _isGrounded = false;
    private float _moveDirection = 0;
    
    private Vector3 _cameraPos;
    private Rigidbody2D _rigidBody;
    private Collider2D _mainCollider;

    private Animator _animator;
    private PlayerControls _playerControls;

    //Check every collider except Player and Ignore Raycast
    private LayerMask _layerMask = ~(1 << 2 | 1 << 8);
    private Transform _transform;

    //Ability Toggles
    public bool CanDash = true;
    public bool CanWallJump = true;
    public bool CanDoubleJump = true;
    public bool CanEnemyBounce = true;
    public bool CanFloat = true;

    //Dashing
    public float DashSpeed = 6.5f;
    public float DashStartTime = 0.1f;

    private bool _isDashing = false;
    private bool _hasAirDashed = false;
    private float _dashTime;

    //Jumping
    public float JumpHeight = 6.5f;
    public float WallJumpHeight = 3.0f;
    private bool _hasDoubleJumped = false;

    //WallJumping and Sliding
    public float WallSlidingSpeed = 5.0f;
    public float XWallJumpForce;
    public float YWallJumpForce;
    public float WallJumpTime = 0.4f;
    private bool _isWallSliding = false;
    private bool _isOnClimeableWall = false;

    //Player Health
    public int MaxHealth;
    public int CurrentHealth;
    public float BlobletVerticalVelocity = 6.5f;
    public float BlobletHorizontalVelocity = 6.5f;

    private enum WallCollision
    {
        None,
        Left,
        Right
    }

    private WallCollision _wallCollision;

    private enum TypeOfJump
    {
        Normal,
        WallJumpClimb
    }

    //Floating
    public float FloatFallSpeed = 0.4f;
    private bool _isFloating = false;
    private bool _isWallJumping = false;

    public void EnableAbility(Ability ability)
    {
        LogDebug("Enabling Ability: " + ability.ToString());
        switch(ability)
        {
            case Ability.WallJump:
                CanWallJump = true;
                break;
            case Ability.DoubleJump:
                CanDoubleJump = true;
                break;
            case Ability.Dash:
                CanDash = true;
                break;
            case Ability.Bash:
                throw new System.Exception("Not Implemented Yet");
                break;
            case Ability.Float:
                CanFloat = true;
                break;
        }
    }

    private void Awake()
    {
        Instance = this;

        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Jump.performed += ctx => OnJump();
        _playerControls.Gameplay.Float.performed += ctx => OnFloatBegin();
        _playerControls.Gameplay.Float.canceled += ctx => OnFloatEnd();
    }

    private void OnFloatEnd()
    {
        _isFloating = false;
    }

    private void OnFloatBegin()
    {
        if(!_isGrounded && !_isOnClimeableWall && CanFloat)
        {
            _isFloating = true;
        }
    }

    private void Start()
    {
        _transform = transform;
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.freezeRotation = true;
        _rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rigidBody.gravityScale = GravityScale;
        _mainCollider = GetComponent<Collider2D>();
        _facingRight = _transform.localScale.x > 0;
        gameObject.layer = 8;
        _animator = GetComponent<Animator>();

        _dashTime = DashStartTime;
        _wallCollision = WallCollision.None;

        if(MainCamera)
        {
            _cameraPos = MainCamera.transform.position;
        }
    }

    //Update is called once per frame
    private void Update()
    {
        //Movement controls
        var movement = _playerControls.Gameplay.Move.ReadValue<float>();
        if(movement != 0 && !_isDashing)
        {
            _moveDirection = movement < 0 ? -1 : 1;
        }
        else
        {
            //This code makes landings not feel good
            //On second thought, this might be ok. I tried this in Ori and she appears to drop
            //straight down. To make it look like she has a bit of a curve, they added a rounded
            //animation and a particle effect where the trail behind her makes a little curve
            _moveDirection = 0;

            //This code makes changing direction mid air not feel good
            //if(_isGrounded || _rigidBody.velocity.magnitude < 0.01f)
            //{
            //    _moveDirection = 0;
            //}
        }

        //Change facing direction
        if(_moveDirection != 0)
        {
            if(_moveDirection > 0 && !_facingRight)
            {
                _facingRight = true;
                _transform.localScale = new Vector3(Mathf.Abs(
                    _transform.localScale.x), _transform.localScale.y, transform.localScale.z);
            }
            if(_moveDirection < 0 && _facingRight)
            {
                _facingRight = false;
                _transform.localScale = new Vector3(-Mathf.Abs(
                    _transform.localScale.x), _transform.localScale.y, _transform.localScale.z);
            }
        }

        //Wall Jumping
        if(_isWallJumping)
        {
            var force = XWallJumpForce * (_wallCollision == WallCollision.Left ? 1 : -1);
            _rigidBody.velocity = new Vector2(force, YWallJumpForce);
        }

        //Mid Air Dash
        var dashingPressed = _playerControls.Gameplay.Dash.ReadValue<float>();
        if(((dashingPressed > 0 && !_isGrounded && !_hasAirDashed) || _isDashing) && CanDash)
        {
            _isDashing = true;
            _hasAirDashed = true;

            Dash();
        }

        //Wall Slide
        if(_isOnClimeableWall && !_isGrounded)
        {
            _isWallSliding = true;
            _hasDoubleJumped = false;

            if(_isWallJumping)
            {
                _rigidBody.velocity = new Vector2(
                    _rigidBody.velocity.x,
                    Mathf.Clamp(_rigidBody.velocity.y, -WallSlidingSpeed, float.MaxValue));
            }
            else
            {
                _rigidBody.velocity = new Vector2(
                    _moveDirection * _rigidBody.velocity.x,
                    Mathf.Clamp(_rigidBody.velocity.y, -WallSlidingSpeed, float.MaxValue));
            }
        }
        else
        {
            _isWallSliding = false;
        }

        //Camera follow
        if(MainCamera)
        {
            MainCamera.transform.position = new Vector3(
                _transform.position.x, _transform.position.y, _cameraPos.z);
        }
    }

    private void FixedUpdate()
    {
        var colliderBounds = _mainCollider.bounds;
        var groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, 0.1f, 0);

        //Check if player is grounded
        var oldIsGrounded = _isGrounded;
        _isGrounded = Physics2D.OverlapCircle(groundCheckPos, 0.23f, _layerMask);

        if(!oldIsGrounded && _isGrounded)
        {
            _hasAirDashed = false;
            _hasDoubleJumped = false;

            _animator.SetTrigger("OnJump");
        }

        //Apply movement velocity
        if(!_isDashing && !_isWallSliding)
        {
            if(_isFloating)
            {
                _rigidBody.velocity = new Vector2(
                    _moveDirection * MaxSpeed, 
                    Mathf.Clamp(_rigidBody.velocity.y, -FloatFallSpeed, float.MaxValue));
            }
            else
            {
                _rigidBody.velocity = new Vector2(_moveDirection * MaxSpeed, _rigidBody.velocity.y);
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Climeable"))
        {
            _hasAirDashed = false;
            _isOnClimeableWall = true;

            //Check which side of the player is hitting the wall
            var hit = col.contacts[0].normal;
            var angle = Vector3.Angle(hit, Vector3.up);
            if(Mathf.Approximately(angle, 90))
            {
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                if(cross.y > 0)
                {
                    _wallCollision = WallCollision.Left;
                }
                else
                {
                    _wallCollision = WallCollision.Right;
                }
            }
        }

        if(col.gameObject.CompareTag("Enemy"))
        {
            //Take Damage
        }

        if(col.gameObject.CompareTag("EnemyHead"))
        {
            //Bash
            if(!_isGrounded)
            {     
                Jump();
            }
        }

        if(col.gameObject.CompareTag("Bloblet"))
        {
            _AbsordBloblet();
        }

        if(col.gameObject.CompareTag("Hazard"))
        {
            LogDebug("Hazard Hit");
            _TakeDamage(col.gameObject.GetComponent<Hazard>().DamagePerHit);
        }
    }

    private void _AbsordBloblet()
    {
        CurrentHealth++;
    }

    private void _TakeDamage(int damagePerHit)
	{
        CurrentHealth -= damagePerHit;

        if(CurrentHealth <= 0)
		{
            GameManager.LoadGame();
        }
		else
		{
            //Play hit animation
            var bloblet = Instantiate(BlobletPrefab);
            bloblet.transform.position = BlobletSpawn.position;

            var blobletRigidBody = bloblet.GetComponent<Rigidbody2D>();
            blobletRigidBody.velocity = new Vector2(
                Random.Range(-1.0f, 1.0f) * BlobletHorizontalVelocity, BlobletVerticalVelocity);

            var attractor = bloblet.GetComponent<Attractor>();
            attractor.PlayerTransform = transform;
        }
	}

	private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Climeable"))
        {
            _isOnClimeableWall = false;
            _wallCollision = WallCollision.None;
        }

        if(col.gameObject.CompareTag("Enemy"))
        {

        }
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void OnJump()
    {
        if(_isGrounded)
        {
            Jump();
        }
        else if(!_hasDoubleJumped && !_isGrounded && CanDoubleJump && !_isOnClimeableWall)
        {
            Jump();
            _hasDoubleJumped = true;
        }
        else if(_isWallSliding)
        {
            var jumpDirection = _playerControls.Gameplay.Move.ReadValue<float>();
            //Wall Jump - make a variable for wall jumping - need more lift when on a wall
            if(_wallCollision == WallCollision.Left && jumpDirection < 0 ||
                _wallCollision == WallCollision.Right && jumpDirection > 0)
            {
                Jump();
            }
            else if(_wallCollision == WallCollision.Left && jumpDirection > 0 ||
                _wallCollision == WallCollision.Right && jumpDirection < 0)
            {
                //Jump away from wall
                _isWallJumping = true;
                Invoke(nameof(SetWallJumpingToFalse), WallJumpTime);
            }
        }
    }

    private void SetWallJumpingToFalse()
    {
        _isWallJumping = false;
    }

    private void Dash()
    {
        if(_dashTime <= 0)
        {
            _isDashing = false;
            _dashTime = DashStartTime;
            _rigidBody.velocity = Vector2.zero;
        }
        else
        {
            _dashTime -= Time.deltaTime;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x * DashSpeed, _rigidBody.velocity.y);
        }
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _isWallSliding ? WallJumpHeight : JumpHeight);
    }
}