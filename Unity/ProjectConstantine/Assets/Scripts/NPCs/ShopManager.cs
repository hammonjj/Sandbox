using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviourBase
{
    public int NumOfItems = 3;
    public UpgradeData[] ShopItemData;
    public GameObject ShopItemUiTemplate;
    public GameObject ItemUiListParent;

    private void Start()
    {
        var dataWarehouse = VerifyComponent<DataWarehouse>(Constants.Tags.GameStateManager);
        ShopItemData = dataWarehouse.ShopItemData;
        ShopItemUiTemplate = dataWarehouse.ShopItemUiTemplate;

        RemoveAcquiredUpgrades();

        var rnd = new System.Random();
        ShopItemData = ShopItemData.OrderBy(x => rnd.Next()).ToArray(); //Randomize Shop Items

        for(int i = 0; i < NumOfItems; i++)
        {
            var itemData = ShopItemData[i];
            var newItem = Instantiate(ShopItemUiTemplate, ItemUiListParent.transform);
            newItem.GetComponent<ShopItemUiTemplate>()
                .SetItemDetails(itemData.Title, itemData.Description, itemData.Cost);

            var button = newItem.GetComponent<Button>();
            button.onClick.AddListener(() => { ItemSelected(itemData); });

            if(i == 0)
            {
                EventSystem.current.SetSelectedGameObject(newItem, null);
            }
        }
    }

    private void RemoveAcquiredUpgrades()
    {
        var abilityTracker = VerifyComponent<PlayerTracker>(Constants.Tags.GameStateManager);
        var upgrades = abilityTracker.GetCurrentUpgrades(Constants.Enums.UpgradeType.All);

        var shopItemDataList = ShopItemData.ToList();
        foreach(var upgrade in upgrades)
        {
            //Upgrades with an empty ID can be purchased multiple times
            if(upgrade.Id == string.Empty)
            {
                continue;
            }

            shopItemDataList.RemoveAll(x => x.Id == upgrade.Id);
        }

        ShopItemData = shopItemDataList.ToArray();
    }

    public void ItemSelected(UpgradeData itemData)
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
