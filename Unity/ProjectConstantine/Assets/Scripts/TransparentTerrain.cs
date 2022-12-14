using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransparentTerrain : MonoBehaviourBase
{
    public Transform PlayerTransform;
    public float TransparentAlpha = 0.5f;

    private RaycastHit _castHit;
    private List<GameObject> _currentlytransparentTerrain = new List<GameObject>();

    void Start()
    {
    }
    
    void Update()
    {
        //Calculate GameObjects in between the player and camera
        var distCalculate = Vector3.Distance(PlayerTransform.position, Camera.main.transform.position);
        var castRay = new Ray(Camera.main.transform.position, PlayerTransform.position - Camera.main.transform.position);

        var rayCastHits = Physics.RaycastAll(castRay, distCalculate);
        if(rayCastHits.Length == 0)
        {
            return;
        }

        //Make terrain in the way transparent
        var validHits = new List<GameObject>();
        foreach(var raycastHit in rayCastHits)
        {
            if(raycastHit.collider == null || !raycastHit.collider.CompareTag("TransparentTerrain"))
            {
                continue;
            }

            var gameObjectHit = raycastHit.collider.gameObject;
            validHits.Add(gameObjectHit);

            //If the object is already transparent, no need to do it again
            if(_currentlytransparentTerrain.Contains(gameObjectHit))
            {
                continue;
            }
            LogDebug($"Valid Hit Detected: {gameObjectHit.name}");
            MakeTerrainTransparent(gameObjectHit);
            _currentlytransparentTerrain.Add(gameObjectHit);
        }
        
        //No need to continue if nothing is transparent
        if(_currentlytransparentTerrain.Count == 0)
        {
            return;
        }
        
        //Make terrain we've moved away from non-transparent
        var terrainNoLongerBlocking = _currentlytransparentTerrain.Except(validHits);
        var terrainToRemove = new List<GameObject>();
        foreach(var terrain in terrainNoLongerBlocking)
        {
            LogDebug($"Object no longer obstructing view: {terrain.name}");
            MakeTerrainOpaque(terrain);
            terrainToRemove.Add(terrain);
        }

        _currentlytransparentTerrain.RemoveAll(x => terrainToRemove.Contains(x));
    }

    private void MakeTerrainTransparent(GameObject gameObjectHit)
    {
        //Try getting all renderers of children and setting them to transparent
        //  - https://docs.unity3d.com/ScriptReference/GameObject.GetComponentsInChildren.html
        //var renderers = gameObjectHit.GetComponentsInChildren<Renderer>();
        var renderer = gameObjectHit.GetComponent<Renderer>();
        renderer.material.color = new Color(
            renderer.material.color.r,
            renderer.material.color.g,
            renderer.material.color.b,
            TransparentAlpha);
    }

    private void MakeTerrainOpaque(GameObject gameObjectHit)
    {
        
        //This might be an issue if I use materials that aren't always totally opaque
        //Might need to modify so we store both the GameObject as well as its original
        //color value in a key/value pair
        var renderer = gameObjectHit.GetComponent<Renderer>();
        renderer.material.color = new Color(
            renderer.material.color.r,
            renderer.material.color.g,
            renderer.material.color.b,
            1.0f);
    }
}
