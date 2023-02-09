using TMPro;

public class PlayerCurrency : MonoBehaviourBase
{
    private PlayerTracker _playerTracker;
    private TextMeshProUGUI _currencyTextObj;

    private void Start()
    {
        //Temp object until I create a real UI
        _currencyTextObj = VerifyComponent<TextMeshProUGUI>();
        _playerTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
    }

    private void Update()
    {
        _currencyTextObj.text = _playerTracker.Currency.ToString();
    }
}
