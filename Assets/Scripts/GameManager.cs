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
    [SerializeField] private InputField gameOverNameText;
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
    
    //Uses Awake() instead of Start() because it is called also on restart
    void Awake()
    {
        Time.timeScale = 1;
        isGameOver = false;
        _absoluteScore = 0;
        score = 0;
        player = FindObjectOfType<PlayerManager>();
        playerHealth = player.health;
        _maxHealth = playerHealth;
        _isPaused = false;
        
        //constant for LeaderBoard is set brutally here because I don't want to change it ever!!!
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
                gameOverNameText.text = SaveSystem.LoadName(); 

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
    }

    //From outside you can call this to resume game
    //TimeScale works like this: 1 - normal time, 0 - no time, 0.5 - half time
    public void OnResume()
    {
        Time.timeScale = 1;
        _isPaused = !_isPaused;
        pausePanel.SetActive(false);
    }

    // I check if we are restarting cause we died or just for fun, if death was involved
    //I save tne name to storage for comfort in future and send score data with the name to server
    public void OnRestart()
    {
        if (isGameOver)
        {
            SaveSystem.SaveName(gameOverNameText.text);
            LeaderBoard.SetLearderboardEntry(gameOverNameText.text, int.Parse(gameOverScoreText.text));
        }
        //In timescale = 0 I don't know if I can load scene, so I set it to 0.1
        Time.timeScale = 0.1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Same as with restart we check the reason of menu call, if death we save same way as in restart
    //then we load menu scene
    public void OnMenu()
    {
        if (isGameOver)
        {
            SaveSystem.SaveName(gameOverNameText.text);
            LeaderBoard.SetLearderboardEntry(gameOverNameText.text, int.Parse(gameOverScoreText.text));
        }
        
        SceneManager.LoadScene($"Menu");
    }

    public void AddScore(float scoreToAdd)
    {
        _absoluteScore += scoreToAdd;
    }

    //We call this method for showing top scores on UI
    //But we need to format it to single string
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
    
    //is called when player dies
    //we set timescale to 0 to pause all update based things
    //we set game over panel active and in game panel inactive
    void GameOver()
    {
        Time.timeScale = 0; //pause all update based things
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = score.ToString();
        inGamePanel.SetActive(false);
        
    }


    #if UNITY_EDITOR
    //This is for debug purposes, it draws borders of the game
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
