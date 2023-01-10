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

    private readonly int ZoneLength; //Includes Boss Fight

    //Room Rewards
    private readonly int ChanceOfBuff = 34;
    private readonly int ChanceOfCosmetic = 33;
    private readonly int ChanceOfCurrency = 33;

    //Includes the current scene type
    private List<Constants.SceneType> _previousSceneTypes;
    private List<Constants.SceneType> _availableSceneTypes;

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

        _availableSceneTypes = new List<Constants.SceneType>();
        _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Boss, chanceOfElite));
        _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.OneExit, chanceOfFight));
        
        if(!_previousSceneTypes.Contains(Constants.SceneType.Rest))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Rest, chanceOfRest));
        }

        if(!_previousSceneTypes.Contains(Constants.SceneType.Story))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Rest, chanceOfStory));
        }

        if(!_previousSceneTypes.Contains(Constants.SceneType.Shop))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Rest, chanceOfShop));
        }
    }

    private List<Constants.SceneType> CreateSceneTypeVotes(Constants.SceneType sceneType, int chanceOfEvent)
    {
        var ret = new List<Constants.SceneType>();
        for(var i = 0; i < chanceOfEvent; i++)
        {
            ret.Add(sceneType);
        }

        return ret;
    }

    public List<NextZone> CalculateNextSceneOptions()
    {
        //Check that we aren't in a static situation
        var ret = CheckAgainstStaticRules();
        if(ret.Count != 0)
        {
            return ret;
        }

        //Calculate next scenes
        var sceneType = _availableSceneTypes[Random.Range(0, _availableSceneTypes.Count - 1)];
        switch(sceneType)
        {
            case Constants.SceneType.OneExit: //Standard Fight
                ret = CalculateFight();
                break;
            case Constants.SceneType.Boss: //Elite Fight
                ret = CalculateEliteFight();
                break;
            case Constants.SceneType.Shop:
                ret.Add(new NextZone()
                {
                    SceneType = Constants.SceneType.Shop,
                    RoomReward = Constants.RoomReward.None
                });
                break;
            case Constants.SceneType.Rest:
                ret.Add(new NextZone()
                {
                    SceneType = Constants.SceneType.Rest,
                    RoomReward = Constants.RoomReward.Combat
                });
                break;
            case Constants.SceneType.Story:
                ret.Add(new NextZone()
                {
                    SceneType = Constants.SceneType.Story,
                    RoomReward = Constants.RoomReward.None
                });
                break;
        }

        return ret;
    }

    private List<NextZone> CalculateFight()
    {
        //Need to get number of exits and reward
        var ret = new List<NextZone>();

        return ret;
    }

    private List<NextZone> CalculateEliteFight()
    {
        //Need to get number of exits and reward
        var ret = new List<NextZone>();

        return ret;
    }

    private List<NextZone> CheckAgainstStaticRules()
    {
        //Check if in these rooms:
        //  - First => Next is always a fight
        //  - Third to Last => Always Shop OR Shop || Rest
        var ret = new List<NextZone>();

        //We're currently in the first room
        if(_previousSceneTypes.Count == 1)
        {
            //Get Number of exits for next scene
            //Create "NextZone" for each exit
            //  - Iterate through each "NextZone" and assign a reward
            ret.Add(new NextZone()
            {
                SceneType = GeRandomNumberOfRoomExits(),
                RoomReward = GetRandomRoomReward()
            });
        }
        else if(_previousSceneTypes.Count == ZoneLength -1) //Boss Room Next
        {
            ret.Add(new NextZone()
            {
                SceneType = Constants.SceneType.Boss,
                RoomReward = GetBossRoomReward()
            });
        }
        else if(_previousSceneTypes.Count == ZoneLength) //In Boss Room
        {
            ret.Add(new NextZone()
            {
                SceneType = Constants.SceneType.None,
                RoomReward = Constants.RoomReward.None
            });
        }

        return ret;
    }

    private Constants.SceneType GeRandomNumberOfRoomExits()
    {
        Constants.SceneType ret;
        var rand = Random.Range(1, 3);
        switch(rand)
        {
            case 1:
                ret = Constants.SceneType.OneExit;
                break;
            case 2:
                ret = Constants.SceneType.TwoExits;
                break;
            case 3:
                ret = Constants.SceneType.ThreeExits;
                break;
            default:
                throw new System.Exception($"GeRandomtNumberOfRoomExits returned invalid range: {rand}");
        }

        return ret;
    }

    private Constants.RoomReward GetRandomRoomReward()
    {
        return Constants.RoomReward.Combat;
    }

    private Constants.RoomReward GetBossRoomReward()
    {
        return Constants.RoomReward.Combat;
    }
}
