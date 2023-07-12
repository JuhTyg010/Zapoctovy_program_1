using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public int shipID;
    public float speed;
    public float difficulty;
    public float spawnTime;

    public delegate void Move(float speed, Transform transform);
    
    [SerializeField] private float health;
    [SerializeField] private float reloadTime;
    [SerializeField] private float changeDirectionTime;
    [SerializeField] private Vector2 playerOffset;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject scorePrefab;
    
    
    private float _reloadTimer;
    private float _changeDirectionTimer;
    private GameObject[] _shootPoints;
    public Move _move;
    private EnemyManager _manager;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _shootPoints = gameObject.ChildrenWithTag("Shooter");
        _manager = GetComponentInParent<EnemyManager>();
        _manager.NewDirection(this, playerOffset);
        _changeDirectionTimer = changeDirectionTime;
        _reloadTimer = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        _move(speed, transform);
        _changeDirectionTimer -= Time.deltaTime;
        if (_changeDirectionTimer < 0)
        {
            _manager.NewDirection(this, playerOffset);
            _changeDirectionTimer = changeDirectionTime;
        }
    }
    
    void Shoot()
    {
        if (_reloadTimer <= 0)
        {
            _reloadTimer = reloadTime;
            CreateBullet();
        }
        _reloadTimer -= Time.deltaTime;
        
    }
    
    void CreateBullet()
    {
        foreach (var shooter in _shootPoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, shooter.transform.position, shooter.transform.rotation);
            Vector2 direction = shooter.transform.up;
            bullet.GetComponent<Bullet>().SetBullet(bulletSpeed, bulletLifeTime, bulletDamage, direction, "Player");
        }
        
    }
    
    public void TakeDamage(float damage)
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
