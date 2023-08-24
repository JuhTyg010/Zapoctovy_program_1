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
    
    // LoadAllCredits is called when the user clicks the "Credits" button.
    // It loads the credits from the credits.txt file in the Resources folder.
    public void LoadAllCredits()
    {
        TextMeshProUGUI text = credits.GetComponent<TextMeshProUGUI>();
        text.fontSize = 5;

        string credit = (Resources.Load("credits") as TextAsset).text;
        text.text = credit;
    }

    // DeleteCredits is called when the user clicks the "Back" button.
    public void DeleteCredits()
    {
        TextMeshProUGUI text = credits.GetComponent<TextMeshProUGUI>();
        text.text = "";
    }
}
