using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugHud : MonoBehaviourBase
{
    private float _rightBorder = 1500;
    private float _leftBorder = 310;
    private float _leftVerticalPadding = 20;
    private float _rightVerticalPadding = 75;

    private DoorManager _doorManager;
    private GameStateManager _gameStateManager;
    private SceneStateManager _sceneStateManager;

    private void Update()
    {
        if(_doorManager == null)
        {
            _doorManager = GameObject.FindGameObjectWithTag(Constants.DoorManager)?.GetComponent<DoorManager>();
            if(_doorManager == null)
            {
                return;
            }

            LogDebug("Acquired Door Manager");
        }

        if(_sceneStateManager == null)
        {
            _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager)?.GetComponent<SceneStateManager>();
            if(_sceneStateManager == null)
            {
                return;
            }

            LogDebug("Acquired Scene Manager");
        }

        if(_gameStateManager == null)
        {
            _gameStateManager = GameObject.FindGameObjectWithTag(Constants.GameStateManager)?.GetComponent<GameStateManager>();
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

        GenerateDebugButtons();
        GenerateSceneInformation();
    }

    private void GenerateDebugButtons()
    {
        if(GUI.Button(new Rect(_rightBorder, 275 + _rightVerticalPadding * 0, 50, 50), "Hurt Player (10)"))
        {
            LogDebug("Hurting player for 10 damage");
        }

        if(GUI.Button(new Rect(_rightBorder, 275 + _rightVerticalPadding * 1, 200, 50), "Kill Player"))
        {
            LogDebug("Killing player");
            EventManager.GetInstance().OnPlayerDeath();
        }

        if(GUI.Button(new Rect(_rightBorder, 275 + _rightVerticalPadding * 2, 50, 50), "Kill All Enemies"))
        {
            LogDebug("Killing all enemies");
        }

        //Kill All Enemies
    }

    private void GenerateSceneInformation()
    {
        GUI.Label(new Rect(_leftBorder, 275 + _leftVerticalPadding * 0, 300, 20), $"Scene: {SceneManager.GetActiveScene().name}");
        GUI.Label(new Rect(_leftBorder, 275 + _leftVerticalPadding * 1, 300, 20), $"Scene Type: {_sceneStateManager.CurrentSceneType}");
        GUI.Label(new Rect(_leftBorder, 275 + _leftVerticalPadding * 2, 300, 20), $"Room Reward: {_sceneStateManager.CurrentRoomReward}");
        GUI.Label(new Rect(_leftBorder, 275 + _leftVerticalPadding * 3, 300, 20), $"Current Chamber: {_gameStateManager.CurrentChamber}");
        GUI.Label(new Rect(_leftBorder, 275 + _leftVerticalPadding * 4, 300, 20), $"Chamber Limit: {_gameStateManager.ZoneMaximumChambers}");

        //Get All Doors and display RoomReward and SceneName
        var labelNumber = 6;
        var zoneDoors = _doorManager.ZoneDoors;

        if(zoneDoors.Count > 0)
        {
            GUI.Label(new Rect(_leftBorder, 275 + _leftVerticalPadding * 5, 300, 20), $"Zone Doors:");
        }

        for(int i = 0; i < _doorManager.ZoneDoors.Count; i++)
        {
            GUI.Label(
                new Rect(_leftBorder, 275 + _leftVerticalPadding * labelNumber, 350, 20),
                $"    - Door #{i + 1} -> Scene: {_doorManager.ZoneDoors[i].SceneToGoTo} - " +
                    $"Reward: {_doorManager.ZoneDoors[i].NextRoomReward}");

            labelNumber += 1;
        }
    }
}
