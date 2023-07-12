using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1;
    
    private float _lifeTimer;
    void Start()
    {
        _lifeTimer = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
        _lifeTimer -= Time.deltaTime;
    }
    
    public void SetLifetime(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }
}
