using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneRouteCoordinator
{
    private bool _hasHadShop;
    private bool _hasHadRest;
    private bool _hasHadStory;

    //Room Type
    private readonly int ChanceOfShop;
    private readonly int ChanceOfRest;
    private readonly int ChanceOfFight;
    private readonly int ChanceOfStory;
    private readonly int ChanceOfElite;

    private readonly int ZoneLength;

    //Room Rewards
    private readonly int ChanceOfBuff;
    private readonly int ChanceOfCurrency;

    //Includes the current scene type
    private List<Constants.SceneType> _previousSceneTypes;

    public ZoneRouteCoordinator(
        int chanceOfShop, 
        int chanceOfRest, 
        int chanceOfFight, 
        int chanceOfStory, 
        int chanceOfElite,
        int zoneLength,
        Constants.SceneType currentSceneType)
    {
        ChanceOfShop = chanceOfShop;
        ChanceOfRest = chanceOfRest;
        ChanceOfFight = chanceOfFight;
        ChanceOfStory = chanceOfStory;
        ChanceOfElite = chanceOfElite;
        ZoneLength = zoneLength;

        _previousSceneTypes = new List<Constants.SceneType>
        {
            currentSceneType
        };
    }

    public List<Constants.SceneType> CalculateNextSceneOptions()
    {
        //Check if in these rooms:
        //  - First => Next is always a fight
        //  - Third to Last => Always Shop OR Shop || Rest
        //  - Second to Last => Always Boss
        //  - Last => Boss leads to outside

        var ret = new List<Constants.SceneType>();

        /* Turn this in to a lottery instead of calculating odds */

        //First decide on fight or not
        var chanceOfFight = ChanceOfFight + ChanceOfElite;

        var randNum = Random.Range(0, 100);

        var isFight = randNum < chanceOfFight;
        var numExits = 0;

        if(isFight)
        {
            //Pick Number of Exits
            numExits = Random.Range(1, 3);
            for(int i = 0; i < numExits; i++)
            {

            }
        }

        if(!isFight)
        {
            //Remove options that can't be possible
        }


        return ret;
    }
}
