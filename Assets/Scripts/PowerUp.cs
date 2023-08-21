using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour
{
    //TODO: do it better someday
    public UnityEvent OnCollect;
    public float heal;
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
