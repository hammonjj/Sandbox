using System.Collections.Generic;

public class DoorManager : MonoBehaviourBase
{
    private List<ZoneDoor> _zoneDoors = new List<ZoneDoor>();

    public void AddDoor(ZoneDoor door)
    {
        LogDebug($"Adding Door");
        _zoneDoors.Add(door);
    }

    public void AssignOptionsToDoors(List<Constants.Scenes> scenes)
    {
        if(_zoneDoors.Count != scenes.Count)
        {
            LogError($"Doors to Options Mismatch - " +
                $"_zoneDoors.Count: {_zoneDoors.Count} - scenes.Count: {scenes.Count}");
        }

        for(var i = 0; i < _zoneDoors.Count; i++)
        {
            _zoneDoors[i].SceneToGoTo = scenes[i];
        }
    }
}
