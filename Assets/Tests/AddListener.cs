//Attach this script to a GameObject
//This script creates a UnityEvent that calls a method when a key is pressed
//Note that 'q' exits this application.

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class AddListener : MonoBehaviour
{
    UnityEvent m_MyEvent = new UnityEvent();

    delegate void MyDelegate();

    private MyDelegate myDelegate;
    void Start()
    {
        
        myDelegate = FindObjectOfType<Paterns>().Patern2;
    }

    void Update()
    {
            myDelegate();
    }

    
}