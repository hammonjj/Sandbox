using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawn : MonoBehaviourBase
{
    public OrbSpawnData OrbSpawnData;

    private bool _initialized = false;
    private float _currentOrbRespawn;
    private GameObject _orbPrefab;
    private GameObject _orbStartPos;

    //Pulled from Orb prefab for convenience
    private float _attackRange;

    public void Initialize(GameObject orbPrefab)
    {
        GetUpgrades();
        _initialized = true;
        _orbPrefab = orbPrefab;
        _attackRange = _orbPrefab.GetComponent<BaseOrb>().BaseOrbData.AttackRange;
        _orbStartPos = GameObject.FindGameObjectWithTag(Constants.Tags.OrbStartPos);
    }

    private void Update()
    {
        if(!_initialized)
        {
            return;
        }

        if(!HasOrb())
        {
            _currentOrbRespawn -= Time.deltaTime;
            if(_currentOrbRespawn <= 0)
            {
                SpawnOrb();
                _currentOrbRespawn = OrbSpawnData.OrbRespawnRate;
            }
        }
    }

    public void Fire(Quaternion rotation)
    {
        if(!HasOrb())
        {
            LogError("No orb to fire");
            return;
        }

        var currentOrb = transform.GetChild(0);
        currentOrb.transform.parent = null;
        currentOrb.transform.rotation = rotation;
        var orb = currentOrb.GetComponent<BaseOrb>();
        orb.Fire(_orbStartPos.transform.position);
    }

    private void SpawnOrb()
    {
        LogDebug("Spawning Orb");
        var orb = Instantiate(_orbPrefab, transform.position, Quaternion.identity);
        orb.name = $"Orb-{Guid.NewGuid()}";
        orb.transform.SetParent(transform);
    }

    public bool HasOrb()
    {
        return transform.childCount > 0;
    }

    private void GetUpgrades()
    {
        /*
        var abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        var upgrades = abilityTracker.GetCurrentUpgrades(Constants.Enums.UpgradeType.PrimaryRing);
        
        foreach(var upgrade in upgrades)
        {
            switch(upgrade.AttackUpgrade)
            {
            }
        }
        */
    }
}
