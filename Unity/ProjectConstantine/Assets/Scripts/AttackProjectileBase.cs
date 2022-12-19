using UnityEngine;

public class AttackProjectileBase : MonoBehaviourBase
{
    [Header("AttackBase")]
    public float AttackSpeed = 20f;
    public float AttackRange = 10f;
    public float AttackDamage = 5f;

    private Vector3 _InitialPosition;

    private void Awake()
    {
        _InitialPosition = gameObject.transform.position;    
    }

    void Update()
    {
        if(Vector3.Distance(_InitialPosition, gameObject.transform.position) > AttackRange)
        {
            LogDebug("Projectile passed AttackRange - Destroying");
            Destroy(this);
        }

        //Update Position
        transform.Translate(Vector3.forward * Time.deltaTime * AttackSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Enemy")
        {
            //Destroy(this);
            return;
        } 
        else if(other.gameObject.tag == "Enemy")
        {
            LogDebug("Attack Hit Enemy");

            //Do damage to enemy
            //Destroy(this);
            return;
        }
    }
}
