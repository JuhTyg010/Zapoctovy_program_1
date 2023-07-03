using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public int shipID;
    public float speed;

    public delegate void Move(float speed, Transform transform);
    
    [SerializeField] private float health;
    [SerializeField] private float reloadTime;
    [SerializeField] private float changeDirectionTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private GameObject bulletPrefab;
    
    
    private float _reloadTimer;
    private float _changeDirectionTimer;
    private GameObject[] _shootPoints;
    public Move _move;
    private Paterns _paterns;
    private EnemyManager _manager;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _shootPoints = gameObject.ChildrenWithTag("Shooter");
        _paterns = GetComponentInParent<Paterns>();
        _manager = GetComponentInParent<EnemyManager>();
        _manager.NewDirection(this);
        _changeDirectionTimer = changeDirectionTime;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        _move(speed, transform);
        _changeDirectionTimer -= Time.deltaTime;
        if (_changeDirectionTimer < 0)
        {
            _manager.NewDirection(this);
            _changeDirectionTimer = changeDirectionTime;
        }
    }
    
    void Shoot()
    {
        if (CanShoot())
        {
            _reloadTimer = reloadTime;
            CreateBullet();
        }
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
    
    
    bool CanShoot()
    {
        return _reloadTimer <= 0;
    }
    
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    { 
        _manager.ShipDestroyed(shipID);
    }
}
