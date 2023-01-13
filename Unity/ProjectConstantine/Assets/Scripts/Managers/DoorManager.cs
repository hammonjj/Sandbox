using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviourBase
{
    public List<ZoneDoor> ZoneDoors 
    {
        get
        {
            return _zoneDoors;
        }
    }

    private List<ZoneDoor> _zoneDoors = new List<ZoneDoor>();

    public void AssignOptionsToDoors(List<(Constants.Enums.Scenes, Constants.Enums.RoomReward)> sceneOptions)
    {
        if(_zoneDoors.Count == 0)
        {
            var doors = GameObject.FindGameObjectsWithTag(Constants.Tags.ZoneDoor);

            LogDebug($"Found {doors.Length} doors");
            foreach(var door in doors)
            {
                _zoneDoors.Add(door.GetComponent<ZoneDoor>());
            }
        }

        if(_zoneDoors.Count == 0)
        {
            LogError("No Doors Detected");
        }

        if(_zoneDoors.Count != sceneOptions.Count)
        {
            LogError($"Doors to Options Mismatch - " +
                $"_zoneDoors.Count: {_zoneDoors.Count} - scenes.Count: {sceneOptions.Count}");
        }

        
        for(var i = 0; i < _zoneDoors.Count; i++)
        {
            _zoneDoors[i].SceneToGoTo = sceneOptions[i].Item1;
            _zoneDoors[i].NextRoomReward = sceneOptions[i].Item2;

            LogDebug($"Door Assigned - Name: {_zoneDoors[i].name} - " +
                $"SceneToGoTo: {sceneOptions[i].Item1} - NextRoomReward: {sceneOptions[i].Item2}");
        }
    }
}
