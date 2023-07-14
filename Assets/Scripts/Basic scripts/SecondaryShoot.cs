using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryShoot : MonoBehaviour
{
    [SerializeField] private protected float reloadTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private GameObject bulletPrefab;  
    
    private  float _reloadTimer;
    private  GameObject[] _shootPoints;
    
    void Start()
    {
        _reloadTimer = reloadTime;
        _shootPoints = gameObject.ChildrenWithTag("Secondary Shooter");
    }

    // Update is called once per frame
    void Update()
    {
        _reloadTimer -= Time.deltaTime;
        if(_reloadTimer <= 0)
        {
            _reloadTimer = reloadTime;
            CreateBullet();
        }
    }
    
    private void CreateBullet()
    {
        foreach (var shooter in _shootPoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, shooter.transform.position, shooter.transform.rotation);
            Vector2 direction = shooter.transform.up;
            bullet.GetComponent<Bullet>().SetBullet(bulletSpeed, bulletLifeTime, bulletDamage, direction, "Player");
        }
    }
}
