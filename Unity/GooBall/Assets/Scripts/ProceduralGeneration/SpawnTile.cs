using UnityEngine;

public class SpawnTile : MonoBehaviourBase
{
    [Header("SpawnTile")]
    public GameObject[] Tiles;

    private void Start()
    {
        var gameObject = Instantiate(Tiles[Random.Range(0, Tiles.Length)], transform.position, Quaternion.identity);
        gameObject.transform.parent = transform; 
    }
}
