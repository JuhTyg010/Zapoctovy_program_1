using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int shipID;
    public float speed;
    public float health;

    public float reloadTime;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    public float bulletDamage;
    [SerializeField] private GameObject bulletPrefab;
    
    
    //using protected so that the child classes can access the variables
    private protected float ReloadTimer;
    private protected GameObject[] ShootPoints;

    void Start()
    {
        ReloadTimer = reloadTime;
        ShootPoints = gameObject.ChildrenWithTag("Shooter");

    }

    //Shoot is different for the player and the enemy
    //but the reloading is the same
    //so we reload the weapon and then call the shoot method of the child class
    protected void Shoot(string target)
    {
        if (ReloadTimer <= 0)
        {
            ReloadTimer = reloadTime;
            CreateBullet(target);
        }
    }

    //CreateBullet creates a copy of bullet for each shoot point,
    //and sets the bullet's variables based on ship's variables
    void CreateBullet(string target)
    {
        foreach (var shooter in ShootPoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, shooter.transform.position, shooter.transform.rotation);
            Vector2 direction = shooter.transform.up;
            bullet.GetComponent<Bullet>().SetBullet(bulletSpeed, bulletLifeTime, bulletDamage, direction, target);
        }
    }
    
    //TakeDamage() is a method that is called when the ship is hit by a bullet
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    //Getters
    public float GetReloadTimer()
    {
        return ReloadTimer;
    }
    public float GetReloadTime()
    {
        return reloadTime;
    }
    
}
