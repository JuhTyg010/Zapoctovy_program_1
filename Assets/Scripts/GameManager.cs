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
    
    [SerializeField] private Text scoreText;
    [SerializeField] private Text healthText;
    [SerializeField] private float scoreIncrease;
    
    private float _absoluteScore;
    
    void Start()
    {
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
    }
    
    void GameOver()
    {
        //stop the game and show the game over screen
    }
}
