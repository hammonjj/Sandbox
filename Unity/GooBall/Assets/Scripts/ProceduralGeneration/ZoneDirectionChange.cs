using UnityEngine;

public class ZoneDirectionChange : MonoBehaviourBase
{
    [Header("ZoneDirectionChange")]
    public ZoneGenerator.Direction Direction;

    private ZoneGenerator zoneGenerator;
    private void Start()
    {
        
    }

    private void Update()
    {
        var raycastCollider = Physics2D.RaycastAll(transform.position, Vector2.right, 1.0f);
        for(int i = 0; i < raycastCollider.Length; ++i)
        {
            if(raycastCollider[i].collider != null && raycastCollider[i].collider.gameObject.CompareTag("ZoneGenerator"))
            {
                LogDebug("Hit ZoneGenerator");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("ZoneGenerator")) 
        {
            LogDebug("Changing Direction");
            collision.gameObject.GetComponent<ZoneGenerator>().MapFlowDirection = Direction;
        }
    }
}
