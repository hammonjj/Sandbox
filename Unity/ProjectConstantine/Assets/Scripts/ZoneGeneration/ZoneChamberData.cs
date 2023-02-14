using UnityEngine;

[CreateAssetMenu(fileName = "ZoneChamberData", menuName = "ZoneChamberData")]
public class ZoneChamberData : ScriptableObjectBase
{
    public Constants.Enums.Zones Zone;
    public int ZoneLength;

    [System.Serializable]
    public class ChamberData
    {
        public int EnemiesPerWave;
        public Constants.Enums.Scenes Scene;
    }

    public ChamberData[] ChamberDataObj;

}
