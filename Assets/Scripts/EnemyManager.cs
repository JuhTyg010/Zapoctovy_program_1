using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fleet;
    [SerializeField] private Vector2[] spawnPoints;
    [SerializeField] private float difficultyMultiplier;
    [SerializeField] private float spawnRate;
    [SerializeField] private float burstRate;
    
    
    private GameManager _gameManager;
    private Paterns _paterns;
    private GameObject _player;
    [SerializeField] private float _currentDifficulty;
    [SerializeField] private float _powerOut;
    private float _burstTimer;
    private bool _burst;
    //private Stack<GameObject> _ships;
    private List<Helper.ShipBaseParams> _fleet;
    
    //rename this to _ships when CalculateBestMatchFleet is ready
    private Stack<Helper.ShipBaseParams> _ships;
    
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _paterns = GetComponent<Paterns>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _currentDifficulty = 0;
        _powerOut = 0;
        _burstTimer = burstRate;
        _burst = false;
        _ships = new Stack<Helper.ShipBaseParams>();
        _fleet = new List<Helper.ShipBaseParams>();
        GenerateSpawners();
        GiveIDs();
    }

    // Update is called once per frame
    void Update()
    {
        _currentDifficulty = _gameManager.score * difficultyMultiplier;
        if (!_burst)
        {
            _burstTimer -= Time.deltaTime;
            if (_burstTimer <= 0)
            {
                _burst = true;
                _burstTimer = burstRate;
                PrepareFleet();
            }
        }
        if (_burst)
        {
            if (_ships.Count > 0 )
            {
                int spawnIndex = ClosestSpawner(_player.transform.position);
                if (spawnIndex >= 0)
                {
                    //ToDo: spawn rate should be based on ship difficulty
                    GameObject.Find($"Spawner {spawnIndex}").GetComponent<Spawner>().Spawn(spawnRate);
                    SpawnShip(spawnPoints[spawnIndex]);
                }
            }
            else
            {
                _burst = false;
            }
        }
    }

    void GenerateSpawners()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject spawner = new GameObject($"Spawner {i}", typeof(Spawner));
            spawner.transform.position = spawnPoints[i];
            spawner.AddComponent<Spawner>();
        }
    }
    
    void GiveIDs()
    {
        for (int i = 0; i < fleet.Length; i++)
        {
            fleet[i].GetComponent<EnemyShip>().shipID = i;
            EnemyShip shipScript = fleet[i].GetComponent<EnemyShip>();
            _fleet.Add(new Helper.ShipBaseParams(shipScript.difficulty, shipScript.spawnTime, i));
        }
    }
    
    int ClosestSpawner(Vector2 playerPosition)
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
    
    
    void SpawnShip(Vector2 spawnPoint)
    {
        //TODO: spawn specific ship based on difficulty and some special algorithm
        GameObject ship = Instantiate(fleet[_ships.Pop().shipID], spawnPoint, Quaternion.identity, transform);
        EnemyShip shipScript = ship.GetComponent<EnemyShip>();
    }

    void PrepareFleet()
    {
        while (_powerOut < _currentDifficulty)
        {
            AddShipToFleet();
        }
    }
    
    void AddShipToFleet()
    {
        //TODO: choose better combo system
        int combo = Random.Range(0, CalculateBestMatchFleet.Combos.Length);
        
        //TODO: replace this with better algorithm
        _ships = CalculateBestMatchFleet.CreateFleet(_fleet, combo ,_currentDifficulty );
        for(int i = 0; i < _ships.Count; i++)
        {
            _powerOut += _ships.Peek().difficulty;
        }
        
    }

    public void ShipDestroyed(float shipDifficulty)
    {
        _powerOut -= shipDifficulty;
        _gameManager.AddScore(shipDifficulty / 2);
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
