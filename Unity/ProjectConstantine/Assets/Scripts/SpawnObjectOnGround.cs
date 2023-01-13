using UnityEngine;

public class SpawnObjectOnGround : MonoBehaviour
{
    private void Start()
    {
        //Set the vertical offset to the object's collider bounds' extends
        var radius = GetComponent<Collider>() == null ? 1f : GetComponent<Collider>().bounds.extents.y;

        //Raycast to find the y-position of the masked collider at the transforms x/z
        RaycastHit raycastHit;
        var ray = new Ray(transform.position + Vector3.up * 100, Vector3.down);
        if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity, LayerMask.GetMask(Constants.Layers.Ground)))
        {
            if(raycastHit.collider != null)
            {
                transform.position = new Vector3(transform.position.x, raycastHit.point.y + radius, transform.position.z);
            }
        }
    }
}