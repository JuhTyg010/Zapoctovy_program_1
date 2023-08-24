using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// BulletGenerator is a script that generates bullets.
// It is attached to the enemy's or player's ship, purpose is to automatically
// generate bullets, based on some time interval.
// it's kind of a bullet by itself
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
    
    //CreateBullet() is a method that creates a bullet and sets its properties
    //It is called when the reload timer is 0
    //it basically creates an prefab object and generate copy of it and set its properties
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
