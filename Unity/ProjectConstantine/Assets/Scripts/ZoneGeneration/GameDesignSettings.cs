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

    public int Zone1Chambers;
    public int Zone2Chambers;
    public int Zone3Chambers;
}
