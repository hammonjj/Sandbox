using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Camera MainCamera;
    private GameObject _player;
    private List<GameObject> _cargoContainersLeft;

    private void Start ()
	{
	    _cargoContainersLeft = FindGameObjectsInLayer(LayerMask.NameToLayer("Cargo"));
	    _player = GameObject.FindGameObjectsWithTag("Player").First();

        //Pause game until tap
    }
	
	private void Update ()
    {
	    if(_cargoContainersLeft.Count < 1)
	    {
            //Next Level
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            Resources.UnloadUnusedAssets();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        //Verify we are in view
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(MainCamera);
        if(!GeometryUtility.TestPlanesAABB(planes, _player.GetComponent<Collider>().bounds))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
        }
    }

    public void ContainerCaptured()
    {
        _cargoContainersLeft.RemoveAt(0);
    }

    private static List<GameObject> FindGameObjectsInLayer(int layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = goArray.Where(t => t.layer == layer).ToList();
        return goList.Count == 0 ? null : goList;
    }
}
