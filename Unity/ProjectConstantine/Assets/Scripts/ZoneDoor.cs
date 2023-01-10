using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneDoor : MonoBehaviourBase
{
    public Constants.Scenes SceneToGoTo;
    public Constants.RoomReward NextRoomReward;

    private bool _isPlayerCloseEnough;
    private GameObject _nextSceneUITextObj;
    private SceneStateManager _sceneStateManager;
    private DoorManager _doorManager;
    private TextMeshProUGUI _nextSceneUIText;

    private void Awake()
    {
        _nextSceneUITextObj = Extensions.FindGameObjectWithTag(Constants.NextZoneText);
        if(_nextSceneUITextObj == null)
        {
            LogError("Couldn't located NextZoneText game object");
        }
        else
        {
            _nextSceneUITextObj.SetActive(false);
            _nextSceneUIText = _nextSceneUITextObj.GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if(_sceneStateManager == null)
        {
            _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager)?.GetComponent<SceneStateManager>();

            if(_sceneStateManager != null)
            {
                LogDebug("Acquired SceneManager");
                //_sceneStateManager = sceneManagerObj.GetComponent<SceneStateManager>();
            }
        }

        if(_doorManager == null)
        {
            _doorManager = GameObject.FindGameObjectWithTag(Constants.DoorManager)?.GetComponent<DoorManager>();
            
            if(_doorManager != null)
            {
                _doorManager.AddDoor(this);
                LogDebug("Acquired DoorManager");
            }
        }

        if(_sceneStateManager == null)
        {
            return;
        }

        if (_isPlayerCloseEnough && _sceneStateManager.AdvanceScenePressed)
        {
            _sceneStateManager.AdvanceToScene(SceneToGoTo, NextRoomReward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Constants.Player)
        {
            LogDebug("Player Entered Exit Zone");
            _isPlayerCloseEnough = true;

            _nextSceneUIText.text = $"Press R1 or F to Advance to {SceneToGoTo}";
            _nextSceneUITextObj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == Constants.Player)
        {
            _isPlayerCloseEnough = false;
            LogDebug("Player Left Exit Zone");

            _nextSceneUITextObj.SetActive(false);
        }
    }
}
