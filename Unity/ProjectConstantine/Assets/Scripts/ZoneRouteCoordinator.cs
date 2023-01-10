﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneRouteCoordinator
{
    //Room Type -> Convert all of these to a single scriptable data object
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
        //_availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Boss, chanceOfElite));
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

    public List<NextRoom> CalculateNextRoomOptions()
    {
        var ret = new List<NextRoom>();

        return ret;
    }

    public List<NextRoom> CalculateNextSceneOptions()
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
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.SceneType.Shop,
                    RoomReward = GetRandomRoomReward(Constants.SceneType.Shop)
                });
                break;
            case Constants.SceneType.Rest:
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.SceneType.Rest,
                    RoomReward = GetRandomRoomReward(Constants.SceneType.Rest)
                });
                break;
            case Constants.SceneType.Story:
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.SceneType.Story,
                    RoomReward = GetRandomRoomReward(Constants.SceneType.Story)
                });
                break;
        }

        return ret;
    }

    private List<NextRoom> CalculateFight()
    {
        //Get Number of exits for current scene
        //Create "NextZone" for each exit
        //  - Iterate through each "NextZone" and assign a reward
        var ret = new List<NextRoom>();

        var currentSceneExits = GetCurrentSceneExits();
        for(int i = 0; i < currentSceneExits; i++)
        {
            var nextSceneType = GetRandomNumberOfRoomExits();
            ret.Add(new NextRoom()
            {
                SceneType = nextSceneType,
                RoomReward = GetRandomRoomReward(nextSceneType)
            });
        }

        return ret;
    }

    private List<NextRoom> CalculateEliteFight()
    {
        //Get Number of exits for current scene
        //Create "NextZone" for each exit
        //  - Iterate through each "NextZone" and assign a reward
        var ret = new List<NextRoom>();

        return ret;
    }

    private List<NextRoom> CheckAgainstStaticRules()
    {
        var ret = new List<NextRoom>();

        //We're currently in the first room
        if(_previousSceneTypes.Count == 1)
        {
            var exits = GetRandomNumberOfRoomExits();
            ret.Add(new NextRoom()
            {
                SceneType = exits,
                RoomReward = GetRandomRoomReward(exits)
            });
        }
        else if (_previousSceneTypes.Count == ZoneLength - 2) //Close to boss -> Rest or Shop
        {
            ret.Add(new NextRoom()
            {
                SceneType = Constants.SceneType.Shop,
                RoomReward = GetRandomRoomReward(Constants.SceneType.Shop)
            });
            ret.Add(new NextRoom()
            {
                SceneType = Constants.SceneType.Rest,
                RoomReward = GetRandomRoomReward(Constants.SceneType.Rest)
            });
        }
        else if(_previousSceneTypes.Count == ZoneLength -1) //Boss Room Next
        {
            ret.Add(new NextRoom()
            {
                SceneType = Constants.SceneType.Boss,
                RoomReward = GetBossRoomReward()
            });
        }
        else if(_previousSceneTypes.Count == ZoneLength) //In Boss Room
        {
            ret.Add(new NextRoom()
            {
                SceneType = Constants.SceneType.None,
                RoomReward = GetRandomRoomReward(Constants.SceneType.Boss)
            });
        }

        return ret;
    }

    private int GetRandomNumberOfExits()
    {
        return Random.Range(1, 4);
    }

    private Constants.SceneType GetRandomNumberOfRoomExits()
    {
        Constants.SceneType ret;
        var rand = Random.Range(1, 4);
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
                throw new System.Exception($"GetRandomtNumberOfRoomExits returned invalid range: {rand}");
        }

        return ret;
    }

    private Constants.RoomReward GetRandomRoomReward(Constants.SceneType sceneType)
    {
        var reward = Constants.RoomReward.None;

        if(sceneType == Constants.SceneType.Shop)
        {
            reward = Constants.RoomReward.Shop;
        }
        else if(sceneType == Constants.SceneType.Story)
        {
            reward = Constants.RoomReward.Story;
        }
        else
        {
            reward = Constants.RoomReward.Combat;
        }

        return reward;
    }

    private List<Constants.RoomReward> GetRandomRoomRewards(Constants.SceneType sceneType)
    {
        var rewardsCount = 0;
        switch (sceneType)
        {
            case Constants.SceneType.Shop:
            case Constants.SceneType.Rest:
            case Constants.SceneType.Story:
            case Constants.SceneType.OneExit:
                rewardsCount = 1;
                break;
            case Constants.SceneType.TwoExits:
                rewardsCount = 2;
                break;
            case Constants.SceneType.ThreeExits:
                rewardsCount = 3;
                break;
            default:
                rewardsCount = 0;
                break;
        }

        var ret = new List<Constants.RoomReward>();
        if(rewardsCount == 0)
        {
            ret.Add(Constants.RoomReward.None);
        }

        for(int i = 0; i < rewardsCount; i++)
        {
            ret.Add(Constants.RoomReward.Combat);
        }

        return ret;
    }

    private Constants.RoomReward GetBossRoomReward()
    {
        var ret = Constants.RoomReward.Combat;

        return ret;
    }

    private int GetCurrentSceneExits()
    {
        var ret = 1;

        var currentSceneType = _previousSceneTypes.Last();
        switch(currentSceneType)
        {
            case Constants.SceneType.Shop:
            case Constants.SceneType.Rest:
            case Constants.SceneType.Story:
            case Constants.SceneType.OneExit:
                ret = 1;
                break;
            case Constants.SceneType.TwoExits:
                ret = 2;
                break;
            case Constants.SceneType.ThreeExits:
                ret = 3;
                break;
        }

        return ret;
    }
}
