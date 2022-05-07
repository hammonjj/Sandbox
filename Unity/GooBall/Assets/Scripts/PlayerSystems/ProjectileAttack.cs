using UnityEngine;

public class ProjectileAttack : MonoBehaviourBase
{
    [Header("Projectile")]
    public float DistanceToTravel = 2.0f;
    public LayerMask _defaultMask;

    [HideInInspector]
    public int ProjectileSelfDamage;

    [HideInInspector]
    public PlayerMovement PlayerMovement;

    private bool _returning;
    private Vector2 _startPosition;

    private CircleCollider2D _collider;

    //Pathfinding
    private EnemyFollow _enemyFollow;

    private void Start()
    {
        _startPosition = transform.position;
        _enemyFollow = GetComponent<EnemyFollow>();
        _collider = GetComponent<CircleCollider2D>();

        var playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(playerCollider, _collider);

        //Find closest Enemy
        var enemyColliders = Physics2D.OverlapCircleAll(_startPosition, DistanceToTravel, _defaultMask);
        if(enemyColliders.Length == 0)
        {
            //Do something to fizzle out
            LogDebug("Found no enemies nearby");
            Destroy(gameObject);
            return;
        }
        else if(enemyColliders.Length == 1)
        {
            if(!enemyColliders[0].gameObject.CompareTag("Enemy"))
            {
                LogDebug("Found no enemies nearby");
                Destroy(gameObject);
                return;
            }

            LogDebug("Found one enemy nearby");
            _enemyFollow.TransformToFollow = enemyColliders[0].gameObject.transform;
            Physics2D.IgnoreCollision(enemyColliders[0].gameObject.GetComponent<Collider2D>(), _collider);
        }
        else
        {
            LogDebug("Found many possible enemies nearby");
            Transform closestEnemy = null;
            var foundFirstEnemy = false;
            for(int i = 1; i < enemyColliders.Length; i++)
            {
                if(!enemyColliders[0].gameObject.CompareTag("Enemy"))
                {
                    continue;
                }

                if(!foundFirstEnemy)
                {
                    foundFirstEnemy = true;
                    closestEnemy = enemyColliders[0].gameObject.transform;
                    continue;
                }

                if(Vector2.Distance(enemyColliders[i].transform.position, _startPosition) < 
                    Vector2.Distance(closestEnemy.position, _startPosition))
                {
                    closestEnemy = enemyColliders[i].transform;
                }
            }

            if(closestEnemy != null)
            {
                LogDebug("Found enemy");
                _enemyFollow.TransformToFollow = closestEnemy.transform;
                Physics2D.IgnoreCollision(closestEnemy.gameObject.GetComponent<Collider2D>(), _collider);
            }
            else
            {
                LogDebug("Found no enemies nearby");
                Destroy(gameObject);
                return;
            }
        }

        _enemyFollow.EnableFollow(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && _returning)
        {
            LogDebug("Hit Player");
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.GainHealth(ProjectileSelfDamage);
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            LogDebug("Hit Enemy");
            //Damage enemy and return
            _returning = true;
            _enemyFollow.TransformToFollow = PlayerMovement.transform;
        }
    }
}
