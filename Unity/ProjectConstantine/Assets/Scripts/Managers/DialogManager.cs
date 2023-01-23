using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogManager
{
    private static DialogManager _instance;

    public DialogManager()
	{
	}

    public static DialogManager GetInstance()
    {
        if(_instance == null)
        {
            _instance = new DialogManager();
        }

        return _instance;
    }

    public List<string> GetDialog(Constants.Enums.Characters character)
    {
        var ret = new List<string>();

        return ret;
    }
}

