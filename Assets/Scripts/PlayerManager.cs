using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyInput;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    
    
   
    public int health = 100;
    
    
    private float _speed;
    private int _shipID;
    private Rigidbody2D _myBody;
    private Ship_parameters _myShip;

    void Start()
    {
        _myBody = GetComponent<Rigidbody2D>();
        _myShip = GetComponentInChildren<Ship_parameters>();
        _shipID = _myShip.shipID;
        health = _myShip.health;
        _speed = _myShip.speed;
        
    }
    
    void Update()
    {
        Vector2 direction = GetDirection();
        transform.position += (Vector3) direction * (Time.deltaTime * _speed);
        if (MyInput.IsShooting())
        {
            _myShip.Shoot();
        }

    }
}

