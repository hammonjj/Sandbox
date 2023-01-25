using UnityEngine;
using UnityEngine.Events;

public class MeleeWeaponHit : MonoBehaviourBase
{
    public UnityEvent<Collider> OnMeleeHit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player")
        {
            return;
        }

        LogDebug("Hit Player");
        OnMeleeHit?.Invoke(other);
    }
}
