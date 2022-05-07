using UnityEngine;

public class RingCollider : MonoBehaviour
{
    private bool _containsPlayer;

    public bool ContainsPlayer()
    {
        return _containsPlayer;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            _containsPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            _containsPlayer = false;
        }
    }
}
