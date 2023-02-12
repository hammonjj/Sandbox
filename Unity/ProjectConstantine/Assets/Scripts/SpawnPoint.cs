using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviourBase
{
    public Constants.Enums.EnemyType EnemyType;
    public float SpawnTimeMin = 0.5f;
    public float SpawnTimeMax = 1.0f;

    public void SpawnEnemy(GameObject prefab)
    {
        StartCoroutine(SpawnEnemy(prefab, Random.Range(SpawnTimeMin, SpawnTimeMax)));
    }

    private IEnumerator SpawnEnemy(GameObject prefab, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //SpawnVFX
        Instantiate(prefab, gameObject.transform);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
