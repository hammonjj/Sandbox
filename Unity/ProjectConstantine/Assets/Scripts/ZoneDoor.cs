using UnityEngine;
using TMPro;

public class ZoneDoor : MonoBehaviourBase
{
    public Constants.Enums.Scenes SceneToGoTo;
    public Constants.Enums.RoomReward NextRoomReward;
    public Constants.Enums.SceneType SceneType;
    public Constants.Enums.DoorType DoorType;

    private bool _enableRoomDoor;
    private bool _isPlayerCloseEnough;
    private GameObject _nextSceneUITextObj;
    private SceneStateManager _sceneStateManager;
    private TextMeshProUGUI _nextSceneUIText;

    private void Awake()
    {
        _nextSceneUITextObj = GameObjectExtensions.FindGameObjectWithTag(Constants.Tags.NextZoneText);
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

    private void Start()
    {
        EventManager.GetInstance().onEncounterEnded += OnEncounterEnded;
        EventManager.GetInstance().onAdvanceScenePressed += OnAdvanceScenePressed;
        _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.Tags.SceneStateManager)?.GetComponent<SceneStateManager>();

        if(_sceneStateManager)
        {
            LogDebug("Acquired Scene Manager");
        }
        else
        {
            LogDebug("Failed to Acquire Scene Manager");
        }

        if(DoorType == Constants.Enums.DoorType.Dumb)
        {
            //Used for Shops and Story segments so they player can leave when they want
            _enableRoomDoor = true;
        }
        else if(_sceneStateManager.GetCurrentZone() == Constants.Enums.Zones.Zone1 &&
            _sceneStateManager.CurrentSceneType == Constants.Enums.SceneType.None)
        {
            //This is to ensure the door locks when we enter the first chamber of the first zone
            _enableRoomDoor = false;
        }
        else
        {
            _enableRoomDoor = _sceneStateManager.CurrentSceneType == Constants.Enums.SceneType.None;
        }
    }

    public void OnEncounterEnded()
    {
        _enableRoomDoor = true;
        LogDebug("Encounter Ended - Enabling Door");
    }

    public void OnAdvanceScenePressed(bool value)
    {
        if(_isPlayerCloseEnough && value && _enableRoomDoor)
        {
            _sceneStateManager.AdvanceToScene(SceneToGoTo, NextRoomReward, SceneType);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Constants.Tags.Player && _enableRoomDoor)
        {
            LogDebug($"Player Entered Exit Zone - Scene to Go To: {SceneToGoTo} - Scene Reward: {NextRoomReward}");
            _isPlayerCloseEnough = true;

            _nextSceneUIText.text = $"Press R1 or F to Advance to {SceneToGoTo}";
            _nextSceneUITextObj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == Constants.Tags.Player && _enableRoomDoor)
        {
            _isPlayerCloseEnough = false;
            LogDebug("Player Left Exit Zone");

            _nextSceneUITextObj.SetActive(false);
        }
    }
}
