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
//            MakeTerrainOpaque(terrain);
            terrainToRemove.Add(terrain);
        }

        _currentlytransparentTerrain.RemoveAll(x => terrainToRemove.Contains(x));
    }

    public enum SurfaceType
    {
        Opaque,
        Transparent
    }

    public enum BlendMode
    {
        Alpha,
        Premultiply,
        Additive,
        Multiply
    }

    private void MakeTerrainTransparent(GameObject gameObjectHit)
    {
        //Try getting all renderers of children and setting them to transparent
        //  - https://docs.unity3d.com/ScriptReference/GameObject.GetComponentsInChildren.html
        //var renderers = gameObjectHit.GetComponentsInChildren<Renderer>();
        //
        //A second option to try is to see if there is a collection of materials on the renderer
        //or more than one renderer that needs to be iterated over
        //
        //A third option if all else fails:
        // - https://www.youtube.com/watch?v=vmLIy62Gsnk
        //Fourth Option
        //  - https://answers.unity.com/questions/1608815/change-surface-type-with-lwrp.html
        //  - https://www.youtube.com/watch?v=nDsTBk6eano

        var renderer = gameObjectHit.GetComponent<Renderer>();
        var material = renderer.material;

        material.SetFloat("_Surface", (float)SurfaceType.Transparent);
        material.SetFloat("_Blend", (float)BlendMode.Alpha);


        bool alphaClip = renderer.material.GetFloat("_AlphaClip") == 1;
        LogDebug($"alphaClip: {alphaClip}");

        material.DisableKeyword("_ALPHATEST_ON");


        SurfaceType surfaceType = (SurfaceType)material.GetFloat("_Surface");
        BlendMode blendMode = (BlendMode)material.GetFloat("_Blend");
        LogDebug($"SurfaceType: {surfaceType}");
        LogDebug($"BlendMode: {blendMode}");

        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        material.SetShaderPassEnabled("ShadowCaster", false);
        /*
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
        material.SetShaderPassEnabled("ShadowCaster", true);
        */
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
