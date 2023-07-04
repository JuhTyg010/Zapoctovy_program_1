using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    private float _currentDifficulty;
    private float _powerOut;
    private float _spawnTimer;
    private float _burstTimer;
    private bool _burst;
    private Stack<GameObject> _ships;
    private float[] _shipDifficulty;
    
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _paterns = GetComponent<Paterns>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _currentDifficulty = 0;
        _powerOut = 0;
        _spawnTimer = spawnRate;
        _burstTimer = burstRate;
        _burst = false;
        _ships = new Stack<GameObject>();
        _shipDifficulty = new float[fleet.Length];
        SetupShipDifficulty();
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer -= Time.deltaTime;
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
                if (_spawnTimer <= 0)
                {
                    _spawnTimer = spawnRate;
                    SpawnShip();
                }
            }
            else
            {
                _burst = false;
            }
        }
    }
    
    void SetupShipDifficulty()
    {
        for (int i = 0; i < fleet.Length; i++)
        {
            EnemyShip shipScript = fleet[i].GetComponent<EnemyShip>();
            _shipDifficulty[i] = shipScript.difficulty;
        }
    }
    
    void SpawnShip()
    {
        
        //TODO: spawn specific ship based on difficulty and some special algorithm
        //for now just spawn random ship
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject ship = Instantiate(_ships.Pop(), spawnPoints[spawnIndex], Quaternion.identity, transform);
        EnemyShip shipScript = ship.GetComponent<EnemyShip>();
        shipScript.shipID = spawnIndex;
        _powerOut += shipScript.difficulty;
    }

    void PrepareFleet()
    {
        //TODO: spawn specific ship based on difficulty and some special algorithm
        while (_powerOut < _currentDifficulty)
        {
            AddShipToFleet();
        }
    }
    
    void AddShipToFleet()
    {
        int shipIndex = Random.Range(0, fleet.Length);
        _ships.Push(fleet[shipIndex]);
        _powerOut += _shipDifficulty[shipIndex];
    }

    public void ShipDestroyed(float shipDifficulty)
    {
        _powerOut -= shipDifficulty;
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
