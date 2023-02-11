using UnityEngine;

public class SpawnPoint : MonoBehaviourBase
{
    public Constants.Enums.EnemyType EnemyType;

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
