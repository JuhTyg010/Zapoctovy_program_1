using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class LoadCredits : MonoBehaviour
{
    [SerializeField] private GameObject credits;
    
    public void LoadAllCredits()
    {
        TextMeshProUGUI text = credits.GetComponent<TextMeshProUGUI>();
        string credit = File.ReadAllText("./Assets/Other/Credits.txt");
        text.text = credit;
    }
    public void DeleteCredits()
    {
        TextMeshProUGUI text = credits.GetComponent<TextMeshProUGUI>();
        text.text = "";
    }
}
