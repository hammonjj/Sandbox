using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviourBase
{
    public Constants.Enums.RoomReward NextRoomReward;
    public GameDesignSettings GameDesignSettings;

    //For Debug UI
    public int CurrentChamber
    {
        get { return _zoneRouteCoordinator == null ? -1 : _zoneRouteCoordinator.CurrentChamber; }
    }

    public int ZoneMaximumChambers
    {
        get { return _zoneRouteCoordinator == null ? -1 : _zoneRouteCoordinator.ZoneLength; }
    }

    private bool _recalculateRoomOptions;
    private bool _isInitialized = false;
    private DoorManager _doorManager;
    private SceneStateManager _sceneStateManager;
    private ZoneRouteCoordinator _zoneRouteCoordinator;

    private static GameStateManager _instance;

    private void Awake()
    {
        _recalculateRoomOptions = true;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if(!_isInitialized)
        {
            if(_sceneStateManager == null)
            {
                _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.Tags.SceneStateManager)?.GetComponent<SceneStateManager>();

                if(_sceneStateManager == null)
                {
                    return;
                }

                LogDebug("Acquired SceneStateManager");
            }

            if(_doorManager == null)
            {
                _doorManager = GameObject.FindGameObjectWithTag(Constants.Tags.DoorManager)?.GetComponent<DoorManager>();

                if(_doorManager == null)
                {
                    return;
                }

                LogDebug("Acquired DoorManager");
            }

            _isInitialized = true;
        }

        if(!_recalculateRoomOptions)
        {
            return;
        }

        _recalculateRoomOptions = false;
        var currentZone = _sceneStateManager.GetCurrentZone();
        if(currentZone != Constants.Enums.Zones.Zone1 &&
            currentZone != Constants.Enums.Zones.Zone2 &&
            currentZone != Constants.Enums.Zones.Zone3)
        {
            LogDebug($"Not in a zone (current zone: {currentZone}). Skipping next room calculation");
            return;
        }

        LogDebug("Calculating Next Rooms");
        if(_zoneRouteCoordinator == null)
        {
            _zoneRouteCoordinator = new ZoneRouteCoordinator(GameDesignSettings);
        }

        //Calculate the next room(s)
        var roomOptions = _zoneRouteCoordinator.CalculateNextRoomOptions(
            _sceneStateManager.CurrentSceneType, _sceneStateManager.GetCurrentZone());

        //Convert roomOptions to Scenes
        var sceneOptions = new List<(Constants.Enums.Scenes, Constants.Enums.RoomReward)>();
        foreach(var roomOption in roomOptions)
        {
            var roomReward = roomOption.RoomReward;
            var sceneName = MapSceneTypeAndZoneToScene(roomOption.SceneType, currentZone);

            sceneOptions.Add((sceneName, roomReward));
        }

        _doorManager.AssignOptionsToDoors(sceneOptions);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LogDebug("OnSceneLoaded Called");

        _doorManager = null;
        _sceneStateManager = null;
        _isInitialized = false;
        _recalculateRoomOptions = true;
    }

    private Constants.Enums.Scenes MapSceneTypeAndZoneToScene(Constants.Enums.SceneType sceneType, Constants.Enums.Zones zone)
    {
        var ret = Constants.Enums.Scenes.None;
        if(sceneType == Constants.Enums.SceneType.Shop)
        {
            switch(zone)
            {
                case Constants.Enums.Zones.Zone1:
                    ret = Constants.Enums.Scenes.Zone1_Shop;
                    break;
                case Constants.Enums.Zones.Zone2:
                    break;
                case Constants.Enums.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.Enums.SceneType.Rest)
        {
            switch(zone)
            {
                case Constants.Enums.Zones.Zone1:
                    ret = Constants.Enums.Scenes.Zone1_Rest;
                    break;
                case Constants.Enums.Zones.Zone2:
                    break;
                case Constants.Enums.Zones.Zone3:
                    break;
            }
        }
        else if (sceneType == Constants.Enums.SceneType.Story)
        {
            switch(zone)
            {
                case Constants.Enums.Zones.Zone1:
                    ret = Constants.Enums.Scenes.Zone1_Story;
                    break;
                case Constants.Enums.Zones.Zone2:
                    break;
                case Constants.Enums.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.Enums.SceneType.OneExit)
        {
            switch(zone)
            {
                case Constants.Enums.Zones.Zone1:
                    ret = Constants.Enums.Scenes.Zone1_OneExit;
                    break;
                case Constants.Enums.Zones.Zone2:
                    break;
                case Constants.Enums.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.Enums.SceneType.TwoExits)
        {
            switch(zone)
            {
                case Constants.Enums.Zones.Zone1:
                    ret = Constants.Enums.Scenes.Zone1_TwoExits;
                    break;
                case Constants.Enums.Zones.Zone2:
                    break;
                case Constants.Enums.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.Enums.SceneType.ThreeExits)
        {
            switch(zone)
            {
                case Constants.Enums.Zones.Zone1:
                    ret = Constants.Enums.Scenes.Zone1_ThreeExits;
                    break;
                case Constants.Enums.Zones.Zone2:
                    break;
                case Constants.Enums.Zones.Zone3:
                    break;
            }
        }
        else if(sceneType == Constants.Enums.SceneType.Boss)
        {
            switch(zone)
            {
                case Constants.Enums.Zones.Zone1:
                    ret = Constants.Enums.Scenes.Zone1_Boss;
                    break;
                case Constants.Enums.Zones.Zone2:
                    break;
                case Constants.Enums.Zones.Zone3:
                    break;
            }
        }

        return ret;
    }
}
