﻿using UnityEngine;

public class PrimaryOrbUpgradeTracker
{
	public bool CanCrit = false;
	public float CritPercent = 10f;
	public float CritModifier = 50f;

	public bool CanPassThroughEnemies = false;

	public int MaxOrbs = 3;
	public int AttackDamage = 10;
	public float OrbRespawnTime = 1.0f;

	public GameObject PrimaryOrbPrefab;

	public PrimaryOrbUpgradeTracker()
    {
		MaxOrbs = 3;
		AttackDamage = 10;
		OrbRespawnTime = 1.0f;

		CanPassThroughEnemies = false;
		CritPercent = 10f;
		CritModifier = 50f;

		CanCrit = false;
	}
}

