using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed;
    private float _lifeTime;
    private float _damage;
    private Vector2 _direction;
    
    private float _lifeTimer;
    public void SetBullet(float speed, float lifeTime, float damage, Vector2 direction)
    {
        this._speed = speed;
        this._lifeTime = lifeTime;
        this._damage = damage;
        this._direction = direction;
    }
    void Start()
    {
        _lifeTimer = _lifeTime;
    }

    void Update()
    {
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
        transform.position += (Vector3) _direction * (Time.deltaTime * _speed);
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<EnemyShip>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
