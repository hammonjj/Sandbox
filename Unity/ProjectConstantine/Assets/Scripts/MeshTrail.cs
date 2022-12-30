using System.Collections;
using UnityEngine;

public class MeshTrail : MonoBehaviourBase
{
    public float ActiveTime = 2.0f;
    public float MeshRefreshRate = 0.1f;
    public Material ShaderMaterial;
    public float MeshDestroyDelay = 3.0f;
    public float ShaderVariableRate = 0.1f;
    public float ShaderVaraibleRefreshRate = 0.05f;

    private bool _isTrailActive = false;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;

    void Update()
    {
        //Input -> Dash
        if(!_isTrailActive)
        {
            LogDebug("Activating Trail");
            _isTrailActive = true;
            StartCoroutine(ActivateTrail(ActiveTime));
        }
    }

    IEnumerator ActivateTrail(float activeTime)
    {
        while(activeTime > 0)
        {
            activeTime -= MeshRefreshRate;

            if(_skinnedMeshRenderers == null || _skinnedMeshRenderers.Length == 0)
            {
                _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                LogDebug($"_skinnedMeshRenderers.Count: {_skinnedMeshRenderers.Length}");
            }

            for(var i = 0; i < _skinnedMeshRenderers.Length; i++)
            {
                //var subMeshCount = _skinnedMeshRenderers[i].sharedMesh.subMeshCount;

                var gObj = new GameObject();
                gObj.name = "PlayerMeshTrail";
                gObj.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);

                var meshRenderer = gObj.AddComponent<MeshRenderer>();
                var meshFilter = gObj.AddComponent<MeshFilter>();

                var mesh = new Mesh();
                _skinnedMeshRenderers[i].BakeMesh(mesh);

                meshFilter.mesh = mesh;
                meshRenderer.material = ShaderMaterial;
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                StartCoroutine(AnimateMaterialFloat(meshRenderer.material, 0, ShaderVariableRate, ShaderVaraibleRefreshRate));
                Destroy(gObj, MeshDestroyDelay);
            }

            yield return new WaitForSeconds(MeshRefreshRate);
        }

        _isTrailActive = false;
    }

    IEnumerator AnimateMaterialFloat(Material mat, float endGoal, float rate, float refreshRate)
    {
        var valueToAnimate = mat.GetFloat("_Alpha");

        while(valueToAnimate > endGoal)
        {
            valueToAnimate -= rate;
            mat.SetFloat("_Alpha", valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
