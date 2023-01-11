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
    private Constants.Zones _currentZone;
    private List<Constants.SceneType> _availableSceneTypes;
    private List<Constants.SceneType> _previousSceneTypes = new();

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

        _availableSceneTypes = new List<Constants.SceneType>();
        //_availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Boss, chanceOfElite));
        _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.OneExit, ChanceOfFight));

        if(!_previousSceneTypes.Contains(Constants.SceneType.Rest))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Rest, ChanceOfRest));
        }

        if(!_previousSceneTypes.Contains(Constants.SceneType.Story))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Story, ChanceOfStory));
        }

        if(!_previousSceneTypes.Contains(Constants.SceneType.Shop))
        {
            _availableSceneTypes.AddRange(CreateSceneTypeVotes(Constants.SceneType.Shop, ChanceOfShop));
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

    public List<NextRoom> CalculateNextRoomOptions(Constants.SceneType currentSceneType, Constants.Zones currentZone)
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
            case Constants.SceneType.OneExit: //Standard Fight
                ret = CalculateFight();
                break;
            case Constants.SceneType.Boss: //Elite Fight
                ret = CalculateEliteFight();
                break;
            case Constants.SceneType.Shop:
                var currentSceneExits = GetCurrentSceneExits();
                for(int i = 0; i < currentSceneExits; i++)
                {
                    ret.Add(new NextRoom()
                    {
                        SceneType = Constants.SceneType.Shop,
                        RoomReward = GetRandomRoomReward(Constants.SceneType.Shop)
                    });
                }
                
                break;
            case Constants.SceneType.Rest:
                currentSceneExits = GetCurrentSceneExits();
                for(int i = 0; i < currentSceneExits; i++)
                {
                    ret.Add(new NextRoom()
                    {
                        SceneType = Constants.SceneType.Rest,
                        RoomReward = GetRandomRoomReward(Constants.SceneType.Rest)
                    });
                }
                break;
            case Constants.SceneType.Story:
                currentSceneExits = GetCurrentSceneExits();
                for(int i = 0; i < currentSceneExits; i++)
                {
                    ret.Add(new NextRoom()
                    {
                        SceneType = Constants.SceneType.Story,
                        RoomReward = GetRandomRoomReward(Constants.SceneType.Story)
                    });
                }
                break;
        }

        return ret;
    }

    private void RemoveInvalidSceneTypes()
    {
        if(_previousSceneTypes.Contains(Constants.SceneType.Rest))
        {
            _availableSceneTypes.RemoveAll(x => x == Constants.SceneType.Rest);
        }

        if(_previousSceneTypes.Contains(Constants.SceneType.Story))
        {
            _availableSceneTypes.RemoveAll(x => x == Constants.SceneType.Story);
        }

        if(!_previousSceneTypes.Contains(Constants.SceneType.Shop))
        {
            _availableSceneTypes.RemoveAll(x => x == Constants.SceneType.Shop);
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
                    SceneType = Constants.SceneType.Shop,
                    RoomReward = GetRandomRoomReward(Constants.SceneType.Shop)
                });
            }
            else if(exits == 2)
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
            else if(exits == 3)
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
                ret.Add(new NextRoom()
                {
                    SceneType = Constants.SceneType.OneExit,
                    RoomReward = GetRandomRoomReward(Constants.SceneType.OneExit)
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
                SceneType = Constants.SceneType.Boss,
                RoomReward = GetBossRoomReward()
            });

            Helper.LogDebug("Boss is next");
        }
        else if(_previousSceneTypes.Count == GetCurrentZoneMaxChambers()) //In Boss Room
        {
            ret.Add(new NextRoom()
            {
                SceneType = Constants.SceneType.None,
                RoomReward = GetRandomRoomReward(Constants.SceneType.Boss)
            });

            Helper.LogDebug("Boss Room");
        }

        return ret;
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
        Constants.RoomReward reward;
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
        var ret = 0;
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

    private int GetCurrentZoneMaxChambers()
    {
        int ret = 0;
        switch(_currentZone)
        {
            case Constants.Zones.None:
                ret = 0;
                break;
            case Constants.Zones.Zone1:
                ret = Zone1Length;
                break;
            case Constants.Zones.Zone2:
                ret = Zone2Length;
                break;
            case Constants.Zones.Zone3:
                ret = Zone3Length;
                break;
        }

        return ret;
    }
}
