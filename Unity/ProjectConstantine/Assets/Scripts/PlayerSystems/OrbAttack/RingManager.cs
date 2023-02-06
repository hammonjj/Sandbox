//This class can get deleted with the next refactor
public class RingManager : MonoBehaviourBase
{
    private Ring _primaryRing;
    private Ring _secondaryRing;

    private PlayerTracker _abilityTracker;

    //Need to take an object that has a mesh and an orbit around the player
    //Player will shoot these out and will have to re-charge them (reload)
    //They will spin around player and protect them
    //Similar to Syndra or Aurelion Sol
    private void Start()
    {
        _secondaryRing = VerifyComponent<Ring>(Constants.Tags.SecondaryRing);
        GetUpgrades();
    }

    private void GetUpgrades()
    {
        _abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        /*
        var upgrades = _abilityTracker.GetCurrentUpgrades(Constants.Enums.UpgradeType.PrimaryRing);
        foreach(var upgrade in upgrades)
        {
            switch(upgrade.AttackUpgrade)
            {
                case Constants.Enums.AttackUpgrade.PrimaryIncreaseOrbs:
                    _primaryRing.MaxOrbs++;
                    break;
            }
        }
        */
        _secondaryRing.enabled = _abilityTracker.SecondaryRingActive;
    }
}