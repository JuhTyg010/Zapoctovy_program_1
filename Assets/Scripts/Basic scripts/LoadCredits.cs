using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;

//using AltoHttp;

public class LoadCredits : MonoBehaviour
{
    [SerializeField] private GameObject credits;
    
    public void LoadAllCredits()
    {
        TextMeshProUGUI text = credits.GetComponent<TextMeshProUGUI>();
        text.fontSize = 5;

        string credit = (Resources.Load("credits") as TextAsset).text;
        text.text = credit;
    }

    public void DeleteCredits()
    {
        TextMeshProUGUI text = credits.GetComponent<TextMeshProUGUI>();
        text.text = "";
    }
}
