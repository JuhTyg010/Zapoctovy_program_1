# Documentation of Pew pew

_Version 1.0.0_ 
_Date: 23.8.2023_
_Unity Version 2021.3.15f1_ 


## Concept

Bullet Hell is an intense and fast-paced arcade shooter game where players control a spaceship and engage in battles against waves of enemies. The gameplay is characterized by a relentless barrage of enemy bullets that players must skillfully navigate through to survive.

## Controls
    
Movement is made by **W** - up, **A** - left **S** -down **D** - right or arrows. 
Player shoots with **SPACE**  and you can pause the game by pressing **P** or **ESC**

## Game Mechanics
    
   ### Player Movement 
   One script is handling the PC input and player 	manager every update cycle asks about input and make movement out 	of it, same works for shooting
   ### Power-ups 
   so far there is only one power-up (cure) others will be added later. Enemy manager after killing some enemy decides if the enemy drops the loot, for now he decides based on score of player.
   ### Enemies and bullets 
   Instead of level design, this project is using generative mechanics for generating the enemies forever, based on current difficulty, which is increasing by time,  this part is handling so-called enemy manager, every enemy shoots bullets which visual, speed, direction and damage are specified by the specific enemy.  
   ### Boss Battles 
   when player achieves some score milestone, there should be a boss fight, however, Bosses are enemies and the current engine sometimes recognize normal enemies as better match.
 
## User Interface (UI)
    
   ### Main Menu You can select ship here, see credits and start the game 
   ### In-Game HUD 
   on the right side you can see your score, and leaderboard with top 5 players in a global scale, on the left side, from head to bottom, you find a health bar, then a reload bar and statistics about enemies how many are there and how many have you killed already.
   ### Pause Menu 
   provides you with a restart option and also with exit option which takes you back to the menu
   ### Game Over Screen 
   there is an input field where you can submit your name and after closing the window it will be automatically sending to leaderboard data and resolved. Also, you can choose to go to menu or restart the game.

## Future

### Sounds and Music There is set of sound effects that are comming in next versions and also background music
### Options 
also cause the sound you could shut it down and for difficulty setting
### More power-ups
### More ships, and enemies


## Credits

**Programming:**  
<i>Marek Sádovský</i>  
*Danial Jumagaliyev (leaderboard server)*
  
**Graphics:**  
<i>Marek Sádovský  
Skorpio  
Ram Zorkot  
https://www.gamedevmarket.net/  
Kenney - https://www.kenney.nl/  
https://craftpix.net/  
https://www.gameart2d.com/</i>  
  
**Sounds:** 
<i> generous anonymous </i>

## Programming part

### Basic scripts

#### ChangeSizeOverTime
MonoBehavior script with two parameters float speed and Vector2 sizeChange. These two provide speed and selected axis change.
#### Lifetime
MonoBehavior script with one parameter float lifetime. After time runs out the script will destroy its object.
*public function SetLifetime(float lifeTime)* - provides external setup of lifetime parameter.

#### LoadCredits
MonoBehavior script with one parameter GameObject credits, which contains a text element.
*public void LoadAllCredits()* - Loads credits from Resources/credits file and Loads them to the above specific GameObject _public_ void DeleteCredits()* - Deletes credits text from GameObject to save some memory space.

#### Movement
MonoBehavior script with two parameters float speed and Vector2 direction. These two provide speed and direction to move the object they are assigned to.
*public void SetMovement(float speed, Vector2 direction)* - provides external setup of movement parameter

#### Rotate
MonoBehavior script with one parameter Vector3 to specify which axes to rotate and which speed to rotate

#### SecondaryShoot
MonoBehavior script. Parameters are floats:
* reloadTime - specifies the time between two shoot attempts
* bulletSpeed - specifies the speed of a bullet
* bulletLifeTime - specifies the lifetime of a bullet
* bulletDamage - specifies the damage dealt by a bullet
* and GameObject bulletPrefab - specifies visual of bullet
 *private void CreateBullet()* - create a copy of bullet specified by bulletPrefab with parameters specified above.

 ### Static
 #### MyInput
 This static class provides some kind of gate between the keyboard and the game.
 *public static Vector2 GetDirection(float verticalMultiplier, float horizontalMultiplier)* - returns the direction of the horizontal and vertical axes based on the keys pressed.

 *public static bool IsShooting()* - returns true if space is pressed (through the "Fire1" key in unity).

 *public static bool IsPause()* - returns true if P or ESC is pressed

 #### Helper
 *public struct ShipBaseParams* - this struct contains the parameters difficulty, spawnTime and shipID to reduce data needed to run through when choosing a new fleet. This structure also has a constructor with all three parameters

 *public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component* - this function runs through all the game objects in children and returns a list of their components that have the same tag as the specified one.
 *public static GameObject[] ChildrenWithTag(this GameObject parent, string tag, bool forceActive = false)* - this function calls the previous one and then returns the list of children gameObjects that have the same tag as the specified one instead of components.

 #### SaveSystem

 *public static void SaveShipId(int id)* - SaveShipId is called from outside as a static method. It saves the ship id to the ship.id file in the persistent data path. By using the BinaryFormatter, we can serialize the data to a file, which is a binary file, so it is not readable.

 *public static void SaveName(string name)* -  SaveName is called from outside as a static method. It saves the name of the last saved player to the Last.name file in the persistent data path. By using the BinaryFormatter, we can serialize the data to a file, which is a binary file, so it is not readable.

*public static int LoadShipId()* - LoadShipId is also called from outside as a static method. It loads the ship id from the ship.id file in the persistent data path. By using the BinaryFormatter, we can deserialize the data from a file, which is a binary file, so it is not readable and returns the same variables.

*public static string LoadName()* - LoadName is called from outside as a static method. It loads the name of the last saved player to the Last.name file in the persistent data path.

### BackgroundManager
MonoBehaviour script that has these parameters:
* float speed - specifies the speed of movement of the background
* float offset - specifies the distance of two main background
* float blurDelta - specifies the time between generating two secondary backgrounds 
* GameObject mainBackground - is the game object that the player see the whole time
* GameObject[] secondaryBackgrounds - these gameObjects are generated based on the blurDelta.

Script has _main1 and _main2 which are the two main backgrounds that are constantly moving down. If the main backgrounds are below the offset, they are moved back up. If the blurTimer is below 0, a new blur background is spawned. Blur is selected randomly from the secondaryBackgrounds array. The movement of blur is the same as the main backgrounds, but it is destroyed after a certain amount of time when they leave the player's view.

### Bullet
*public void SetBullet(float speed, float lifeTime, float damage, Vector2 direction, string targetTag)* - this void is called from outside as it was a constructor, it saves the parameters.
Object with this class is moving based on the SetBullet parameters, if this object collides with another object with the tag specified by the targetTag it deals damage to the target object, and destroys itself. If this object leaves the Gamefield bullet will be destroyed.

### BulletGenerator
MonoBehavior script that generates bullets. It is attached to the enemy's or player's ship, purpose is to automatically generate bullets, based on some time interval.
*private void CreateBullet()* - is a method that creates a copy of a specific bullet and sets its properties. After creating some bullets this method destroys this gameobject.

### CalculateBestMatchFleet
*public static Stack<Helper.ShipBaseParams> CreateFleet(List<Helper.ShipBaseParams> fleet, float difficulty, bool isBoss)* - is a method that creates a fleet of ships. It's called by the EnemyManager which gives it a list of ships and a difficulty which represents the output power of the fleet, isBoss bool is for different constants. The method returns a stack of ships that will be spawned.

*private static int MaxInBurst(Helper.ShipBaseParams ship, float difficulty)* - is a method that returns the maximum number of ships of a certain type that can be spawned at once in current difficulty.

*private static int FindBestCoefficient(List<(int, float)> coefficient)* - is a method that returns the shipID of the ship that has the best 'coefficient'. The coefficient is a list of tuples where Item1 is shipID and Item2 is a coefficient for some ship. This coefficient is calculated by dividing the difference between the difficulty and the number of ships, multiplied by the difficulty of the ship, also affected by the number of ships in CreateFleet().

### EnemyManager
This class uses these properties to setup:
* GameObject[] fleet - the list of different ships that the enemy can spawn
* GameObject powerUpHeal - a special object that is spawned when some enemies die
* Vector2[] spawnPoints - specifies the positions of so called spawners, places to spawn the ships
* float difficultyMultiplier - float to specify a speed of increasing the difficulty
* float burstRate - float to specify delta time between two consecutive generating of ships
* float powerUpRatio - float to specify how often the powerup is generated
* Text actual - text to show how many ships the enemy has
* Text killed - text to show how many enemy ships the player has killed

This class is monoBehaviour so it has a default method like update() that handles the generating of ships displaying the enemy texts to UI.
*private void GenerateSpawners(_)_ - This method is called on start and generates the spawners. It takes given spawn points (positions) and creates a game object with a spawner script on it
*private void GiveIDs()* - this method is called on start and gives an ID to each ship in the fleet. Cause the fleet holds whole prefabs it is easier to create a new list with just the parameters we need and give them an ID.
*private int ClosestSpawner(Vector2 playerPosition)* - this method is called to find out which spawner is closest to the player, and if it's free it runs through all the spawners and checks if they are free, if they are, it checks the distance to the player. Free means that the spawner waited the spawn time and is ready to spawn again, if there is no free spawner, it returns -1
*private void PrepareFleet(bool isBoss)* - this method is called when the burst is about to start. It calls the CalculateBestMatchFleet class, gives the needed parameters and gets a stack of ships back, then it adds the difficulty of the ships to the power out.
*public void ShipDestroyed(float shipDifficulty, Vector2 position)* - ShipDestroyed is called by the enemy ship when it dies, it takes the difficulty of the ship and the position. Then it reduce the power out by the difficulty, reduce the actual number of ships, increase the killed number and add score to the player, checks if should spawn a power up, if yes it spawns it.
*void SpawnPowerUp(Vector2 position)* - this method generate copy of prefab on specified position
*public void NewDirection(EnemyShip shipID, Vector2 playerOffset)* - this method is called by enemy ship when it considers that it should change direction. Depending on the ships position and the player position, it chooses one of the paterns to move.

### EnemyShip
This class inherits from Ship class which is MonoBehavior. It has these properties: 
* float difficulty - float to specify the difficulty of specific ship
* float spawnTime - float to specify the time which it takes to spawn specific ship
* float changeDirectionTime - float to specify how often to change direction
* Vector2 playerOffset - position which is ideal to be on compared to the player
* GameObject explosionPrefab - object which is shown after death
* GameObject scorePrefab - object which is shown after death and shows the score
This class use also delegate Move in which is pushed movement function based on the ship's position. Enemy ship shoots always if it can.
*public override void TakeDamage(float damage)* - is a method that is called when the ship is hit by a bullet. It is called by the specific bullet that hit the ship, it reduces the health of the ship and if the health is 0 or less, it destroys the ship and creates an explosion animation and shows score.
OnDestroy it calles the EnemyManager function ShipDestroyed().

### GameManager
This method handles the game so it has many UI properties such as Healthbar or score. Also there are specified borders for player which he can't leaves. This script checks if the game isn't over or if a player didn't pressed the pause button. If the game is over it shows the game over UI panel and shows the score, and saves the name and score to leaderboard. In this class are also defined functions of some buttons like resume, restart or exit which are changing the scenes or time scale of the game.

### LeaderBoard
This class constains the public key for some specific leaderboard.
*public static List<string> GetLeaderboard(int max)* - this method returns a list of strings with the top scores. Max is the number of scores to return, we check if the number of scores is less than max, if so we return the number of scores, otherwise we return max
*public static void SetLearderboardEntry(string name, int score)* - this method uploads a new entry to the leaderboard than we refresh the leaderboard.

### MenuShips
This script handles UI in menu. It contains logic for some buttons in menu and in select ship panel. Then there is void Load() this method get propterities from ship prefabs and show them on select ship panel.

### Paterns
This script contains easy movement methods which are passed to EnemyShip by the delegate.

### PlayerManager
This script handles movement of player ship, shooting and healing. Properties:
* float health - the health of the player
* Slider reloadSlider - shows time to next shot possible
* List<GameObject> ships - list of possible player ships
*void Move(Vector2 direction)* - this method moves the player, it asks the MyInput to get the direction, than it set movement to the ship.
*public void Heal(float heal)* - this method is called from outside and it heals the player by float passed as parameter

### PlayerShip
This script inherits from ship, only aditional thing is function Shooting which is called from outside by PlayerManager

### PowerUp
Object with this script can do special ability on trigger when triggers with player ship. For now it calls Heal in PlayerManager, in future it should be universal for many kinds of power ups.

### Ship
This is monoBehaviour script, contains properties for bullet which are shooted by specific ship, also contains properties of the ship as health and speed.
*void CreateBullet(string target)* - is a method that creates a copy of a specific bullet and sets its properties. After creating some bullets this method destroys this gameobject.
*protected void Shoot(string target)* - Shoot is different for the player and the enemy, but the reloading is the same, so we reload the weapon and then call the shoot method of the child class

### Spawner
This class is used by spawners to handle when they are free to generate next enemy ship. When ship is going to be generated it call the function Spawn from this class and set timer to wait for the next possible generating.