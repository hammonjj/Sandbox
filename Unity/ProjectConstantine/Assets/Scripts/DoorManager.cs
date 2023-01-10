using System.Collections.Generic;

public class DoorManager : MonoBehaviourBase
{
    private List<ZoneDoor> _zoneDoors = new List<ZoneDoor>();

    public void AddDoor(ZoneDoor door)
    {
        _zoneDoors.Add(door);
        LogDebug($"Adding Door - Door Count: {_zoneDoors.Count}");
    }

    public void AssignOptionsToDoors(List<(Constants.Scenes, Constants.RoomReward)> sceneOptions)
    {
        
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
