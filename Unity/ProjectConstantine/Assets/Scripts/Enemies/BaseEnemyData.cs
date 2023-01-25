using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyData", menuName = "Enemy/BaseEnemyData")]
public class BaseEnemyData : ScriptableObject
{
    [Header("Base")]
    public string Name;
    public int MaxHealth = 100;

    [Header("Movement")]
    public float MovementSpeed = 2f;
    public float DetectionRange = 5f;
}
