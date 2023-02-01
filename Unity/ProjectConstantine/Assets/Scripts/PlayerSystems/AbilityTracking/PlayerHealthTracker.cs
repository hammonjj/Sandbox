using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthTracker", menuName = "Trackers/PlayerHealthTracker")]
public class PlayerHealthTracker : ScriptableObjectBase
{
    public int CurrentHealth;
    public int MaxHealth;
    public float EnemyDamageResistPercent;
}
