using System.Collections.Generic;
using System.Linq;
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

    private void Awake()
    {
        //Decide how many doors this room will have
        //  - Check to see if OneDoor, TwoDoors or ThreeDoors exist in scene
        //  - Pick a number and enable that game object
        LogDebug("Setting room doors");
        var tags = new List<string>()
        {
            Constants.Tags.OneDoor,
            Constants.Tags.TwoDoors,
            Constants.Tags.ThreeDoors
        };

        //new string[] { "1", "2", "3" }.ToList()
        var doorConfigurations = Extensions.FindGameObjectsWithTags(tags);
        var doorNumber = doorConfigurations[Helper.RandomInclusiveRange(0, doorConfigurations.Count - 1)];
        doorNumber.SetActive(true);

        //Set the others as inactive
        foreach(var config in doorConfigurations)
        {
            if(config.name == doorNumber.name)
            {
                continue;
            }

            config.SetActive(false);
        }

        LogDebug($"Door Configuration Chosen: {doorNumber.name}");
    }

    public void AssignOptionsToDoors(List<NextRoom> roomOptions)
    {
        LogDebug("Assigning options to room doors");
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

        if(_zoneDoors.Count != roomOptions.Count)
        {
            LogError($"Doors to Options Mismatch - " +
                $"_zoneDoors.Count: {_zoneDoors.Count} - scenes.Count: {roomOptions.Count}");
        }
        
        for(var i = 0; i < _zoneDoors.Count; i++)
        {
            _zoneDoors[i].SceneToGoTo = roomOptions[i].SceneName;
            _zoneDoors[i].NextRoomReward = roomOptions[i].RoomReward;

            LogDebug($"Door Assigned - Name: {_zoneDoors[i].name} - " +
                $"SceneToGoTo: {roomOptions[i].SceneName} - NextRoomReward: {roomOptions[i].RoomReward}");
        }
    }
}
