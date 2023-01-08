using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneDoor : MonoBehaviourBase
{
    public string SceneNameToGoTo;
    public GameObject NextSceneUITextObj;

    private bool _isPlayerCloseEnough;
    private GameStateManager _gameStateManager;

    private void Awake()
    {
        NextSceneUITextObj.SetActive(false);
        var text = NextSceneUITextObj.GetComponent<TextMeshProUGUI>();
        text.text = $"Press R1 or F to Advance to {SceneNameToGoTo}";

        _gameStateManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
    }

    private void Update()
    {
        if(_isPlayerCloseEnough && _gameStateManager.AdvanceScenePressed)
        {
            SceneManager.LoadScene(SceneNameToGoTo);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            LogDebug("Player Entered Exit Zone");
            _isPlayerCloseEnough = true;

            NextSceneUITextObj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _isPlayerCloseEnough = false;
            LogDebug("Player Left Exit Zone");

            NextSceneUITextObj.SetActive(false);
        }
    }
}
