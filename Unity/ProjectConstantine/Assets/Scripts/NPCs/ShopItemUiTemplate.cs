using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUiTemplate : MonoBehaviourBase
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Cost;

    public void SetItemDetails(string title, string description, int cost)
    {
        Title.text = title;
        Description.text = description;
        Cost.text = cost.ToString();

        //check if player can buy
        var abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        if(abilityTracker.Currency < cost)
        {
            Cost.color = Color.red;
        }
    }
}
