using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransparentTerrain : MonoBehaviourBase
{
    public float TransparentAlpha = 0.5f;

    private GameObject _playerTransform;
    private List<GameObject> _currentlytransparentTerrain = new List<GameObject>();

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
    }

    //Thoughts:
    //  - Use coroutines to fade out the objects over a half a second
    //  - Understand how the shader code works
    //      - Watch: https://www.youtube.com/watch?v=vmLIy62Gsnk
    private void Update()
    {
        if(_playerTransform == null)
        {
            LogDebug("Reaquiring Player Object");
            _playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player);
            return;
        }

        //Calculate GameObjects in between the player and camera
        var distCalculate = Vector3.Distance(_playerTransform.transform.position, Camera.main.transform.position);
        var castRay = new Ray(Camera.main.transform.position, _playerTransform.transform.position - Camera.main.transform.position);

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

            FadeObjectOut(gameObjectHit.GetComponent<Renderer>().materials);
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
            FadeObjectIn(terrain.GetComponent<Renderer>().materials);
            terrainToRemove.Add(terrain);
        }

        _currentlytransparentTerrain.RemoveAll(x => terrainToRemove.Contains(x));
    }

    private void FadeObjectOut(Material[] materials)
    {
        foreach (var material in materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Surface", 1);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", true);

            material.SetOverrideTag("RenderType", "Transparent");

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        }

        while (materials[0].color.a > TransparentAlpha)
        {
            foreach (var material in materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        TransparentAlpha
                    );
                }
            }
        }
    }

    private void FadeObjectIn(Material[] materials)
    {
        while (materials[0].color.a < 1.0f) //Sub in initial alpha here
        {
            foreach (var material in materials)
            {
                if (material.HasProperty("_Color"))
                {
                    material.color = new Color(
                        material.color.r,
                        material.color.g,
                        material.color.b,
                        1.0f //Will likely need to store original alpha
                    );
                }
            }
        }

        foreach (var material in materials)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.SetInt("_Surface", 0);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            material.SetShaderPassEnabled("DepthOnly", true);
            material.SetShaderPassEnabled("SHADOWCASTER", true);

            material.SetOverrideTag("RenderType", "Opaque");

            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }
    }
}
