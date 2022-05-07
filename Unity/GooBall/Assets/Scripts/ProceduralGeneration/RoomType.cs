using UnityEngine;
using static Enums;

public class RoomType : MonoBehaviourBase
{
    [Header("RoomType")]
    public RoomOpenings RoomOpenings;

    public void DestroyRoom()
    {
        Destroy(gameObject);
    }
}
