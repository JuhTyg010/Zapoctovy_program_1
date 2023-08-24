using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 axes;
    
    private Transform t;    //used to cache transform to avoid multiple calls
    void Start()
    {
        t = transform;
    }

    void Update()
    {
        Quaternion rotation = t.rotation;
        rotation.x += (axes.x * Time.deltaTime);
        rotation.y += (axes.y * Time.deltaTime);
        rotation.z += (axes.z * Time.deltaTime);
    }
}
