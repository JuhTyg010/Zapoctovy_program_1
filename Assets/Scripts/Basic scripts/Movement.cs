using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 direction;
    
    void Update()
    {
        transform.position += (Vector3) direction * (Time.deltaTime * speed);
    }
    
    public void SetMovement(float speed, Vector2 direction)
    {
        this.speed = speed;
        this.direction = direction;
    }
}
