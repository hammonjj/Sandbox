using UnityEngine;
using static Enums;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAbilities))]
public class PlayerMovement : MonoBehaviourBase
{
    [Header("Movement")]
    public float MoveSpeed;
    public float WallJumpSpeed;
    public float VerticalJumpSpeed;
    public LayerMask GroundMask;

    public Direction LookDirection { get; set; }
    
    private bool _isGrounded;
    private bool _isTouchingLeft;
    private bool _isTouchingRight;
    private bool _hasDoubleJumped;
    private bool _WallJumpCoyoteTimeStarted;
    private Direction _moveDirection;
    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private PlayerControls _playerControls;
    private PlayerAbilities _playerAbilities;

    private bool _hasBashed;

    //Collider Boxes - Bottom, Left, and Right
    [Header("Collider Boxes")]
    public float GroundedBoxY = 0.36f;
    public float GroundedOverlapBoxX = 0.75f;
    public float GroundedOverlapBoxY = 0.02f;

    public float LeftBoxX = 0.37f;
    public float LeftOverlapBoxX = -0.2f;
    public float LeftOverlapBoxY = -0.68f;

    public float RightBoxX = 0.37f;
    public float RightOverlapBoxX = 0.02f;
    public float RightOverlapBoxY = 0.68f;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Look.canceled += ctx => OnLook();
        _playerControls.Gameplay.Look.performed += ctx => OnLook();
        _playerControls.Gameplay.Jump.performed += ctx => OnJump();
        //_playerControls.Gameplay.Dash.performed += ctx => OnDash();
        //_playerControls.Gameplay.Float.performed += ctx => OnFloatBegin();
        //_playerControls.Gameplay.Float.canceled += ctx => OnFloatEnd();
    }

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerAbilities = GetComponent<PlayerAbilities>();
    }

    private void Update()
    {
        CheckBoundingBoxes();

        var movement = GetMovement();
        _rigidbody.velocity = new Vector2(movement * MoveSpeed, _rigidbody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LogDebug("OnCollisionEnter2D");
        var bash = _playerControls.Gameplay.Bash.ReadValue<float>();
        if(bash > 0 && !_hasBashed)
        {
            _hasBashed = true;
            _Bash(collision);
        }
    }
    private void CheckBoundingBoxes()
    {
        var oldTouchingLeft = _isTouchingLeft;
        var oldTouchingRight = _isTouchingRight;
        
        _isGrounded = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - GroundedBoxY),
            new Vector2(GroundedOverlapBoxX, GroundedOverlapBoxY), 0f, GroundMask);

        if(_isGrounded)
        {
            _hasDoubleJumped = false;
        }

        _isTouchingLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - LeftBoxX, gameObject.transform.position.y),
            new Vector2(LeftOverlapBoxX, LeftOverlapBoxY), 0f, GroundMask);

        _isTouchingRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + RightBoxX, gameObject.transform.position.y),
            new Vector2(RightOverlapBoxX, RightOverlapBoxY), 0f, GroundMask);

        if((oldTouchingLeft && !_isTouchingLeft) || (oldTouchingRight && !_isTouchingRight))
        {
            //Gives time to hit the left or right movement, then jump. Makes for a better feeling wall jump
            _WallJumpCoyoteTimeStarted = true;
            Invoke(nameof(_SetCoyoteTimeToFalse), 0.12f);
        }
    }

    private void OnLook()
    {
        var look = _playerControls.Gameplay.Look.ReadValue<float>();
        if(look == 0)
        {
            //Player look "normal" animation
            LookDirection = _moveDirection;
        }
        else if(look > 0)
        {
            //Player Look Up Animation
            LookDirection = Direction.Up;
        }
        else
        {
            //Player Look Down Animation
            LookDirection = Direction.Down;
        }
    }

    private void OnJump()
    {
        LogVerbose("OnJump");
        if(_isGrounded)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, VerticalJumpSpeed);
        }
        else if(CheckWallSliding() && _playerAbilities.CanWallJump)
        {
            _rigidbody.velocity = new Vector2(0, VerticalJumpSpeed);
        }
        else if(_WallJumpCoyoteTimeStarted && _playerAbilities.CanWallJump)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, VerticalJumpSpeed);
        }
        else if(!_isGrounded && !_hasDoubleJumped && _playerAbilities.CanDoubleJump)
        {
            _hasDoubleJumped = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, VerticalJumpSpeed);
        }
    }

    private void _Bash(Collision2D collision)
    {
        LogVerbose("Bash");

        //_rigidBody.velocity = new Vector2(_rigidBody.velocity.x * DashSpeed, _rigidBody.velocity.y);

    }

    private bool CheckWallSliding()
    {
        //Debug.Log("IsGrounded: " + _isGrounded + " - IsTouchingLeft: " + _isTouchingLeft + " - IsTouchingRight: " + _isTouchingRight);
        return !_isGrounded && (_isTouchingLeft || _isTouchingRight);
    }

    private void _SetCoyoteTimeToFalse()
    {
        _WallJumpCoyoteTimeStarted = false;
    }

    private int GetMovement()
    {
        var movement = _playerControls.Gameplay.Move.ReadValue<float>();
        if(movement == 0)
        {
            return 0;
        }
        else if(movement > 0)
        {
            LookDirection = Direction.Right;
            _moveDirection = Direction.Right;
            return 1;
        }
        else
        {
            LookDirection = Direction.Left;
            _moveDirection = Direction.Left;
            return -1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - GroundedBoxY),
            new Vector2(GroundedOverlapBoxX, GroundedOverlapBoxY));

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - LeftBoxX, gameObject.transform.position.y),
            new Vector2(LeftOverlapBoxX, LeftOverlapBoxY));

        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + RightBoxX, gameObject.transform.position.y),
            new Vector2(RightOverlapBoxX, RightOverlapBoxY));

    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
