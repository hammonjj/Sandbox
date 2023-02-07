using UnityEngine;

public class ForceField : MonoBehaviourBase
{
    public int DamageToAbsorb;
    public float Duration;

    private int _currentDamage;

    private void Start()
    {
        Destroy(gameObject, Duration);
    }

    private void Update()
    {
        if(_currentDamage >= DamageToAbsorb)
        {
            LogDebug("Shield max damage reached: Destroying");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(Constants.Tags.Projectile))
        {
            var projectileBase = other.gameObject.GetComponent<EnemyProjectileAttackBase>();
            _currentDamage += projectileBase.AttackData.ProjectileDamage;

            LogDebug($"Damage Absorbed: {projectileBase.AttackData.ProjectileDamage}");
        }
    }
}
