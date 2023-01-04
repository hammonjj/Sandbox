using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthObj", menuName = "PlayerHealthObj")]
public class PlayerHealthObj : ScriptableObject
{
    public int MaxHealth = 100;
    public int CurrentHealth = 100;
}
