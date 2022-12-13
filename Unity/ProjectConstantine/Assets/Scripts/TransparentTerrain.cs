using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentTerrain : MonoBehaviour
{
    public List<Material> normalMats;
    public List<Material> transparentMats;

    public Transform playerTransform;
    public GameObject lastBuilding;
    public float distCalculate;

    private Ray castRay;
    private RaycastHit castHit;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
