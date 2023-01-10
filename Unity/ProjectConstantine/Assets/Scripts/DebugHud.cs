using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHud : MonoBehaviourBase
{
    //Scene Name
    //

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Hello World!");
    }
}
