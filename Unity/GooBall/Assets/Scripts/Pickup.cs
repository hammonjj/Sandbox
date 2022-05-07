using UnityEngine;
using static Enums;

public class Pickup : MonoBehaviourBase
{
    [Header("Item")]
    public ItemType Type;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            LogDebug("Player picked up a key");
            var inventory = col.gameObject.GetComponent<Inventory>();
            inventory.AddItem(Type);

            Destroy(this.gameObject);
        }
    }
}
