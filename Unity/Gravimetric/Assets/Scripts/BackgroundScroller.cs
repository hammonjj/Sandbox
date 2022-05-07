using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var mr = GetComponent<MeshRenderer>();
	    var material = mr.material;
	    var offset = material.mainTextureOffset;
	    offset.x += Time.deltaTime / 10f;
	    material.mainTextureOffset = offset;
	}
}
