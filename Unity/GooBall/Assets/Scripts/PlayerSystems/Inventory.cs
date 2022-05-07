using UnityEngine;
using static Enums;

public class Inventory : MonoBehaviourBase
{
    [Header("Inventory")]
    public int KeyCount;

    public void AddItem(ItemType type)
    {
        switch(type)
        {
            case ItemType.Key:
                KeyCount++;
                break;
        }
    }

    public bool RemoveItem(ItemType type)
    {
        switch(type)
        {
            case ItemType.Key:
                if(KeyCount <= 0)
                {
                    return false;
                }

                KeyCount--;
                break;
        }

        return true;
    }
}
