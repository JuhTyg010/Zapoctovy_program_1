using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : Ship
{

    
    // Update is called once per frame
    void Update()
    {
        ReloadTimer -= Time.deltaTime;
    }

    public void Shooting(string target)
    {
        Shoot(target);
    }
}
