using UnityEngine;

public class AutoSavePoint : MonoBehaviourBase
{
    [Header("Save")]
    public GameObject RespawnPoint;
    public GameManager GameManager;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            GameManager.SaveGame(RespawnPoint.transform.position);
        }
    }
}
