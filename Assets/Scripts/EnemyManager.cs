using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fleet;
    [SerializeField] private GameObject powerUpHeal;
    [SerializeField] private Vector2[] spawnPoints;
    [SerializeField] private float difficultyMultiplier;
    [SerializeField] private float burstRate;
    [SerializeField] private float powerUpRatio;
    [SerializeField] private Text actual;
    [SerializeField] private Text killed;


    private int _actual;
    private long _killed;
    private GameManager _gameManager;
    private Paterns _paterns;
    private GameObject _player;
    [SerializeField] private float currentDifficulty;
    private float _powerOut;
    private float _burstTimer;
    private bool _burst;
    private bool _newMilestone;
    private int _nextMilestone;
    private float _nextCheck;
    private bool _powerUpPossible;
    private List<Helper.ShipBaseParams> _fleet;
    private Stack<Helper.ShipBaseParams> _ships;
    
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _paterns = GetComponent<Paterns>();
        _player = GameObject.FindGameObjectWithTag("Player");
        currentDifficulty = 0;
        _actual = 0;
        _killed = 0;
        _powerOut = 0;
        _burstTimer = burstRate;
        _nextMilestone = 500;
        _newMilestone = false;
        _burst = false;
        _ships = new Stack<Helper.ShipBaseParams>();
        _fleet = new List<Helper.ShipBaseParams>();
        _powerUpPossible = false;
        _nextCheck = powerUpRatio;
        GenerateSpawners();
        GiveIDs();
    }

    // Update is called once per frame
    void Update()
    {
        currentDifficulty += Time.deltaTime * difficultyMultiplier;

        #region UI

        actual.text = _actual.ToString();
        killed.text = _killed.ToString();

        #endregion
        
        if (_gameManager.score >= _nextMilestone)
        {
            _newMilestone = true;
        }

        if (!_burst)
        {
            _burstTimer -= Time.deltaTime;
            if (_burstTimer <= 0)
            {
                _burst = true;
                _burstTimer = burstRate;
                PrepareFleet(_newMilestone);
                if (_newMilestone)
                {
                    _newMilestone = false;
                    _nextMilestone *= 2;
                    difficultyMultiplier += 0.01f;
                    burstRate += burstRate / 2;
                }
            }
        }
        if (_burst)
        {
            if (_ships.Count > 0 )
            {
                int spawnIndex = ClosestSpawner(_player.transform.position);
                if (spawnIndex >= 0)
                {
                    GameObject shipPrefab = fleet[_ships.Pop().shipID];
                    _actual++;
                    float spawnTime = shipPrefab.GetComponent<EnemyShip>().spawnTime;
                    GameObject.Find($"Spawner {spawnIndex}").GetComponent<Spawner>().Spawn(spawnTime);
                    Instantiate(shipPrefab, spawnPoints[spawnIndex], Quaternion.Euler(new Vector3(0,0,180)), transform);
                }
            }
            else
            {
                _burst = false;
            }
        }
    }

    private void GenerateSpawners()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject spawner = new GameObject($"Spawner {i}", typeof(Spawner));
            spawner.transform.position = spawnPoints[i];
            spawner.AddComponent<Spawner>();
        }
    }
    
    private void GiveIDs()
    {
        for (int i = 0; i < fleet.Length; i++)
        {
            fleet[i].GetComponent<EnemyShip>().shipID = i;
            EnemyShip shipScript = fleet[i].GetComponent<EnemyShip>();
            _fleet.Add(new Helper.ShipBaseParams(shipScript.difficulty, shipScript.spawnTime, i));
        }
    }
    
    private int ClosestSpawner(Vector2 playerPosition)
    {
        int closestSpawner = -1;
        float closestDistance = float.MaxValue;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            float distance = Vector2.Distance(playerPosition, spawnPoints[i]);
            if (distance < closestDistance )
            {
                if (GameObject.Find($"Spawner {i}").GetComponent<Spawner>().isFree)
                {
                    closestDistance = distance;
                    closestSpawner = i;
                }
            }
        }

        return closestSpawner;
    }


    private void PrepareFleet(bool isBoss)
    {
        _ships = CalculateBestMatchFleet.CreateFleet(_fleet, currentDifficulty - _powerOut, isBoss);
        for (int i = 0; i < _ships.Count; i++)
        {
            _powerOut += _ships.Peek().difficulty;
        }
    }

    public void ShipDestroyed(float shipDifficulty, Vector2 position)
    {
        _powerOut -= shipDifficulty;
        _actual--;
        _killed++;
        _gameManager.AddScore(Mathf.RoundToInt(shipDifficulty / 2));
        
        if (_powerUpPossible)
        {
            SpawnPowerUp(position);
            _powerUpPossible = false;
        }
        else if (_gameManager.score >= _nextCheck)
        {
            _nextCheck += powerUpRatio;
            _powerUpPossible = true;
        }
    }
    
    void SpawnPowerUp(Vector2 position)
    {
        Instantiate(powerUpHeal, position, Quaternion.identity);
    }

    public void NewDirection(EnemyShip shipID, Vector2 playerOffset)
    {
        Vector3 direction = shipID.transform.position - (_player.transform.position + (Vector3)playerOffset);
        direction.Normalize();
        Vector2 absDirection = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        if(direction.x > 0)
        {
            if (direction.y > 0)
            {
                if (absDirection.x - absDirection.y > 0.5)
                {
                    shipID._move = _paterns.Left;
                }
                else if (absDirection.x - absDirection.y < -0.5)
                {
                    shipID._move = _paterns.Down;
                }
                else
                {
                    shipID._move = _paterns.LeftDown;
                }
            }
            else
            {
                if (absDirection.x - absDirection.y > 0.5)
                {
                    shipID._move = _paterns.LeftUp;
                }
                else if (absDirection.x - absDirection.y < -0.5)
                {
                    shipID._move = _paterns.Up;
                }
                else
                {
                    shipID._move = _paterns.Up;
                }
            }
        }
        else
        {
            if (direction.y > 0)
            {
                if (absDirection.x - absDirection.y > 0.5)
                {
                    shipID._move = _paterns.Right;
                }
                else if (absDirection.x - absDirection.y < -0.5)
                {
                    shipID._move = _paterns.Down;
                }
                else
                {
                    shipID._move = _paterns.RightDown;
                }
            }
            else
            {
                if (absDirection.x - absDirection.y > 0.5)
                {
                    shipID._move = _paterns.RightUp;
                }
                else if (absDirection.x - absDirection.y < -0.5)
                {
                    shipID._move = _paterns.Up;
                }
                else
                {
                    shipID._move = _paterns.Up;
                }
            }
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        
        foreach (var point in spawnPoints)
        {
            Handles.DrawSolidDisc(point, Vector3.forward, 0.3f);
        }
    }
    #endif
}
