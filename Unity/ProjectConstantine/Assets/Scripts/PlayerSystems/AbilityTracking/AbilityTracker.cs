using UnityEditor;
using UnityEngine;

public class AbilityTracker : MonoBehaviourBase
{
    public PlayerHealthTracker PlayerHealthTracker = new();
    public PrimaryOrbUpgradeTracker PrimaryOrbUpgradeTracker = new();


    //Secondary Ring Stats
    public bool SecondaryRingActive = false;
    public int SecondaryMaxOrbs = 3;
    public GameObject SecondaryOrbPrefab;

    private static AbilityTracker _instance;

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
            Helper.LogDebug("Reseting AbilityTracker");
            _instance = null;
        }
    }
}

