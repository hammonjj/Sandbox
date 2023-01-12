using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCombatText : MonoBehaviourBase
{
    public float DestroyedAfterTime = 3f;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomizeIntensity = new Vector3(0.5f, 0, 0);

    private void Awake()
    {
        Destroy(gameObject, DestroyedAfterTime);

        transform.localPosition +=
            new Vector3(
                Random.Range(-RandomizeIntensity.x, RandomizeIntensity.y),
                Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
                Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }
}
