using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int shipID;
    public float speed;
    public float health;

    [SerializeField] private protected float reloadTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    [SerializeField] private float bulletDamage;
    [SerializeField] private GameObject bulletPrefab;

    private protected float ReloadTimer;
    private protected GameObject[] ShootPoints;

    void Start()
    {
        ReloadTimer = reloadTime;
        ShootPoints = gameObject.ChildrenWithTag("Shooter");

    }

    protected void Shoot(string target)
    {
        if (ReloadTimer <= 0)
        {
            ReloadTimer = reloadTime;
            CreateBullet(target);
        }
    }

    void CreateBullet(string target)
    {
        foreach (var shooter in ShootPoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, shooter.transform.position, shooter.transform.rotation);
            Vector2 direction = shooter.transform.up;
            bullet.GetComponent<Bullet>().SetBullet(bulletSpeed, bulletLifeTime, bulletDamage, direction, target);
        }
    }
    
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public float GetReloadTimer()
    {
        return ReloadTimer;
    }
    public float GetReloadTime()
    {
        return reloadTime;
    }
    
}
