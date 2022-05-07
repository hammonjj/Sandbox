using UnityEngine;
using static Enums;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Door : MonoBehaviourBase
{
    [Header("Door")]
    public int KeysToOpen;

    private bool _doorOpen;
    private bool _playerInRange;
    private Collider2D _collider;
    private SpriteRenderer _renderer;
    private Inventory _playerInventory;
    private PlayerControls _playerControls;

    // Start is called before the first frame update
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!_playerInRange || _doorOpen)
        {
            return;
        }

        var interact = _playerControls.Gameplay.Interact.ReadValue<float>();
        if(interact > 0 && _playerInventory.KeyCount >= KeysToOpen)
        {
            _doorOpen = true;
            RemovePlayerKeys();
            _collider.enabled = false;
            _renderer.enabled = false;
        }
    }

    private void RemovePlayerKeys()
    {
        for(int i = 0; i < KeysToOpen; ++i)
        {
            _playerInventory.RemoveItem(ItemType.Key);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            _playerInRange = true;
            _playerInventory = col.gameObject.GetComponent<Inventory>();
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            _playerInRange = false;
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
