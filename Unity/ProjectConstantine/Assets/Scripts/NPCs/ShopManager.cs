using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviourBase
{
    public int NumOfItems = 3;
    public ShopItemData[] ShopItemData;
    public GameObject ShopItemUiTemplate; // -> ShopItemUiTemplate
    public GameObject ItemUiListParent;

    private void Start()
    {
        var dataWarehouse = VerifyComponent<DataWarehouse>(Constants.Tags.GameStateManager);
        ShopItemData = dataWarehouse.ShopItemData;
        ShopItemUiTemplate = dataWarehouse.ShopItemUiTemplate;

        var rnd = new System.Random();
        ShopItemData = ShopItemData.OrderBy(x => rnd.Next()).ToArray(); //Randomize Shop Items

        for(int i = 0; i < NumOfItems; i++)
        {
            var itemData = ShopItemData[i];
            var newItem = Instantiate(ShopItemUiTemplate, ItemUiListParent.transform);
            newItem.GetComponent<ShopItemUiTemplate>()
                .SetItemDetails(itemData.Name, itemData.Description, itemData.Cost);

            var button = newItem.GetComponent<Button>();
            button.onClick.AddListener(() => { ItemSelected(itemData); });

            if(i == 0)
            {
                EventSystem.current.SetSelectedGameObject(newItem, null);
            }
        }
    }

    public void ItemSelected(ShopItemData itemData)
    {
        LogDebug($"Item Selected: {itemData.name}");

        var abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        if(abilityTracker.Currency < itemData.Cost)
        {
            return;
        }

        EventManager.GetInstance().OnUpgradePurchase(itemData);
    }
}
