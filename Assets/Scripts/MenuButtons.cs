using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void OnPlayPressed()
    {
        SceneManager.LoadScene($"GameScene");
    }
    
    public void OnExit()
    {
        //TODO: save data
        Application.Quit();
    }
}
