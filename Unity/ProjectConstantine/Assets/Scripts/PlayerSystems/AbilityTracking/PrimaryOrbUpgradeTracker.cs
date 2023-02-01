using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PrimaryOrbUpgradeTracker", menuName = "Trackers/PrimaryOrbUpgradeTracker")]
public class PrimaryOrbUpgradeTracker : ScriptableObjectBase
{
	[Header("Critical Hits")]
	public bool CanCrit;
	public float CritPercent;
	public float CritModifier;

	[Header("Movement Modifiers")]
	public bool CanPassThroughEnemies;

	[Header("Base Stats")]
	public int MaxOrbs;
	public int AttackDamage;
	public float OrbRespawnTime;
}

