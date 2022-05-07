using System.Collections;
using UnityEngine;

public class TrajectoryPredictor : MonoBehaviour
{
    public int PointsToCalculate = 5;
    public GameObject TrajectoryMarker;
    
    private bool _playerAlive = true;
    private const float GravitationConstant = 667.4f;
    private GameObject _trackedObject;

    private void Start() 
	{
        _trackedObject = GameObject.FindGameObjectWithTag("Player");
	    StartCoroutine(_TrackObject());
    }

    private IEnumerator _TrackObject()
    {
        yield return new WaitForSeconds(1.0f);

        while(_playerAlive)
        {
            _playerAlive = true;
            var totalForce = new Vector3();
            var attractors = FindObjectsOfType<Attractor>();
            foreach(var attractor in attractors)
            {
                if(attractor.gameObject.CompareTag("Player") == _trackedObject.CompareTag("Player"))
                {
                    continue;
                }
                
                totalForce += Attract(attractor);
            }

            Debug.Log("Total Force: " + totalForce);

            //v_final = (Force * time)/mass
            Vector3 velocity = (totalForce / _trackedObject.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime;
            //var velocity = (totalForce * 2.0f) / _trackedObject.GetComponent<Rigidbody>().mass;

            Debug.Log("Future Velocity: " + velocity);

            //distance = velocity * t
            var distance = velocity * 2.0f;
            var endingPosition = _trackedObject.GetComponent<Rigidbody>().position + distance;
            Debug.Log("Ending Position: " + endingPosition);
            Instantiate(TrajectoryMarker);

            yield return new WaitForSeconds(2.0f);
        }
    }

    private Vector3 Attract(Attractor objAttracting)
    {
        var rbToAttract = _trackedObject.GetComponent<Rigidbody>();
        
        var direction = objAttracting.Rigidbody.position - rbToAttract.position;
        var distance = direction.magnitude;

        var forceMagnitude = GravitationConstant * (objAttracting.Rigidbody.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        return direction.normalized * forceMagnitude;
    }
}
