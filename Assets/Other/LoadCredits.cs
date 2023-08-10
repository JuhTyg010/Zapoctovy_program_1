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
        
        //Try to load from net
        string url = "drive.google.com/u/O/uaid=1894uoEIK6HMOMel9UvTNQ-9BL-LjuR7&export=download";

       /* HttpDownloader downloader = new HttpDownloader(url,Application.persistentDataPath + "/Credits.txt");
        downloader.DownloadCompleted += _OnDownloadCompleted;
        downloader.Start();*/
        
        /*var textFromFile = (new WebClient()).DownloadString(url);
        text.text = textFromFile;*/
        
        /*string credit = File.ReadAllText("./Assets/Other/Credits.txt");
        text.text = credit;*/
    }

    private void _OnDownloadCompleted(object sender, EventArgs e)
    {
        TextMeshProUGUI text = credits.GetComponent<TextMeshProUGUI>();
        string credit = File.ReadAllText(Application.persistentDataPath + "/Credits.txt");
        text.text = credit;
    }

    public void DeleteCredits()
    {
        TextMeshProUGUI text = credits.GetComponent<TextMeshProUGUI>();
        text.text = "";
    }
}
