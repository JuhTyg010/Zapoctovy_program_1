using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyShip is a class that inherits from Ship
public class EnemyShip : Ship
{
    
    public float difficulty;
    public float spawnTime;

    //this delegate is used to change the movement of the ship by the manager
    public delegate void Move(float speed, Transform transform);
    
    [SerializeField] private float changeDirectionTime;
    [SerializeField] private Vector2 playerOffset;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject scorePrefab;
    
    
    private float _changeDirectionTimer;
    public Move _move;
    private EnemyManager _manager;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        ShootPoints = gameObject.ChildrenWithTag("Shooter");
        _manager = GetComponentInParent<EnemyManager>();
        _manager.NewDirection(this, playerOffset);
        _changeDirectionTimer = changeDirectionTime;
        ReloadTimer = reloadTime;
    }

    // Update is called once per frame
    // if possible, the ship will shoot at the player
    // if its time the ship will change direction, by calling the manager
    void Update()
    {
        ReloadTimer -= Time.deltaTime;
        _changeDirectionTimer -= Time.deltaTime;
        Shoot("Player");
        _move(speed, transform);
        if (_changeDirectionTimer < 0)
        {
            _manager.NewDirection(this, playerOffset);
            _changeDirectionTimer = changeDirectionTime;
        }
    }
    
    //TakeDamage() is a method that is called when the ship is hit by a bullet
    //it is called by the specific bullet that hit the ship
    //it reduces the health of the ship and if the health is 0 or less, it destroys the ship
    //and creates an explosion animation and shows score
    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameObject score = Instantiate(scorePrefab, transform.position, Quaternion.identity);
            score.GetComponent<TextMesh>().text = "+ " + Mathf.RoundToInt(difficulty / 2).ToString();
            Destroy(gameObject);
        }
    }

    //when the ship is destroyed, it calls the manager to update the difficulty and score
    private void OnDestroy()
    {
        _manager.ShipDestroyed(difficulty, transform.position);
    }
}
