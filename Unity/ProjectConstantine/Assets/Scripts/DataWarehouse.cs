using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataWarehouse : MonoBehaviourBase
{
    [Header("Shop")]
    public UpgradeData[] ShopItemData;
    public GameObject ShopItemUiTemplate;

    [Header("Orbs")]
    public GameObject PrimaryOrbPrefab;

    private static DataWarehouse _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            EditorApplication.playModeStateChanged += OnPlayModeChange;
        }
        else
        {
            Destroy(this);
        }
    }

    private static void OnPlayModeChange(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
        {
            Helper.LogDebug("Reseting DataWarehouse");
            _instance = null;
        }
    }
}
