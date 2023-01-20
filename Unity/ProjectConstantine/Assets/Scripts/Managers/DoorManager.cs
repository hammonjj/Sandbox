using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviourBase
{
    public List<ZoneDoor> ZoneDoors 
    {
        get
        {
            if(!_initializaed)
            {
                _initializaed = true;
                SetRoomDoors();
                GetRoomDoorObjects();
            }

            return _zoneDoors;
        }
    }

    private bool _initializaed = false;
    private List<ZoneDoor> _zoneDoors = new();

    private void Start()
    {
        if(_initializaed)
        {
            LogDebug("Already initialized");
            return;
        }

        _initializaed = true;
        SetRoomDoors();
        GetRoomDoorObjects();
    }

    private void SetRoomDoors()
    {
        //Decide how many doors this room will have
        //  - Check to see if OneDoor, TwoDoors or ThreeDoors exist in scene
        //  - Pick a number and enable that game object
        LogDebug("Setting room doors");

        var sceneManager = GameObject.FindGameObjectWithTag(Constants.Tags.SceneStateManager).GetComponent<SceneStateManager>();
        if(sceneManager.CurrentSceneType == Constants.Enums.SceneType.None)
        {
            LogDebug("Not in a Zone");
            return;
        }

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

    private void GetRoomDoorObjects()
    {
        var doors = GameObject.FindGameObjectsWithTag(Constants.Tags.ZoneDoor);

        LogDebug($"Found {doors.Length} doors");
        foreach(var door in doors)
        {
            _zoneDoors.Add(door.GetComponent<ZoneDoor>());
        }
    }

    public void AssignOptionsToDoors(List<NextRoom> roomOptions)
    {
        LogDebug("Assigning options to room doors");
        if(!_initializaed)
        {
            _initializaed = true;
            SetRoomDoors();
            GetRoomDoorObjects();
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
            _zoneDoors[i].SceneType = roomOptions[i].SceneType;

            LogDebug($"Door Assigned - Name: {_zoneDoors[i].name} - " +
                $"SceneToGoTo: {roomOptions[i].SceneName} - NextRoomReward: {roomOptions[i].RoomReward}");
        }
    }
}
