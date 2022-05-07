using System;
using UnityEngine;

public class GameManager : MonoBehaviourBase
{
    [Header("Manager")]
    public PlayerController Player;

    private void Awake()
    {
        if(!GameInformation.IsNewGame)
        {
            LoadGame();
        }
    }

    public void SaveGame(Vector2 respawnPoint)
    {
        try
        {
            LogDebug("Saving Game");
            SaveSystem.SavePlayer(Player, respawnPoint);
        }
        catch(Exception ex)
        {
            //Figure out how to recover
            LogError("Error Saving Game: " + ex.Message);
        }
    }

    public void LoadGame()
    {
        try
        {
            LogDebug("Loading Save Game");
            var playerData = SaveSystem.LoadPlayer();
            Player.CanDash = playerData.CanDash;
            Player.CanWallJump = playerData.CanWallJump;
            Player.CanDoubleJump = playerData.CanDoubleJump;
            Player.CanEnemyBounce = playerData.CanEnemyBounce;
            Player.CanFloat = playerData.CanFloat;

            Player.CurrentHealth = playerData.CurrentHealth;
            Player.MaxHealth = playerData.MaxHealth;

            var respawnPos = new Vector2(playerData.RespawnPoint[0], playerData.RespawnPoint[1]);
            Player.transform.position = respawnPos;

            //Shrines Visited
            //Game Progress
            //Map Uncovered
        }
        catch(Exception ex)
        {
            //Figure out how to recover
            LogError("Error Loading Game: " + ex.Message);
        }
    }
}
