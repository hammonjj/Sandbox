using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    //PC Locations: C:/Users/JamesHammond/AppData/LocalLow/DefaultCompany/GooBall/player-{CurrentGame}.bin
    private static string _saveFilePath = Application.persistentDataPath + "/player-" + GameInformation.CurrentGame + ".bin";

    public static bool DoesSaveExist(int save)
    {
        return File.Exists(_saveFilePath);
    }
    public static void SavePlayer(PlayerController player, Vector2 respawnPoint)
    {
        //Don't save debug player
        if(GameInformation.CurrentGame == 0)
        {
            Debug.Log("Skipping save for debug player");
            return;
        }

        var formatter = new BinaryFormatter();
        using(var stream = new FileStream(_saveFilePath, FileMode.OpenOrCreate))
        {
            var data = new PlayerData(player, respawnPoint);
            formatter.Serialize(stream, data);
        }
    }

    public static PlayerData LoadPlayer()
    {
        Debug.Log("Loading Save Game: " + GameInformation.CurrentGame);
        if(!File.Exists(_saveFilePath))
        {
            throw new Exception("Save file missing from: " + _saveFilePath);
        }

        var formatter = new BinaryFormatter();
        using(var stream = new FileStream(_saveFilePath, FileMode.Open))
        {
            var data = formatter.Deserialize(stream) as PlayerData;
            return data;
        }
    }
}
