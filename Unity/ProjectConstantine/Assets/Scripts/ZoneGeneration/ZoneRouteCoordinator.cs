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

    private readonly int ChanceOfShop;
    private readonly int ChanceOfRest;
    private readonly int ChanceOfFight;
    private readonly int ChanceOfStory;
    private readonly int ChanceOfElite;

    //Room Rewards
    private readonly int ChanceOfBuff;
    private readonly int ChanceOfCosmetic;
    private readonly int ChanceOfCurrency;

    //Includes the current scene type
    private Constants.Enums.Zones _currentZone;
    private List<Constants.Enums.RoomType> _availableSceneTypes = new();
    private List<Constants.Enums.SceneType> _previousSceneTypes = new();
    private List<Constants.Enums.RoomReward> _availableRoomRewards = new();

    public ZoneRouteCoordinator(GameDesignSettings gameDesignSettings)
    {
        Zone1Length = gameDesignSettings.Zone1Chambers;
        Zone2Length = gameDesignSettings.Zone2Chambers;
        Zone3Length = gameDesignSettings.Zone3Chambers;

        ChanceOfBuff = gameDesignSettings.ChanceOfBuff;
        ChanceOfCosmetic = gameDesignSettings.ChanceOfCosmetic;
        ChanceOfCurrency = gameDesignSettings.ChanceOfCurrency;

        ChanceOfShop = gameDesignSettings.ChanceOfShop;
        ChanceOfRest = gameDesignSettings.ChanceOfRest;
        ChanceOfFight = gameDesignSettings.ChanceOfFight;
        ChanceOfStory = gameDesignSettings.ChanceOfStory;
        ChanceOfElite = gameDesignSettings.ChanceOfElite;

        _availableSceneTypes.AddRange(CreateVotes(Constants.Enums.RoomType.Elite, ChanceOfElite));
        _availableSceneTypes.AddRange(CreateVotes(Constants.Enums.RoomType.Normal, ChanceOfFight));
        _availableSceneTypes.AddRange(CreateVotes(Constants.Enums.RoomType.Rest, ChanceOfRest));
        _availableSceneTypes.AddRange(CreateVotes(Constants.Enums.RoomType.Story, ChanceOfStory));
        _availableSceneTypes.AddRange(CreateVotes(Constants.Enums.RoomType.Shop, ChanceOfShop));

        _availableRoomRewards.AddRange(CreateVotes(Constants.Enums.RoomReward.Combat, ChanceOfBuff));
        _availableRoomRewards.AddRange(CreateVotes(Constants.Enums.RoomReward.Cosmetic, ChanceOfCosmetic));
        _availableRoomRewards.AddRange(CreateVotes(Constants.Enums.RoomReward.Currency, ChanceOfCurrency));
    }

    private List<T> CreateVotes<T>(T eventType, int chanceOfEvent)
    {
        var ret = new List<T>();
        for(var i = 0; i < chanceOfEvent; i++)
        {
            ret.Add(eventType);
        }

        return ret;
    }

    private int GetCurrentSceneExits()
    {
        var doorManager = GameObject.FindGameObjectWithTag(Constants.Tags.DoorManager).GetComponent<DoorManager>();
        if(doorManager == null)
        {
            Helper.LogError("Unable to acquire DoorManager");
            return 0;
        }

        return doorManager.ZoneDoors.Count;
    }

    public List<NextRoom> CalculateNextRoomOptions(Constants.Enums.SceneType currentSceneType, Constants.Enums.Zones currentZone)
    {
        _currentZone = currentZone;
        _previousSceneTypes.Add(currentSceneType);

        //Check that we aren't in a static situation
        var ret = CheckAgainstStaticRules();
        if(ret.Count != 0)
        {
            return ret;
        }

        //Check number of doors in the current room
        //Iterate over each door to determine what room it leads to
        //  - Get a reward for each room if it's of type rest, fight, or elite
        //  - Determine how many exits should be in the next room so we know what to load
        var currentSceneExits = GetCurrentSceneExits();
        for(int i = 0; i < currentSceneExits; i++)
        {
            var nextRoomType = _availableSceneTypes[Helper.RandomInclusiveRange(0, _availableSceneTypes.Count - 1)];
            switch(nextRoomType)
            {
                case Constants.Enums.RoomType.Normal:
                    ret.Add(
                        new NextRoom(
                            Constants.Enums.SceneType.Fight, 
                            GetRoomReward(Constants.Enums.SceneType.Fight)));

                    break;
                case Constants.Enums.RoomType.Elite:
                    ret.Add(
                        new NextRoom(
                            Constants.Enums.SceneType.Elite,
                            GetRoomReward(Constants.Enums.SceneType.Fight)));

                    break;
                case Constants.Enums.RoomType.Shop:
                    ret.Add(
                        new NextRoom(
                            Constants.Enums.SceneType.Shop,
                            GetRoomReward(Constants.Enums.SceneType.Shop)));

                    _availableSceneTypes.RemoveAll(x => x == Constants.Enums.RoomType.Shop);
                    break;
                case Constants.Enums.RoomType.Rest:
                    ret.Add(
                        new NextRoom(
                            Constants.Enums.SceneType.Rest,
                            GetRoomReward(Constants.Enums.SceneType.Rest)));

                    _availableSceneTypes.RemoveAll(x => x == Constants.Enums.RoomType.Rest);
                    break;
                case Constants.Enums.RoomType.Story:
                    ret.Add(
                        new NextRoom(
                            Constants.Enums.SceneType.Story,
                            GetRoomReward(Constants.Enums.SceneType.Story)));

                    _availableSceneTypes.RemoveAll(x => x == Constants.Enums.RoomType.Story);
                    break;
            }
        }

        return ret;
    }

    private List<NextRoom> CheckAgainstStaticRules()
    {
        var ret = new List<NextRoom>();

        //We're currently in the first room
        if(_previousSceneTypes.Count == 1)
        {
            ret.Add(
                new NextRoom(
                    Constants.Enums.SceneType.Fight,
                    GetRoomReward(Constants.Enums.SceneType.Fight)));

            Helper.LogDebug("First room of the zone");
        }
        else if(_previousSceneTypes.Count == GetCurrentZoneMaxChambers() - 2) //Close to boss -> Rest or Shop
        {
            //Need to check current room to know how many exits to prepare
            var exits = GetCurrentSceneExits();
            if(exits == 1)
            {
                ret.Add(
                    new NextRoom(
                        Constants.Enums.SceneType.Shop,
                        GetRoomReward(Constants.Enums.SceneType.Shop)));
            }
            else if(exits == 2)
            {
                ret.Add(
                    new NextRoom(
                        Constants.Enums.SceneType.Shop,
                        GetRoomReward(Constants.Enums.SceneType.Shop)));

                ret.Add(
                    new NextRoom(
                        Constants.Enums.SceneType.Rest,
                        GetRoomReward(Constants.Enums.SceneType.Rest)));
            }
            else if(exits == 3)
            {
                ret.Add(
                    new NextRoom(
                        Constants.Enums.SceneType.Shop,
                        GetRoomReward(Constants.Enums.SceneType.Shop)));

                ret.Add(
                    new NextRoom(
                        Constants.Enums.SceneType.Rest,
                        GetRoomReward(Constants.Enums.SceneType.Rest)));

                ret.Add(
                    new NextRoom(
                        Constants.Enums.SceneType.Fight,
                        GetRoomReward(Constants.Enums.SceneType.Fight)));
            }
            else
            {
                Helper.LogDebug($"Invalid number of exits: {exits}");
            }

            Helper.LogDebug("Two doors from the boss");
        }
        else if(_previousSceneTypes.Count == GetCurrentZoneMaxChambers() - 1) //Boss Room Next
        {
            ret.Add(
                new NextRoom(
                    Constants.Enums.SceneType.Boss,
                    GetRoomReward(Constants.Enums.SceneType.Boss)));

            Helper.LogDebug("Boss is next");
        }
        else if(_previousSceneTypes.Count == GetCurrentZoneMaxChambers()) //In Boss Room
        {
            ret.Add(
                new NextRoom(
                    Constants.Enums.SceneType.WorldHub,
                    GetRoomReward(Constants.Enums.SceneType.None)));

            Helper.LogDebug("Boss Room");
        }

        return ret;
    }

    private Constants.Enums.RoomReward GetRoomReward(Constants.Enums.SceneType sceneType)
    {
        Constants.Enums.RoomReward reward;
        switch (sceneType)
        {
            case Constants.Enums.SceneType.Fight:
                //Choose between combat, currency or cosmetic
                reward = _availableRoomRewards[Helper.RandomInclusiveRange(0, _availableRoomRewards.Count - 1)];
                Helper.LogDebug($"Room Reward: {reward}");
                break;
            case Constants.Enums.SceneType.Elite:
                //Elite fights are hard, so should return combat
                reward = Constants.Enums.RoomReward.Combat;
                break;
            case Constants.Enums.SceneType.Boss:
                //Need to figure out what mega reward to give for bosses
                reward = Constants.Enums.RoomReward.Combat;
                break;
            case Constants.Enums.SceneType.Rest:
            case Constants.Enums.SceneType.Shop:
            case Constants.Enums.SceneType.Story:
            case Constants.Enums.SceneType.None:
                reward = Constants.Enums.RoomReward.None;
                break;
            default:
                Helper.LogError($"Unknown SceneType: {sceneType}");
                reward = Constants.Enums.RoomReward.None;
                break;
        }

        return reward;
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
