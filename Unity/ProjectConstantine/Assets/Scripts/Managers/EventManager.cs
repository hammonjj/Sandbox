using System;

public class EventManager
{
    //Game Events
    public Action onEnemyDeath;
    public Action onPlayerDeath;
    public Action onEncounterEnded;

    //Player Inputs
    public Action onUseItem;
    public Action onPlayerDash;
    public Action onPlayerPrimaryAttack;
    public Action onPlayerSecondaryAttack;

    //Misc. Player Actions
    public Action onPause;
    public Action<bool> onAdvanceScenePressed;

    //Private Members
    private static EventManager _instance;

    public static EventManager GetInstance()
    {
        if(_instance == null)
        {
            _instance = new EventManager();
        }

        return _instance;
    }

    public void OnPlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }

    public void OnEnemyDeath()
    {
        onEnemyDeath?.Invoke();
    }

    public void OnUseItem()
    {
        onUseItem?.Invoke();
    }

    public void OnPlayerDash()
    {
        onPlayerDash?.Invoke();
    }

    public void OnPlayerPrimaryAttack()
    {
        onPlayerPrimaryAttack?.Invoke();
    }

    public void OnPlayerSecondaryAttack()
    {
        onPlayerSecondaryAttack?.Invoke();
    }

    public void OnAdvanceScenePressed(bool value)
    {
        onAdvanceScenePressed?.Invoke(value);
    }

    public void OnPause()
    {
        onPause?.Invoke();
    }

    public void OnEncounterEnded()
    {
        onEncounterEnded?.Invoke();
    }
}
