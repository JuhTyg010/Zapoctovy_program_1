using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] fleet;
    
    //for more power ups, add more prefabs and add them to the array, for now we only have one so no need for a list
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
        //caching the reasonable big guys in the scene
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

        #region UI just reloads the values 

        actual.text = _actual.ToString();
        killed.text = _killed.ToString();

        #endregion
        
        //check if is time for a boss
        if (_gameManager.score >= _nextMilestone)
        {
            _newMilestone = true;
        }

        //check if is time for spawn some ships, if so, prepare the fleet, if not, wait
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
        //while the burst is happening, spawn the ships, if there is no more ships, stop the burst
        //ships are spawned in the closest spawner to the player, and one per frame
        else
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

    //this method is called on start and generates the spawners
    //it takes given spawn points (positions) and creates a game object with a spawner script on it
    private void GenerateSpawners()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject spawner = new GameObject($"Spawner {i}", typeof(Spawner));
            spawner.transform.position = spawnPoints[i];
            spawner.AddComponent<Spawner>();
        }
    }
    
    //this method is called on start and gives an ID to each ship in the fleet
    //cause fleet holds whole prefabs its easier to create new list with just the parameters we need and give them an ID
    private void GiveIDs()
    {
        for (int i = 0; i < fleet.Length; i++)
        {
            fleet[i].GetComponent<EnemyShip>().shipID = i;
            EnemyShip shipScript = fleet[i].GetComponent<EnemyShip>();
            _fleet.Add(new Helper.ShipBaseParams(shipScript.difficulty, shipScript.spawnTime, i));
        }
    }
    
    //this method is called to find out which spawner is closest to the player, and if its free
    //it runs through all the spawners and checks if they are free, if they are, it checks the distance to the player
    //free means that the spawner waited the spawn time and is ready to spawn again
    //if there is no free spawner, it returns -1
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


    //this method is called when the burst is about to start, it calls the CalculateBestMatchFleet class
    //gives the needed parameters and gets a stack of ships back, then it adds the difficulty of the ships to the power out
    private void PrepareFleet(bool isBoss)
    {
        _ships = CalculateBestMatchFleet.CreateFleet(_fleet, currentDifficulty - _powerOut, isBoss);
        for (int i = 0; i < _ships.Count; i++)
        {
            _powerOut += _ships.Peek().difficulty;
        }
    }

    //ShipDestroyed is called by the enemy ship when it dies, it takes the difficulty of the ship and the position
    //then it reduce the power out by the difficulty, reduce the actual number of ships, increase the killed number
    //and add score to the player, checks if should spawn a power up, if yes it spawns it
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
    
    //SpawnPowerUp generate copy of prefab on specified position
    void SpawnPowerUp(Vector2 position)
    {
        Instantiate(powerUpHeal, position, Quaternion.identity);
    }

    //this method is called by enemy ship when it considers that it should change direction
    //depending on the ships position and the player position, it chooses one of the paterns to move
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
    
    //special method for devel to visualize the spawn points in the editor
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
