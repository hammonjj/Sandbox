using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviourBase
{
    public GameDesignSettings GameDesignSettings;
    public Constants.Enums.SceneType NextSceneType;
    public Constants.Enums.RoomReward NextRoomReward;

    public List<Constants.Enums.Scenes> AvailableZoneFightChambers = new();

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

        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            RegisterEventListeners();
            AvailableZoneFightChambers = Constants.Enums.Zone1FightRooms;
        }
        else
        {
            Destroy(this);
        }
    }

    private void RegisterEventListeners()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventManager.GetInstance().onGameReset += OnGameReset;
        EditorApplication.playModeStateChanged += OnPlayModeChange;
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

    private void OnGameReset()
    {
        LogDebug("Reseting Game");
        SceneManager.LoadScene(Constants.Enums.Scenes.WorldHub.ToString());

        Destroy(_instance);
        _instance = null;
    }

    private void AcquireManagers()
    {
        if(_sceneStateManager == null)
        {
            _sceneStateManager = GameObject.FindGameObjectWithTag(
                Constants.Tags.SceneStateManager)?.GetComponent<SceneStateManager>();
        }

        if(_doorManager == null)
        {
            _doorManager = GameObject.FindGameObjectWithTag(
                Constants.Tags.DoorManager)?.GetComponent<DoorManager>();
        }

        if(_doorManager != null && _sceneStateManager != null)
        {
            _isInitialized = true;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LogDebug("OnSceneLoaded Called");

        _doorManager = null;
        _sceneStateManager = null;
        _isInitialized = false;
        _recalculateRoomOptions = true;

        RegisterEventListeners();
    }

    private static void OnPlayModeChange(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            Helper.LogDebug("Reseting GameStateManager");
            Destroy(_instance);
            _instance = null;
        }
    }
}
