using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fleet;
    [SerializeField] private int[] shipDifficulty;
    [SerializeField] private Vector2[] spawnPoints;
    [SerializeField] private float difficultyMultiplier;
    [SerializeField] private float spawnRate;
    
    
    private GameManager _gameManager;
    private Paterns _paterns;
    private GameObject _player;
    private float _currentDifficulty;
    private float _powerOut;
    private float _spawnTimer;
    
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _paterns = GetComponent<Paterns>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _currentDifficulty = 0;
        _powerOut = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        _currentDifficulty = _gameManager.score * difficultyMultiplier;
        if((_currentDifficulty > _powerOut) && (_spawnTimer <= 0))
        {
            SpawnShip();
            _spawnTimer = spawnRate;
        }
    }
    
    void SpawnShip()
    {
        
        //TODO: spawn specific ship based on difficulty and some special algorithm
        //for now just spawn random ship
        int shipIndex = Random.Range(0, fleet.Length);
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject ship = Instantiate(fleet[shipIndex], spawnPoints[spawnIndex], Quaternion.identity, transform);
        EnemyShip shipScript = ship.GetComponent<EnemyShip>();
        shipScript.shipID = shipIndex;
        _powerOut += shipDifficulty[shipIndex];
    }
    

    public void ShipDestroyed(int shipID)
    {
        _powerOut -= shipDifficulty[shipID];
    }

    public void NewDirection(EnemyShip shipID)
    {
        Vector3 direction = shipID.transform.position - _player.transform.position;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Handles.color = Color.green;
        
        foreach (var point in spawnPoints)
        {

            Handles.DrawSolidDisc(point, Vector3.forward, 0.3f);
            
            //Gizmos.DrawSphere(point, 0.3f);
        }
    }
}
