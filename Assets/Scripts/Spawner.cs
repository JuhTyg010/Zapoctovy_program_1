using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool isFree;
    
    private float _spawnTimer;

    void Start()
    {
        isFree = true;
    }

    void Update()
    {
        if (!isFree)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0)
            {
                isFree = true;
            }
        }
    }

    public void Spawn(float spawnTime)
    {
        _spawnTimer = spawnTime;
        isFree = false;
    }
}
