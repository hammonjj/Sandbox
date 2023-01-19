using UnityEngine;

[CreateAssetMenu(fileName = "GameDesignData", menuName = "GameDesignData")]
public class GameDesignSettings : ScriptableObject
{
    [Header("Zone Design")]
    public int ChanceOfShop;
    public int ChanceOfRest;
    public int ChanceOfFight;
    public int ChanceOfStory;
    public int ChanceOfElite;

    [Header("Room Rewards")]
    public int ChanceOfBuff;
    public int ChanceOfCosmetic;
    public int ChanceOfCurrency;

    [Header("Zone Length")]
    public int Zone1Chambers;
    public int Zone2Chambers;
    public int Zone3Chambers;
}
