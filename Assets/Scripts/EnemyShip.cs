using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Ship
{
    
    public float difficulty;
    public float spawnTime;

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
    
    
    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GameObject score = Instantiate(scorePrefab, transform.position, Quaternion.identity);
            score.GetComponent<TextMesh>().text = "+ " + (difficulty / 2).ToString();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        _manager.ShipDestroyed(difficulty);
    }
}
