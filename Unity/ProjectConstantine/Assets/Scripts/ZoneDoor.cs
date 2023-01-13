using UnityEngine;
using TMPro;

public class ZoneDoor : MonoBehaviourBase
{
    public Constants.Scenes SceneToGoTo;
    public Constants.RoomReward NextRoomReward;

    private bool _isPlayerCloseEnough;
    private GameObject _nextSceneUITextObj;
    private SceneStateManager _sceneStateManager;
    private TextMeshProUGUI _nextSceneUIText;

    private void Awake()
    {
        EventManager.GetInstance().onAdvanceScenePressed += OnAdvanceScenePressed;

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

    private void Start()
    {
        _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager)?.GetComponent<SceneStateManager>();

        if(_sceneStateManager)
        {
            LogDebug("Acquired Scene Manager");
        }
        else
        {
            LogDebug("Failed to Acquire Scene Manager");
        }
    }

    private void Update()
    {
        /*
        if(_sceneStateManager == null)
        {
            _sceneStateManager = GameObject.FindGameObjectWithTag(Constants.SceneStateManager)?.GetComponent<SceneStateManager>();

            if(_sceneStateManager != null)
            {
                LogDebug("Acquired SceneManager");
            }
        }
        */
    }

    public void OnAdvanceScenePressed(bool value)
    {
        if(_isPlayerCloseEnough && value)
        {
            _sceneStateManager.AdvanceToScene(SceneToGoTo, NextRoomReward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == Constants.Player)
        {
            LogDebug($"Player Entered Exit Zone - Scene to Go To: {SceneToGoTo} - Scene Reward: {NextRoomReward}");
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
