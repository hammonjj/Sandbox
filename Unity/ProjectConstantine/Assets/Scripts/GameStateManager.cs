using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviourBase
{
    public GameDesignSettings GameDesignSettings;

    private bool _isInitialized = false;
    private DoorManager _doorManager;
    private SceneStateManager _sceneStateManager;
    private ZoneRouteCoordinator _zoneRouteCoordinator;

    private static GameStateManager _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        /*
        //Convert to send whole GameDesignSettings object
        _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager).GetComponent<SceneStateManager>();

        var currentZone = _sceneStateManager.GetCurrentZone();
        if (currentZone != Constants.Zones.Zone1 &&
            currentZone != Constants.Zones.Zone2 &&
            currentZone != Constants.Zones.Zone3)
        {
            LogDebug($"Not in a zone (current zone: {currentZone}). Skipping next room calculation");
            return;
        }

        LogDebug("Calculating Next Rooms");
        _zoneRouteCoordinator = new ZoneRouteCoordinator(
            GameDesignSettings.ChanceOfShop,
            GameDesignSettings.ChanceOfRest,
            GameDesignSettings.ChanceOfFight,
            GameDesignSettings.ChanceOfStory,
            GameDesignSettings.ChanceOfElite,
            10,
            _sceneStateManager.CurrentSceneType);

        //Calculate the next room(s)
        var roomOptions = _zoneRouteCoordinator.CalculateNextSceneOptions();


        //Convert roomOptions to Scenes
        var sceneOptions = new List<(Constants.Scenes, Constants.RoomReward)>();
        foreach(var roomOption in roomOptions)
        {
            var roomReward = roomOption.RoomReward;
            var sceneName = MapSceneTypeAndZoneToScene(roomOption.SceneType, currentZone);

            sceneOptions.Add((sceneName, roomReward));
        }


        var doorManager = GameObject.FindGameObjectWithTag(Constants.DoorManager).GetComponent<DoorManager>();
        doorManager.AssignOptionsToDoors(sceneOptions);
        //Call Scene Manager and assign each room to a door
        */
    }

    private void Update()
    {
        if(_isInitialized)
        {
            return;
        }

        if(_sceneStateManager == null)
        {
            _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager)?.GetComponent<SceneStateManager>();

            if(_sceneStateManager == null)
            {
                return;
            }

            LogDebug("Acquired SceneStateManager");
        }

        if(_doorManager == null)
        {
            _doorManager = GameObject.FindGameObjectWithTag(Constants.DoorManager)?.GetComponent<DoorManager>();

            if(_doorManager == null)
            {
                return;
            }

            LogDebug("Acquired DoorManager");
        }

        _isInitialized = true;
        var currentZone = _sceneStateManager.GetCurrentZone();
        if(currentZone != Constants.Zones.Zone1 &&
            currentZone != Constants.Zones.Zone2 &&
            currentZone != Constants.Zones.Zone3)
        {
            LogDebug($"Not in a zone (current zone: {currentZone}). Skipping next room calculation");
            return;
        }

        LogDebug("Calculating Next Rooms");
        _zoneRouteCoordinator = new ZoneRouteCoordinator(
            GameDesignSettings.ChanceOfShop,
            GameDesignSettings.ChanceOfRest,
            GameDesignSettings.ChanceOfFight,
            GameDesignSettings.ChanceOfStory,
            GameDesignSettings.ChanceOfElite,
            10,
            _sceneStateManager.CurrentSceneType);

        //Calculate the next room(s)
        var roomOptions = _zoneRouteCoordinator.CalculateNextSceneOptions();

        //Convert roomOptions to Scenes
        var sceneOptions = new List<(Constants.Scenes, Constants.RoomReward)>();
        foreach(var roomOption in roomOptions)
        {
            var roomReward = roomOption.RoomReward;
            var sceneName = MapSceneTypeAndZoneToScene(roomOption.SceneType, currentZone);

            sceneOptions.Add((sceneName, roomReward));
        }

        _doorManager.AssignOptionsToDoors(sceneOptions);
    }

    private Constants.Scenes MapSceneTypeAndZoneToScene(Constants.SceneType sceneType, Constants.Zones zone)
    {
        Constants.Scenes ret = Constants.Scenes.None;

        if(sceneType == Constants.SceneType.Shop)
        {
            switch(zone)
            {
                case Constants.Zones.Zone1:
                    ret = Constants.Scenes.Zone1_Shop;
                    break;
                case Constants.Zones.Zone2:
                    break;
                case Constants.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.SceneType.Rest)
        {
            switch(zone)
            {
                case Constants.Zones.Zone1:
                    ret = Constants.Scenes.Zone1_Rest;
                    break;
                case Constants.Zones.Zone2:
                    break;
                case Constants.Zones.Zone3:
                    break;
            }
        }
        else if (sceneType == Constants.SceneType.Story)
        {
            switch(zone)
            {
                case Constants.Zones.Zone1:
                    ret = Constants.Scenes.Zone1_Story;
                    break;
                case Constants.Zones.Zone2:
                    break;
                case Constants.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.SceneType.OneExit)
        {
            switch(zone)
            {
                case Constants.Zones.Zone1:
                    ret = Constants.Scenes.Zone1_OneExit;
                    break;
                case Constants.Zones.Zone2:
                    break;
                case Constants.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.SceneType.TwoExits)
        {
            switch(zone)
            {
                case Constants.Zones.Zone1:
                    ret = Constants.Scenes.Zone1_TwoExits;
                    break;
                case Constants.Zones.Zone2:
                    break;
                case Constants.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.SceneType.ThreeExits)
        {
            switch(zone)
            {
                case Constants.Zones.Zone1:
                    ret = Constants.Scenes.Zone1_ThreeExits;
                    break;
                case Constants.Zones.Zone2:
                    break;
                case Constants.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.SceneType.Boss)
        {
            switch(zone)
            {
                case Constants.Zones.Zone1:
                    ret = Constants.Scenes.Zone1_Boss;
                    break;
                case Constants.Zones.Zone2:
                    break;
                case Constants.Zones.Zone3:
                    break;
            }
        }

        return ret;
    }
}
