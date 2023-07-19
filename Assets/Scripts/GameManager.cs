using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private Text highScoreText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private float scoreIncrease;
    
    [SerializeField] public float leftBorder;
    [SerializeField] public float rightBorder;
    [SerializeField] public float topBorder;
    [SerializeField] public float bottomBorder;
    
    private float _absoluteScore;
    private float _maxHealth;
    private bool _isPaused;
    private List<string> _highScores = new List<string>();
    
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
        _highScores.Add("High Score: " + PlayerPrefs.GetInt("HighScore"));
        _highScores.Add("Second High Score: " + PlayerPrefs.GetInt("SecondHighScore"));
        _highScores.Add("Third High Score: " + PlayerPrefs.GetInt("ThirdHighScore"));
        _highScores.Add("Fourth High Score: " + PlayerPrefs.GetInt("FourthHighScore"));
        _highScores.Add("Fifth High Score: " + PlayerPrefs.GetInt("FifthHighScore"));
        ShowTopScores();
    }

    // Update is called once per frame
    void Update()
    {

        #region draw UI

        scoreText.text = score.ToString();
        healthBar.value = playerHealth / _maxHealth;

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
        inGamePanel.SetActive(false);
        //TODO: setup some data show maybe some save options 
        
    }

    public void OnResume()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        inGamePanel.SetActive(true);
    }

    public void AddScore(float scoreToAdd)
    {
        _absoluteScore += scoreToAdd;
    }

    private void ShowTopScores()
    {
        int count = _highScores.Count;
        string output = "";
        for (int i = 0; i < count; i++)
        {
            output += _highScores[i] + "\n";
        }

        output.Remove(output.Length - 1);
        highScoreText.text = output;
    }
    
    
    
    void GameOver()
    {
        Time.timeScale = 0; //pause all update based things
        gameOverPanel.SetActive(true);
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
