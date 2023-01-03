using UnityEngine;

[CreateAssetMenu(fileName = "Mutant", menuName = "Enemy/Mutant")]
public class Mutant : EnemyBaseObj
{
    public override void OnAttack() 
    {
    }

    public override int GetAttackAnimationID()
    {
        return PlayerConstants.AnimID_MutantAttack;
    }
}
