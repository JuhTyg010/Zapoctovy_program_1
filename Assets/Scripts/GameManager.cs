using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score;
    public PlayerManager player;
    public float playerHealth;
    public bool isGameOver;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text gameOverNameText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float scoreIncrease;
    
    [SerializeField] public float leftBorder;
    [SerializeField] public float rightBorder;
    [SerializeField] public float topBorder;
    [SerializeField] public float bottomBorder;
    
    private float _absoluteScore;
    private float _maxHealth;
    private bool _isPaused;
    private List<string> _highScores;
    
    void Start()
    {
        Time.timeScale = 1;
        isGameOver = false;
        _absoluteScore = 0;
        score = 0;
        player = FindObjectOfType<PlayerManager>();
        playerHealth = player.health;
        _maxHealth = playerHealth;
        _isPaused = false;
        //TODO: load from file
        _highScores = LeaderBoard.GetLeaderboard(5);
        ShowTopScores();
    }

    // Update is called once per frame
    void Update()
    {

        #region draw UI

        scoreText.text = score.ToString();
        healthBar.value = playerHealth / _maxHealth;
        ShowTopScores();

        #endregion draw UI

        if (isGameOver)
        {
            GameOver();
        }
        else if (_isPaused)
        {
            OnPause();
        }
        
        else
        {
            if (playerHealth <= 0)
            {
                isGameOver = true;
            }
            else if (MyInput.IsPause())
            {
                _isPaused = !_isPaused;
            }
            playerHealth = player.health;
            _absoluteScore += Time.deltaTime * scoreIncrease;
            score = (int) _absoluteScore;
        }
    }

    public void OnPause() //maybe make function in other place
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true); 
        //TODO: setup some data show maybe some save options 
        
    }

    public void OnResume()
    {
        Debug.Log("Resume");
        Time.timeScale = 1;
        _isPaused = !_isPaused;
        pausePanel.SetActive(false);
    }

    public void OnRestart()
    {
        if (isGameOver)
        {
            LeaderBoard.SetLearderboardEntry(gameOverNameText.text, int.Parse(gameOverScoreText.text));
        }
        Time.timeScale = 0.1f;
        SceneManager.LoadScene($"GameScene");
    }

    public void OnMenu()
    {
        if (isGameOver)
        {
            LeaderBoard.SetLearderboardEntry(gameOverNameText.text, int.Parse(gameOverScoreText.text));
        }
        
        SceneManager.LoadScene($"Menu");
    }

    public void AddScore(float scoreToAdd)
    {
        _absoluteScore += scoreToAdd;
    }

    private void ShowTopScores()
    {
        if (_highScores == null)
        {
            highScoreText.text = "No connection \nto server";
            return;
        }
        int count = _highScores.Count;
        string output = "";
        for (int i = 0; i < count; i++)
        {
            output += $"{_highScores[i]}\n";
        }
        output.TrimEnd();
        highScoreText.text = output;
    }
    
    
    
    void GameOver()
    {
        Time.timeScale = 0; //pause all update based things
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = score.ToString();
        inGamePanel.SetActive(false);
        
        //TODO: make gameOverPanel restart option and push some data
    }


    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(leftBorder, -10), new Vector2(leftBorder, 10));
        Gizmos.DrawLine(new Vector2(rightBorder, -10), new Vector2(rightBorder, 10));
        Gizmos.DrawLine(new Vector2(-10, topBorder), new Vector2(10, topBorder));
        Gizmos.DrawLine(new Vector2(-10, bottomBorder), new Vector2(10, bottomBorder));
    }
    #endif
}
