using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyInput;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    
    public float health = 100;
    
    private float _speed;
    private int _shipID;
    private Rigidbody2D _myBody;
    private Ship_parameters _myShip;
    private Vector2 _verticalBorder;
    private Vector2 _horizontalBorder;

    void Start()
    {
        _myBody = GetComponent<Rigidbody2D>();
        _myShip = GetComponentInChildren<Ship_parameters>();
        _shipID = _myShip.shipID;
        health = _myShip.health;
        _speed = _myShip.speed;
        GameManager gm = FindObjectOfType<GameManager>();
        _verticalBorder = new Vector2(gm.leftBorder, gm.rightBorder);
        _horizontalBorder = new Vector2(gm.bottomBorder, gm.topBorder);
        
    }
    
    void Update()
    {
        Vector2 direction = GetDirection();
        Move(direction);
        if (MyInput.IsShooting())
        {
            _myShip.Shoot(0); //better to use _speed instead of direction.x but it's still not perfect
        }

    }

    void Move(Vector2 direction)
    {
        Vector3 wantedPosition = transform.position + (Vector3) direction * (Time.deltaTime * _speed);
        transform.position = new Vector3(Mathf.Clamp(wantedPosition.x, _verticalBorder.x, _verticalBorder.y),
            Mathf.Clamp(wantedPosition.y, _horizontalBorder.x, _horizontalBorder.y), 0);
    }
}

