using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingCombatText : MonoBehaviourBase
{
    public float DestroyedAfterTime = 1.5f;
    public Vector3 RandomizeIntensity = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        Destroy(gameObject, DestroyedAfterTime);

        transform.localPosition +=
            new Vector3(
                Random.Range(-RandomizeIntensity.x, RandomizeIntensity.y),
                Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
                Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z));
    }

    public void SetDamage(int damage)
    {
        var combatTextObj = GetComponent<TextMeshPro>();
        combatTextObj.text = damage.ToString();
    }
}
