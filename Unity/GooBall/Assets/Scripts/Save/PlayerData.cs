using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool CanDash;
    public bool CanWallJump;
    public bool CanDoubleJump;
    public bool CanEnemyBounce;
    public bool CanFloat;
    public int CurrentHealth;
    public int MaxHealth;
    public float[] RespawnPoint;

    public PlayerData(PlayerController player, Vector2 respawnPoint)
    {
        CanDash = player.CanDash;
        CanWallJump = player.CanWallJump;
        CanDoubleJump = player.CanDoubleJump;
        CanEnemyBounce = player.CanEnemyBounce;
        CanFloat = player.CanFloat;

        CurrentHealth = player.CurrentHealth;
        MaxHealth = player.MaxHealth;

        RespawnPoint = new float[2];
        RespawnPoint[0] = respawnPoint.x;
        RespawnPoint[1] = respawnPoint.y;

        //Shrines Visited
        //Game Progress
        //Map Uncovered
    }
}
