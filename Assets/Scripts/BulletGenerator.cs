using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    
    [SerializeField] private protected float reloadTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount;
    
    private float _reloadTimer;
    void Start()
    {
        _reloadTimer = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        _reloadTimer -= Time.deltaTime;
        if(_reloadTimer <= 0)
        {
            _reloadTimer = reloadTime;
            bulletCount--;
            CreateBullet();
        }
    }
    
    private void CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Vector2 direction = transform.up;
        bullet.GetComponent<Bullet>().SetBullet(bulletSpeed, bulletLifeTime, bulletDamage, direction, "Player");
        if (bulletCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
