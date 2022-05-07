using UnityEngine;
using static Enums;

public class Shrine : MonoBehaviourBase
{
    [Header("Shrine")]
    public Ability Ability;

    private bool _shrineUsed;
    private bool _playerInRange;
    private PlayerControls _playerControls;
    private PlayerAbilities _playerAbilities;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            _playerInRange = true;
            _playerAbilities = col.gameObject.GetComponent<PlayerAbilities>();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }

    private void Update()
    {
        if(!_playerInRange || _shrineUsed)
        {
            return;
        }

        var interact = _playerControls.Gameplay.Interact.ReadValue<float>();
        if(interact > 0)
        {
            _shrineUsed = true;
            _playerAbilities.EnableAbility(Ability);
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
}
