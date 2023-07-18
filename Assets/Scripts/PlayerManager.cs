using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MyInput;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    
    public float health = 100;
    
    [SerializeField] private Slider reloadSlider;
    
    private float _speed;
    private int _shipID;
    private Rigidbody2D _myBody;
    private float _reloadTime;
    private PlayerShip _myShip;
    private Vector2 _verticalBorder;
    private Vector2 _horizontalBorder;

    void Start()
    {
        _myBody = GetComponent<Rigidbody2D>();
        _myShip = GetComponentInChildren<PlayerShip>();
        _shipID = _myShip.shipID;
        health = _myShip.health;
        _speed = _myShip.speed;
        GameManager gm = FindObjectOfType<GameManager>();
        _verticalBorder = new Vector2(gm.leftBorder, gm.rightBorder);
        _horizontalBorder = new Vector2(gm.bottomBorder, gm.topBorder);
        _reloadTime = _myShip.GetReloadTime();
        reloadSlider.maxValue = _reloadTime;

        
    }
    
    void Update()
    {
        Vector2 direction = GetDirection();
        Move(direction);
        if (MyInput.IsShooting())
        {
            _myShip.Shooting("Enemy");
        }
        health = _myShip.health;
        reloadSlider.value = _reloadTime - _myShip.GetReloadTimer() / _reloadTime;
        

    }

    void Move(Vector2 direction)
    {
        Vector3 wantedPosition = transform.position + (Vector3) direction * (Time.deltaTime * _speed);
        transform.position = new Vector3(Mathf.Clamp(wantedPosition.x, _verticalBorder.x, _verticalBorder.y),
            Mathf.Clamp(wantedPosition.y, _horizontalBorder.x, _horizontalBorder.y), 0);
    }
}

