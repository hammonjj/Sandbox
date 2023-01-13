using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneRouteCoordinator
{
    public int ZoneLength 
    { 
        get 
        { 
            return GetCurrentZoneMaxChambers();  //Includes Boss Fight
        } 
    }

    public int CurrentChamber
    {
        get { return _previousSceneTypes.Count(); }
    }

    private readonly int Zone1Length;
    private readonly int Zone2Length;
    private readonly int Zone3Length;

    //Room Type -> Convert all of these to a single scriptable data object
    private readonly int ChanceOfShop;
    private readonly int ChanceOfRest;
    private readonly int ChanceOfFight;
    private readonly int ChanceOfStory;
    private readonly int ChanceOfElite;

    //Room Rewards
    private readonly int ChanceOfBuff = 34;
    private readonly int ChanceOfCosmetic = 33;
    private readonly int ChanceOfCurrency = 33;

    //Includes the current scene type
    private Constants.Enums.Zones _currentZone;
    private List<Constants.Enums.SceneType> _availableSceneTypes;
    private List<Constants.Enums.SceneType> _previousSceneTypes = new();

    public ZoneRouteCoordinator(GameDesignSettings gameDesignSettings)
    {
        Zone1Length = gameDesignSettings.Zone1Chambers;
        Zone2Length = gameDesignSettings.Zone2Chambers;
        Zone3Length = gameDesignSettings.Zone3Chambers;

        ChanceOfShop = gameDesignSettings.ChanceOfShop;
        ChanceOfRest = gameDesignSettings.ChanceOfRest;
        ChanceOfFight = gameDesignSettings.ChanceOfFight;
        ChanceOfStory = gameDesignSettings.ChanceOfStory;
        ChanceOfElite = gameDesignSettings.ChanceOfElite;

        _availableSceneTypes = new List<Constants.Enums.SceneType>();
        //_availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Boss, chanceOfElite));
        _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.Enums.SceneType.OneExit, ChanceOfFight));

        if(!_previousSceneTypes.Contains(Constants.Enums.SceneType.Rest))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.Enums.SceneType.Rest, ChanceOfRest));
        }

        if(!_previousSceneTypes.Contains(Constants.Enums.SceneType.Story))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.Enums.SceneType.Story, ChanceOfStory));
        }

        if(!_previousSceneTypes.Contains(Constants.Enums.SceneType.Shop))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.Enums.SceneType.Shop, ChanceOfShop));
        }
    }

    private List<Constants.Enums.SceneType> CreateSceneTypeVotes(Constants.Enums.SceneType sceneType, int chanceOfEvent)
    {
        var ret = new List<Constants.Enums.SceneType>();
        for(var i = 0; i < chanceOfEvent; i++)
        {
            ret.Add(sceneType);
        }

        return ret;
    }

    public List<NextRoom> CalculateNextRoomOptions(Constants.Enums.SceneType currentSceneType, Constants.Enums.Zones currentZone)
    {
        _currentZone = currentZone;
        _previousSceneTypes.Add(currentSceneType);
        RemoveInvalidSceneTypes();

        //Check that we aren't in a static situation
        var ret = CheckAgainstStaticRules();
        if(ret.Count != 0)
        {
            return ret;
        }

        //Calculate next scenes
        var sceneType = _availableSceneTypes[Random.Range(0, _availableSceneTypes.Count - 1)];
        Helper.LogDebug($"Calculating Next Room Options - {sceneType}");
        switch(sceneType)
        {
            case Constants.Enums.SceneType.OneExit: //Standard Fight
                ret = CalculateFight();
                break;
            case Constants.Enums.SceneType.Boss: //Elite Fight
                ret = CalculateEliteFight();
                break;
            case Constants.Enums.SceneType.Shop:
                var currentSceneExits = GetCurrentSceneExits();
                for(int i = 0; i < currentSceneExits; i++)
                {
                    ret.Add(new NextRoom()
                    {
                        SceneType = Constants.Enums.SceneType.Shop,
                        RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Shop)
                    });
                }
                
                break;
            case Constants.Enums.SceneType.Rest:
                currentSceneExits = GetCurrentSceneExits();
                for(int i = 0; i < currentSceneExits; i++)
                {
                    ret.Add(new NextRoom()
                    {
                        SceneType = Constants.Enums.SceneType.Rest,
                        RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Rest)
                    });
                }
                break;
            case Constants.Enums.SceneType.Story:
                currentSceneExits = GetCurrentSceneExits();
                for(int i = 0; i < currentSceneExits; i++)
                {
                    ret.Add(new NextRoom()
                    {
                        SceneType = Constants.Enums.SceneType.Story,
                        RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Story)
                    });
                }
                break;
        }

        return ret;
    }

    private void RemoveInvalidSceneTypes()
    {
        if(_previousSceneTypes.Contains(Constants.Enums.SceneType.Rest))
        {
            _availableSceneTypes.RemoveAll(x => x == Constants.Enums.SceneType.Rest);
        }

        if(_previousSceneTypes.Contains(Constants.Enums.SceneType.Story))
        {
            _availableSceneTypes.RemoveAll(x => x == Constants.Enums.SceneType.Story);
        }

        if(!_previousSceneTypes.Contains(Constants.Enums.SceneType.Shop))
        {
            _availableSceneTypes.RemoveAll(x => x == Constants.Enums.SceneType.Shop);
        }
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

            Helper.LogDebug("First room of the zone");
        }
        else if (_previousSceneTypes.Count == GetCurrentZoneMaxChambers() - 2) //Close to boss -> Rest or Shop
        {
            //Need to check current room to know how many exits to prepare
            var exits = GetCurrentSceneExits();
            if(exits == 1)
            {
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.Enums.SceneType.Shop,
                    RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Shop)
                });
            }
            else if(exits == 2)
            {
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.Enums.SceneType.Shop,
                    RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Shop)
                });
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.Enums.SceneType.Rest,
                    RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Rest)
                });
            }
            else if(exits == 3)
            {
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.Enums.SceneType.Shop,
                    RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Shop)
                });
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.Enums.SceneType.Rest,
                    RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Rest)
                });
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.Enums.SceneType.OneExit,
                    RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.OneExit)
                });
            }
            else
            {
                Helper.LogDebug($"Invalid number of exits: {exits}");
            }

            Helper.LogDebug("Two doors from the boss");
        }
        else if(_previousSceneTypes.Count == GetCurrentZoneMaxChambers() - 1) //Boss Room Next
        {
            ret.Add(new NextRoom()
            {
                SceneType = Constants.Enums.SceneType.Boss,
                RoomReward = GetBossRoomReward()
            });

            Helper.LogDebug("Boss is next");
        }
        else if(_previousSceneTypes.Count == GetCurrentZoneMaxChambers()) //In Boss Room
        {
            ret.Add(new NextRoom()
            {
                SceneType = Constants.Enums.SceneType.None,
                RoomReward = GetRandomRoomReward(Constants.Enums.SceneType.Boss)
            });

            Helper.LogDebug("Boss Room");
        }

        return ret;
    }

    private Constants.Enums.SceneType GetRandomNumberOfRoomExits()
    {
        Constants.Enums.SceneType ret;
        var rand = Random.Range(1, 4);
        switch(rand)
        {
            case 1:
                ret = Constants.Enums.SceneType.OneExit;
                break;
            case 2:
                ret = Constants.Enums.SceneType.TwoExits;
                break;
            case 3:
                ret = Constants.Enums.SceneType.ThreeExits;
                break;
            default:
                throw new System.Exception($"GetRandomtNumberOfRoomExits returned invalid range: {rand}");
        }

        return ret;
    }

    private Constants.Enums.RoomReward GetRandomRoomReward(Constants.Enums.SceneType sceneType)
    {
        Constants.Enums.RoomReward reward;
        if(sceneType == Constants.Enums.SceneType.Shop)
        {
            reward = Constants.Enums.RoomReward.Shop;
        }
        else if(sceneType == Constants.Enums.SceneType.Story)
        {
            reward = Constants.Enums.RoomReward.Story;
        }
        else
        {
            reward = Constants.Enums.RoomReward.Combat;
        }

        return reward;
    }

    private List<Constants.Enums.RoomReward> GetRandomRoomRewards(Constants.Enums.SceneType sceneType)
    {
        var rewardsCount = 0;
        switch (sceneType)
        {
            case Constants.Enums.SceneType.Shop:
            case Constants.Enums.SceneType.Rest:
            case Constants.Enums.SceneType.Story:
            case Constants.Enums.SceneType.OneExit:
                rewardsCount = 1;
                break;
            case Constants.Enums.SceneType.TwoExits:
                rewardsCount = 2;
                break;
            case Constants.Enums.SceneType.ThreeExits:
                rewardsCount = 3;
                break;
            default:
                rewardsCount = 0;
                break;
        }

        var ret = new List<Constants.Enums.RoomReward>();
        if(rewardsCount == 0)
        {
            ret.Add(Constants.Enums.RoomReward.None);
        }

        for(int i = 0; i < rewardsCount; i++)
        {
            ret.Add(Constants.Enums.RoomReward.Combat);
        }

        return ret;
    }

    private Constants.Enums.RoomReward GetBossRoomReward()
    {
        var ret = Constants.Enums.RoomReward.Combat;

        return ret;
    }

    private int GetCurrentSceneExits()
    {
        var ret = 0;
        var currentSceneType = _previousSceneTypes.Last();
        switch(currentSceneType)
        {
            case Constants.Enums.SceneType.Shop:
            case Constants.Enums.SceneType.Rest:
            case Constants.Enums.SceneType.Story:
            case Constants.Enums.SceneType.OneExit:
                ret = 1;
                break;
            case Constants.Enums.SceneType.TwoExits:
                ret = 2;
                break;
            case Constants.Enums.SceneType.ThreeExits:
                ret = 3;
                break;
        }

        return ret;
    }

    private int GetCurrentZoneMaxChambers()
    {
        int ret = 0;
        switch(_currentZone)
        {
            case Constants.Enums.Zones.None:
                ret = 0;
                break;
            case Constants.Enums.Zones.Zone1:
                ret = Zone1Length;
                break;
            case Constants.Enums.Zones.Zone2:
                ret = Zone2Length;
                break;
            case Constants.Enums.Zones.Zone3:
                ret = Zone3Length;
                break;
        }

        return ret;
    }
}
