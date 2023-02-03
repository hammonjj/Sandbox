using UnityEngine;
using UnityEngine.EventSystems;

public class ShopKeeper : MonoBehaviourBase
{
    public GameObject ShopUi;


    public float PlayerDistance;

    private bool _isPlayerCloseEnough = false;
    private Transform _playerTransform;

    private void Start()
    {
        EventManager.GetInstance().onAdvanceScenePressed += OnInteract;
        _playerTransform = VerifyComponent<Transform>(Constants.Tags.Player);
    }

    private void Update()
    {
        DebugLines();

        _isPlayerCloseEnough = Vector3.Distance(
            gameObject.transform.position, _playerTransform.position) <= PlayerDistance;

        if(!_isPlayerCloseEnough && ShopUi.activeSelf)
        {
            ShopUi.SetActive(false);
            EventManager.GetInstance().OnPausePlayerController(false);
        }
    }

    private void OnInteract(bool value)
    {
        if(value && ShopUi.activeSelf)
        {
            ShopUi.SetActive(false);
            EventManager.GetInstance().OnPausePlayerController(false);
            return;
        }

        if(!value || !_isPlayerCloseEnough)
        {
            return;
        }

        LogDebug("Opening Shop UI");
        ShopUi.SetActive(true);
        EventManager.GetInstance().OnPausePlayerController(true);
    }

    private void DebugLines()
    {
        if(!Application.isEditor)
        {
            return;
        }

        var rotation = gameObject.transform.rotation;
        rotation *= Quaternion.Euler(90, 0, 0);

        Debug.DrawCircle(
            gameObject.transform.position,
            rotation,
            PlayerDistance,
            Color.red);
    }
}
