using UnityEngine;
using TMPro;

public class ZoneDoor : MonoBehaviourBase
{
    public Constants.Enums.Scenes SceneToGoTo;
    public Constants.Enums.RoomReward NextRoomReward;

    private bool _enableRoomDoor;
    private bool _isPlayerCloseEnough;
    private GameObject _nextSceneUITextObj;
    private SceneStateManager _sceneStateManager;
    private TextMeshProUGUI _nextSceneUIText;

    private void Awake()
    {
        EventManager.GetInstance().onAdvanceScenePressed += OnAdvanceScenePressed;

        _nextSceneUITextObj = Extensions.FindGameObjectWithTag(Constants.Tags.NextZoneText);
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
        _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.Tags.SceneStateManager)?.GetComponent<SceneStateManager>();

        if(_sceneStateManager)
        {
            LogDebug("Acquired Scene Manager");
        }
        else
        {
            LogDebug("Failed to Acquire Scene Manager");
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
            _sceneStateManager.AdvanceToScene(SceneToGoTo, NextRoomReward);
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
