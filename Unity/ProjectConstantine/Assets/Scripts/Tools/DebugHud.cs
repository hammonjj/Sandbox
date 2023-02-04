using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugHud : MonoBehaviourBase
{
    public float LeftBorder = 310;
    public float VerticalBorder = 275;

    //private float _rightBorder = 1500;
    private float _leftVerticalPadding = 20;
    //private float _rightVerticalPadding = 75;

    private DoorManager _doorManager;
    private GameStateManager _gameStateManager;
    private SceneStateManager _sceneStateManager;

    private void Start()
    {
        //get camer
        //get player
        //get distance during update
    }

    private void Update()
    {
        if(_doorManager == null)
        {
            _doorManager = GameObject.FindGameObjectWithTag(Constants.Tags.DoorManager)?.GetComponent<DoorManager>();
            if(_doorManager == null)
            {
                return;
            }

            LogDebug("Acquired Door Manager");
        }

        if(_sceneStateManager == null)
        {
            _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.Tags.SceneStateManager)?.GetComponent<SceneStateManager>();
            if(_sceneStateManager == null)
            {
                return;
            }

            LogDebug("Acquired Scene Manager");
        }

        if(_gameStateManager == null)
        {
            _gameStateManager = GameObject.FindGameObjectWithTag(Constants.Tags.GameStateManager)?.GetComponent<GameStateManager>();
            if(_gameStateManager == null)
            {
                return;
            }

            LogDebug("Acquired Game Manager");
        }
    }

    private void OnGUI()
    {
        if(_doorManager == null ||
            _sceneStateManager == null ||
            _gameStateManager == null)
        {
            LogDebug("One or more managers are null");
            return;
        }

        GenerateSceneInformation();
    }

    private void GenerateSceneInformation()
    {
        GUI.Label(new Rect(LeftBorder, VerticalBorder + _leftVerticalPadding * 0, 300, 20), $"Scene: {SceneManager.GetActiveScene().name}");
        GUI.Label(new Rect(LeftBorder, VerticalBorder + _leftVerticalPadding * 1, 300, 20), $"Scene Type: {_sceneStateManager.CurrentSceneType}");
        GUI.Label(new Rect(LeftBorder, VerticalBorder + _leftVerticalPadding * 2, 300, 20), $"Room Reward: {_sceneStateManager.CurrentRoomReward}");
        GUI.Label(new Rect(LeftBorder, VerticalBorder + _leftVerticalPadding * 3, 300, 20), $"Current Chamber: {_gameStateManager.CurrentChamber}");
        GUI.Label(new Rect(LeftBorder, VerticalBorder + _leftVerticalPadding * 4, 300, 20), $"Chamber Limit: {_gameStateManager.ZoneMaximumChambers}");

        //Get All Doors and display RoomReward and SceneName
        var labelNumber = 6;
        var zoneDoors = _doorManager.ZoneDoors;

        if(zoneDoors.Count > 0)
        {
            GUI.Label(new Rect(LeftBorder, VerticalBorder + _leftVerticalPadding * 5, 300, 20), $"Zone Doors:");
        }

        for(int i = 0; i < _doorManager.ZoneDoors.Count; i++)
        {
            GUI.Label(
                new Rect(LeftBorder, VerticalBorder + _leftVerticalPadding * labelNumber, 350, 20),
                $"    - Door #{i + 1} -> Scene: {_doorManager.ZoneDoors[i].SceneToGoTo} - " +
                    $"Reward: {_doorManager.ZoneDoors[i].NextRoomReward}");

            labelNumber += 1;
        }
    }
}
