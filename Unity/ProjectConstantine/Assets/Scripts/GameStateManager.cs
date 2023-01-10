using UnityEngine;

public class GameStateManager : MonoBehaviourBase
{
    public GameDesignSettings GameDesignSettings;

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

        //GameDesignSettings.Zone1Chambers
        _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager).GetComponent<SceneStateManager>();
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

        //var doorManager = GameObject.FindGameObjectWithTag(Constants.DoorManager).GetComponent<DoorManager>();
        //Call Scene Manager and assign each room to a door
    }
}
