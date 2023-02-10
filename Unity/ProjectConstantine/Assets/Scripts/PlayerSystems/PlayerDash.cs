using Constantine;
using EditorExtensions;
using System.Collections;
using UnityEngine;

public class PlayerDash : MonoBehaviourBase
{
    [Header("Ability Settings")]
    [Tooltip("Dash speed of the character in m/s")]
    public float DashSpeed = 40f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to dash again. Set to 0f to instantly dash again")]
    public float DashTimeout = 1f;

    [Tooltip("The amount of time that the character will dash for")]
    public float DashTime = 0.1f;

    [DisplayWithoutEdit()]
    public bool IsDashing = false;

    [Header("Animation Settings")]
    public float MeshRefreshRate = 0.05f;
    public Material ShaderMaterial;
    public float MeshDestroyDelay = 0.5f;
    public float ShaderVariableRate = 0.1f;
    public float ShaderVariableRefreshRate = 0.05f;

    //private bool _isTrailActive = false;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;

    //Internal Controls
    private bool _canDash = true;
    private float _totaldashTime;
    private float _dashTimeoutCurrent;

    private void Start()
    {
        EventManager.GetInstance().onPlayerDash += OnDash;
    }

    private void Update()
    {
        _dashTimeoutCurrent -= Time.deltaTime;

        if(IsDashing)
        {
            _totaldashTime += Time.deltaTime;
        }

        _canDash = _dashTimeoutCurrent <= 0.0f;
        if(IsDashing && _totaldashTime >= DashTime)
        {
            IsDashing = false;
            LogDebug("Dash Ended");
        }
    }

    private void OnDash()
    {
        if(!_canDash)
        {
            return;
        }

        LogDebug("OnDash Called");

        _canDash = false;
        IsDashing = true;
        _totaldashTime = 0f;
        _dashTimeoutCurrent = DashTimeout;

        StartCoroutine(ActivateTrail(DashTime));
    }

    IEnumerator ActivateTrail(float activeTime)
    {
        while(activeTime > 0)
        {
            activeTime -= MeshRefreshRate;

            if(_skinnedMeshRenderers == null || _skinnedMeshRenderers.Length == 0)
            {
                _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                //LogDebug($"_skinnedMeshRenderers.Count: {_skinnedMeshRenderers.Length}");
            }

            for(var i = 0; i < _skinnedMeshRenderers.Length; i++)
            {
                //var subMeshCount = _skinnedMeshRenderers[i].sharedMesh.subMeshCount;
                //LogDebug($"subMeshCount: {subMeshCount}");
                var gObj = new GameObject();
                gObj.name = "PlayerMeshTrail";
                gObj.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);

                var meshRenderer = gObj.AddComponent<MeshRenderer>();
                var meshFilter = gObj.AddComponent<MeshFilter>();

                var mesh = new Mesh();
                _skinnedMeshRenderers[i].BakeMesh(mesh);
                //mesh = _skinnedMeshRenderers[i].sharedMesh;
                //Physics.BakeMesh(mesh.GetInstanceID(), false);

                meshFilter.mesh = mesh;
                meshRenderer.material = ShaderMaterial;
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                StartCoroutine(AnimateMaterialFloat(meshRenderer.material));
                Destroy(gObj, MeshDestroyDelay);
            }

            yield return new WaitForSeconds(MeshRefreshRate);
        }
    }

    IEnumerator AnimateMaterialFloat(Material mat)
    {
        var valueToAnimate = mat.GetFloat("_Alpha");
        while(valueToAnimate > 0)
        {
            valueToAnimate -= ShaderVariableRate;
            mat.SetFloat("_Alpha", valueToAnimate);
            yield return new WaitForSeconds(ShaderVariableRefreshRate);
        }
    }
}
