using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ship_parameters : MonoBehaviour
{
    public int shipID;
    public int health;
    
    public float speed;
    [SerializeField] private float reloadTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private GameObject bulletPrefab;
    
    
    private float _reloadTimer;
    private GameObject[] _shootPoints;
    bool CanShoot()
    {
        return _reloadTimer <= 0;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void Shoot()
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
            bullet.GetComponent<Bullet>().SetBullet(bulletSpeed, bulletLifeTime, bulletDamage, direction);
        }
        
    }




    // Start is called before the first frame update
    void Start()
    {
        _shootPoints = gameObject.ChildrenWithTag("Shooter");
    }

    // Update is called once per frame
    void Update()
    {
        _reloadTimer -= Time.deltaTime;
    }
}
