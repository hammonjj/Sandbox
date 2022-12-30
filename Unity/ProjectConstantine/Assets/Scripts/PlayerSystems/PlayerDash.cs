using Constantine;
using UnityEngine;

public class PlayerDash : MonoBehaviourBase
{
    [Tooltip("Dash speed of the character in m/s")]
    public float DashSpeed = 40f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to dash again. Set to 0f to instantly dash again")]
    public float DashTimeout = 1f;

    [Tooltip("The amount of time that the character will dash for")]
    public float DashTime = 0.1f;

    //Used by Third Person Controller
    public bool IsDashing = false;

    //Internal Controls
    private bool _canDash = true;
    private float _totaldashTime;
    private float _dashTimeoutCurrent;

    private void Awake()
    {
        var playerInputs = GetComponent<PlayerInputs>();
        playerInputs.OnPlayerDash += OnDash;
    }

    private void Update()
    {
        _dashTimeoutCurrent -= Time.deltaTime;

        if(IsDashing)
        {
            _totaldashTime += Time.deltaTime;
        }

        _canDash = _dashTimeoutCurrent <= 0.0f;
        IsDashing = !(_totaldashTime >= DashTime);
    }

    private void OnDash()
    {
        if(!_canDash)
        {
            return;
        }

        LogDebug("OnDash Called");

        _canDash = false;
        IsDashing = true;
        _totaldashTime = 0f;
        _dashTimeoutCurrent = DashTimeout;
        
        //Start dash animation
    }
}
