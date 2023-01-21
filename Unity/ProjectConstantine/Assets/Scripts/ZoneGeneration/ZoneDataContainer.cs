using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDataContainer : MonoBehaviourBase
{
    //ScriptableObject that contains prefabs for Boss/Normal/Elite enemies
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public List<GameObject> GetAvailableBossEnemies(Constants.Enums.Zones zone, Constants.Enums.RoomType roomType)
    {
        var ret = new List<GameObject>();
        return ret;
    }

    public List<GameObject> GetAvailableEliteEnemies(Constants.Enums.Zones zone, Constants.Enums.RoomType roomType)
    {
        var ret = new List<GameObject>();
        return ret;
    }

    public List<GameObject> GetAvailableNormalEnemies(Constants.Enums.Zones zone, Constants.Enums.RoomType roomType)
    {
        var ret = new List<GameObject>();
        return ret;
    }
}
