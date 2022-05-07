using UnityEngine;
using static Enums;

public class PlayerAbilities : MonoBehaviourBase
{
    [Header("Player Abilities")]
    public bool CanDash;
    public bool CanBash;
    public bool CanFloat;
    public bool CanWallJump;
    public bool CanDoubleJump;

    public void EnableAbility(Ability ability)
    {
        LogDebug("Enabling Ability: " + ability.ToString());
        switch(ability)
        {
            case Ability.WallJump:
                CanWallJump = true;
                break;
            case Ability.DoubleJump:
                CanDoubleJump = true;
                break;
            case Ability.Dash:
                CanDash = true;
                break;
            case Ability.Bash:
                CanBash = true;
                break;
            case Ability.Float:
                CanFloat = true;
                break;
        }
    }
}
