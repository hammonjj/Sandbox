using System.Linq;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    private LevelManager _levelManager;
    void Start()
    {
        _levelManager = GameObject.FindGameObjectsWithTag("GameController").First().GetComponent<LevelManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("DestroyOnContact:" + col.gameObject.name);
        _levelManager.ContainerCaptured();
        Destroy(gameObject);
    }
}
