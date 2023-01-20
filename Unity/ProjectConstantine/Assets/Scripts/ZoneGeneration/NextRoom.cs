using System.Collections.Generic;
using UnityEngine;

public class NextRoom
{
    public Constants.Enums.SceneType SceneType;
    public Constants.Enums.RoomReward RoomReward;

    public Constants.Enums.Scenes SceneName { get; private set; }

    //Fight Rooms
    private List<Constants.Enums.Scenes> _zone1FightRooms = new List<Constants.Enums.Scenes>()
    {
        Constants.Enums.Scenes.Zone1_Chair,
        Constants.Enums.Scenes.Zone1_Large,
        Constants.Enums.Scenes.Zone1_Square,
    };

    public NextRoom(
        Constants.Enums.SceneType sceneType,
        Constants.Enums.RoomReward roomReward)
    {
        SceneType = sceneType;
        RoomReward = roomReward;

        MapSceneTypeToSceneName();
    }

    private void MapSceneTypeToSceneName()
    {
        var currentZone = GameObject.FindGameObjectWithTag(Constants.Tags.SceneStateManager)
            .GetComponent<SceneStateManager>()
            .GetCurrentZone();

        //Need a system to determine which fight chambers have been used so players
        //won't see the same chamber multiple times
        switch (SceneType)
        {
            case Constants.Enums.SceneType.Shop:
                switch(currentZone)
                {
                    case Constants.Enums.Zones.Zone1:
                        SceneName = Constants.Enums.Scenes.Zone1_Shop_2;
                        break;
                    case Constants.Enums.Zones.Zone2:
                        break;
                    case Constants.Enums.Zones.Zone3:
                        break;
                }
                break;
            case Constants.Enums.SceneType.Rest:
                switch(currentZone)
                {
                    case Constants.Enums.Zones.Zone1:
                        SceneName = Constants.Enums.Scenes.Zone1_Rest_2;
                        break;
                    case Constants.Enums.Zones.Zone2:
                        break;
                    case Constants.Enums.Zones.Zone3:
                        break;
                }
                break;
            case Constants.Enums.SceneType.Story:
                switch(currentZone)
                {
                    case Constants.Enums.Zones.Zone1:
                        SceneName = Constants.Enums.Scenes.Zone1_Story_2;
                        break;
                    case Constants.Enums.Zones.Zone2:
                        break;
                    case Constants.Enums.Zones.Zone3:
                        break;
                }
                break;
            case Constants.Enums.SceneType.Elite:
            case Constants.Enums.SceneType.Fight:
                //Use the same rooms for Elites and Normal Fights for the time being
                SceneName = GetRandomFightRoom(currentZone);
                break;
            case Constants.Enums.SceneType.Boss:
                switch(currentZone)
                {
                    case Constants.Enums.Zones.Zone1:
                        SceneName = Constants.Enums.Scenes.Zone1_Boss;
                        break;
                    case Constants.Enums.Zones.Zone2:
                        break;
                    case Constants.Enums.Zones.Zone3:
                        break;
                }
                break;
            case Constants.Enums.SceneType.None:
                //Do nothing
                break;
        }
    }

    private Constants.Enums.Scenes GetRandomFightRoom(Constants.Enums.Zones zone)
    {
        var scene = Constants.Enums.Scenes.None;
        if(zone == Constants.Enums.Zones.Zone1)
        {
            scene = _zone1FightRooms[Helper.RandomInclusiveRange(0, _zone1FightRooms.Count - 1)];
            Helper.LogDebug($"Next Scene Fight Room: {scene}");
        }

        return scene;
    }
}
