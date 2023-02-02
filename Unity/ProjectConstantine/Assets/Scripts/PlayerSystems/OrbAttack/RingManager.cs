public class RingManager : MonoBehaviourBase
{
    private Ring _primaryRing;
    private Ring _secondaryRing;

    private AbilityTracker _abilityTracker;

    //Need to take an object that has a mesh and an orbit around the player
    //Player will shoot these out and will have to re-charge them (reload)
    //They will spin around player and protect them
    //Similar to Syndra or Aurelion Sol
    private void Start()
    {
        EventManager.GetInstance().onSceneEnding += SceneEnding;
        _primaryRing = VerifyComponent<Ring>(Constants.Tags.PrimaryRing);
        _secondaryRing = VerifyComponent<Ring>(Constants.Tags.SecondaryRing);

        //Update ring/orb status
        _abilityTracker = VerifyComponent<AbilityTracker>(Constants.Tags.GameStateManager);
        
        _primaryRing.MaxOrbs = _abilityTracker.PrimaryOrbUpgradeTracker.MaxOrbs;

        _secondaryRing.MaxOrbs = _abilityTracker.SecondaryMaxOrbs;
        _secondaryRing.enabled = _abilityTracker.SecondaryRingActive;

        _primaryRing.Initialize();
        _secondaryRing.Initialize();
    }

    private void SceneEnding()
    {
        LogDebug("Saving Ring Data");
        _abilityTracker.PrimaryOrbUpgradeTracker.MaxOrbs = _primaryRing.MaxOrbs;

        _abilityTracker.SecondaryRingActive = _secondaryRing.enabled;
        _abilityTracker.SecondaryMaxOrbs = _secondaryRing.MaxOrbs;
    }
}