using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneDataContainer : MonoBehaviourBase
{
    [System.Serializable]
    public class EnemyEntry
    {
        public int MaxPerWave;
        public GameObject Prefab;
        public Constants.Enums.EnemyType EnemyType;
    }
    /*
    [System.Serializable]
    public class ChamberData
    {
        public int EnemiesPerWave;
        public Constants.Enums.Scenes Scene;
    }
    */
    public ZoneChamberData[] ZoneChamberData;
    
    public EnemyEntry[] Zone1Enemies;
    //public ChamberData[] Zone1ChamberData;

    private Dictionary<(Constants.Enums.Zones, Constants.Enums.RoomType), EnemyEntry[]> _enemyDictionary = new();

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

    public List<EnemyEntry> GetAvailableNormalEnemies(Constants.Enums.Zones zone, Constants.Enums.RoomType roomType)
    {
        return (_enemyDictionary[(zone, roomType)]).ToList();
    }

    public ZoneChamberData.ChamberData GetDataForChamber(Constants.Enums.Scenes scene, Constants.Enums.Zones zone)
    {

        try
        {
            return ZoneChamberData.Where(x => x.Zone == zone).First().ChamberDataObj.Where(x => x.Scene == scene).First();
        }
        catch
        {
            return null;
        }
    }
}
