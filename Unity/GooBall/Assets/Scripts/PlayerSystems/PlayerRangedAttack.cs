using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerRangedAttack : MonoBehaviourBase
{
    [Header("Attack")]
    public int ProjectileDamage = 1;
    public int ProjectileSelfDamage = 1;
    public float ProjectileSpeed = 6.5f;
    public float ProjectileDistance = 6.5f;
    public float TimeBetweenAttacks = 1.0f;
    public Transform BlobletSpawn;
    public GameObject ProjectilePrefab;

    private bool _canAttack = true;
    private PlayerHealth _playerHealth;
    private PlayerControls _playerControls;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Gameplay.Attack.performed += ctx => OnAttack();
    }

    private void Start()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnAttack()
    {
        if(!_canAttack)
        {
            return;
        }

        if(_playerHealth.CurrentHealth <= ProjectileSelfDamage)
        {
            //Play can't attack animation
            return;
        }

        _canAttack = false;
        Invoke(nameof(_AllowAttack), TimeBetweenAttacks);
        _playerHealth.TakeDamage(ProjectileSelfDamage, false);

        var bloblet = Instantiate(ProjectilePrefab);
        bloblet.transform.position = BlobletSpawn.position;

        var blobletAttack = bloblet.GetComponent<ProjectileAttack>();
        
        blobletAttack.ProjectileSelfDamage = ProjectileSelfDamage;
        blobletAttack.PlayerMovement = GetComponent<PlayerMovement>();
        
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void _AllowAttack()
    {
        _canAttack = true;
    }
}
