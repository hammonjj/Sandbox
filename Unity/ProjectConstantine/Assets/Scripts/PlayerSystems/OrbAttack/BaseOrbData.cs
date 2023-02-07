using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseOrbData", menuName = "Orbs/BaseOrbData")]
public class BaseOrbData : ScriptableObjectBase
{
    [Header("Base Orb")]
    public int AttackDamage = 10;
    public float AttackRange = 10f;
    public float ProjectileSpeed = 20f;

    public virtual void Initialize() 
    {
        EditorApplication.playModeStateChanged += OnPlayModeChange;
        EventManager.GetInstance().onPlayerDeath += OnPlayerDeath;
    }

    public virtual void OnMaxRangePassed(Vector3 currPos) { }

    //Returns true if the orb should be destroyed
    public virtual bool OnHit(Collider other, bool hasBeenFired)
    {
        if(!hasBeenFired)
        {
            return false;
        }

        if(other.tag == Constants.Tags.Enemy)
        {
            LogDebug("Orb Hitting Enemy (Fired)");
            var baseEnemy = other.GetComponent<BaseEnemy>();
            if(baseEnemy == null)
            {
                LogError($"Failed to get enemy base - {other.gameObject.name}");
                Debug.Break();
            }
            else
            {
                baseEnemy.TakeDamage(AttackDamage);
            }
        }
        else if(other.tag == Constants.Tags.Player)
        {
            return false;
        }

        return true;
    }

    protected virtual void OnPlayModeChange(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            LogDebug("Reseting BaseOrbData");
            AttackDamage = 10;
            AttackRange = 10f;
            ProjectileSpeed = 20f;
        }
    }

    private void OnPlayerDeath()
    {
        LogDebug("Reseting BaseOrbData");
        AttackDamage = 10;
        AttackRange = 10f;
        ProjectileSpeed = 20f;
    }
}
