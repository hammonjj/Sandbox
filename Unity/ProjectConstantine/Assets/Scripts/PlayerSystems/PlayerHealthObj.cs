using UnityEngine;

//PossibleDelete
[CreateAssetMenu(fileName = "PlayerHealthObj", menuName = "PlayerHealthObj")]
public class PlayerHealthObj : ScriptableObjectBase
{
    public int MaxHealth = 100;
    public int CurrentHealth = 100;

    private void Awake()
    {
        EventManager.GetInstance().onPlayerDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        MaxHealth = 100;
        CurrentHealth = 100;
    }
}
