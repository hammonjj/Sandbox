using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneDataContainer : MonoBehaviourBase
{
    public GameObject[] Zone1Enemies;

    private Dictionary<(Constants.Enums.Zones, Constants.Enums.RoomType), GameObject[]> _enemyDictionary = new();

    private void Awake()
    {
        (Constants.Enums.Zones, Constants.Enums.RoomType) key = (Constants.Enums.Zones.Zone1, Constants.Enums.RoomType.Normal);
        _enemyDictionary.Add(key, Zone1Enemies);
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
        return (_enemyDictionary[(zone, roomType)]).ToList();
    }
}
