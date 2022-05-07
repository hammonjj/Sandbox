using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public Rigidbody Rigidbody;

    private static List<Attractor> _attractors; 
    private const float GravitationConstant = 667.4f;

    private void FixedUpdate()
    {
        var layerNumber = LayerMask.NameToLayer("Planet");
        foreach (var attractor in _attractors)
        {
            //We don't attract to ourselves and planets don't attract to each other
            if(attractor == this || 
               gameObject.layer == layerNumber && attractor.gameObject.layer == layerNumber)
            {
                continue;
            }
            
            Attract(attractor);
        }
    }

    private void OnEnable()
    {
        if(_attractors == null)
        {
            _attractors = new List<Attractor>();
        }

        _attractors.Add(this);
    }

    private void OnDisable()
    {
        if (_attractors == null)
        {
            _attractors = new List<Attractor>();
        }

        _attractors.Remove(this);
    }

    private void Attract(Attractor objToAttract)
    {
        var rbToAttract = objToAttract.Rigidbody;
        var direction = Rigidbody.position - rbToAttract.position;
        var distance = direction.magnitude;

        var forceMagnitude = GravitationConstant * (Rigidbody.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        var force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}
