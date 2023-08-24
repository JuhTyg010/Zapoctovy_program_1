using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MyInput;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerManager : MonoBehaviour
{
    private float _maxHealth = 100;
    public float health;
    
    [SerializeField] private Slider reloadSlider;
    [SerializeField] private List<GameObject> ships;
    
    private float _speed;
    private int _shipID;
    private Rigidbody2D _myBody;
    private float _reloadTime;
    private PlayerShip _myShip;
    private Vector2 _verticalBorder;
    private Vector2 _horizontalBorder;

    void Start()
    {
        //we load the selected ship id from menu via the saved file 
        _shipID = SaveSystem.LoadShipId();
        //we instantiate the selected ship
        foreach (var ship in ships)
        {
            if (ship.GetComponent<PlayerShip>().shipID == _shipID)
            {
                Instantiate(ship, Vector3.zero, Quaternion.identity,transform);
                break;
            }
        }
        
        _myBody = GetComponent<Rigidbody2D>();
        _myShip = GetComponentInChildren<PlayerShip>();
        _shipID = _myShip.shipID;
        _maxHealth = _myShip.health;
        health = _maxHealth;
        _speed = _myShip.speed;
        GameManager gm = FindObjectOfType<GameManager>();
        _verticalBorder = new Vector2(gm.leftBorder, gm.rightBorder);
        _horizontalBorder = new Vector2(gm.bottomBorder, gm.topBorder);
        _reloadTime = _myShip.GetReloadTime();
        reloadSlider.maxValue = _reloadTime;
        
    }
    //On update we get the direction from the input and move the ship
    //also we check if the player is shooting and if so we call the shooting method from the ship
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

    //this method is to simulate the movement of the ship, to chosen direction
    void Move(Vector2 direction)
    {
        Vector3 wantedPosition = transform.position + (Vector3) direction * (Time.deltaTime * _speed);
        transform.position = new Vector3(Mathf.Clamp(wantedPosition.x, _verticalBorder.x, _verticalBorder.y),
            Mathf.Clamp(wantedPosition.y, _horizontalBorder.x, _horizontalBorder.y), 0);
    }
    
    //this is for powerup use to restore the health of the ship 
    public void Heal(float heal)
    {
        _myShip.health += heal;
        if (_myShip.health > _maxHealth)
        {
            _myShip.health = _maxHealth;
        }
    }
}

