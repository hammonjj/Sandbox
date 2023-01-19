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
            AcquireManagers();
            return;
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
            _sceneStateManager.CurrentSceneType, currentZone);

        _doorManager.AssignOptionsToDoors(roomOptions);
    }

    private void AcquireManagers()
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LogDebug("OnSceneLoaded Called");

        _doorManager = null;
        _sceneStateManager = null;
        _isInitialized = false;
        _recalculateRoomOptions = true;
    }
}
