using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviourBase
{
    public Ring PrimaryRing;
    public Ring SecondaryRing;

    private GameStateManager _gameStateManager;

    //Need to take an object that has a mesh and an orbit around the player
    //Player will shoot these out and will have to re-charge them (reload)
    //They will spin around player and protect them
    //Similar to Syndra or Aurelion Sol
    private void Start()
    {
        EventManager.GetInstance().onSceneEnding += SceneEnding;
        PrimaryRing = VerifyComponent<Ring>(Constants.Tags.PrimaryRing);
        SecondaryRing = VerifyComponent<Ring>(Constants.Tags.SecondaryRing);

        //Update ring/orb status
        _gameStateManager = VerifyComponent<GameStateManager>(Constants.Tags.GameStateManager);
        
        PrimaryRing.MaxOrbs = _gameStateManager.PrimaryMaxOrbs;
        PrimaryRing.enabled = _gameStateManager.PrimaryRingActive;

        SecondaryRing.MaxOrbs = _gameStateManager.SecondaryMaxOrbs;
        SecondaryRing.enabled = _gameStateManager.SecondaryRingActive;

        PrimaryRing.Initialize();
        SecondaryRing.Initialize();
    }

    private void SceneEnding()
    {
        LogDebug("Saving Ring Data");
        _gameStateManager.PrimaryRingActive = PrimaryRing.enabled;
        _gameStateManager.PrimaryMaxOrbs = PrimaryRing.MaxOrbs;

        _gameStateManager.SecondaryRingActive = SecondaryRing.enabled;
        _gameStateManager.SecondaryMaxOrbs = SecondaryRing.MaxOrbs;
    }
}