using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour
{
    //TODO: do it better someday
    //original idea is to have multiple power ups and use events to assign the abilities to them
    public UnityEvent OnCollect;
    public float heal;
    
    //for now, we just heal the player, when player ship collects the power up
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //OnCollect.Invoke();
            
            other.GetComponentInParent<PlayerManager>().Heal(heal);
            Destroy(this.gameObject);
        }
    }
}
