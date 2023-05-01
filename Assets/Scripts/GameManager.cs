using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score;
    public int playerHealth;
    public bool isGameOver;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text healthText;
    [SerializeField] private float scoreIncrease;
    
    [SerializeField] private float leftBorder;
    [SerializeField] private float rightBorder;
    [SerializeField] private float topBorder;
    [SerializeField] private float bottomBorder;
    
    private float _absoluteScore;
    
    void Start()
    {
        Time.timeScale = 1;
        isGameOver = false;
        _absoluteScore = 0;
        score = 0;
        playerHealth = FindObjectOfType<PlayerManager>().health;
    }

    // Update is called once per frame
    void Update()
    {

        #region draw UI

        scoreText.text = $"Score: {score}";
        healthText.text = $"Health: {playerHealth}";   

        #endregion draw UI

        if (isGameOver)
        {
            GameOver();
        }
        else
        {
            if (playerHealth <= 0)
            {
                isGameOver = true;
            }
            
            _absoluteScore += Time.deltaTime * scoreIncrease;
            score = (int) _absoluteScore;
        }
        _absoluteScore += Time.deltaTime * scoreIncrease;
    }

    void onPause() //maybe make function in other place
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        inGamePanel.SetActive(false);
        //TODO: setup some data show maybe some save options 
        
    }

    void onResume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        inGamePanel.SetActive(true);
    }
    
    void GameOver()
    {
        
        Time.timeScale = 0; //pause all update based things
        gameOverPanel.SetActive(true);
        inGamePanel.SetActive(false);
        //TODO: make gameOverPanel restart option and push some data
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(leftBorder, -10), new Vector2(leftBorder, 10));
        Gizmos.DrawLine(new Vector2(rightBorder, -10), new Vector2(rightBorder, 10));
        Gizmos.DrawLine(new Vector2(-10, topBorder), new Vector2(10, topBorder));
        Gizmos.DrawLine(new Vector2(-10, bottomBorder), new Vector2(10, bottomBorder));
    }
}
