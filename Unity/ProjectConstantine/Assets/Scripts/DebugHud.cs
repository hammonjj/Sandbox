using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugHud : MonoBehaviourBase
{
    //Scene Name
    //Door List
    //  - Next Scene
    //  - Room Reward
    private DoorManager _doorManager;
    private GameStateManager _gameStateManager;
    private SceneStateManager _sceneStateManager;
    
    private void Start()
    {
        
    }

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
            LogDebug("One or more managers are null - waiting");
            return;
        }

        //LogDebug($"SceneManager.GetActiveScene().name: {SceneManager.GetActiveScene().name}");
        var currentScene = "Scene: " + SceneManager.GetActiveScene().name;
        LogDebug(currentScene);
        GUI.Label(new Rect(10, 10, 100, 20), currentScene);
    }
}
