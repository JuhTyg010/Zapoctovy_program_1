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
    
   ### Player Movement - One script is handling the PC input and player 	manager every update cycle asks about input and make movement out 	of it, same works for shooting
   ### Power-ups - so far there is only one power-up (cure) others will be added later. Enemy manager after killing some enemy decides if the enemy drops the loot, for now he decides based on score of player.
   ### Enemies and bullets - Instead of level design, this project is using generative mechanics for generating the enemies forever, based on current difficulty, which is increasing by time,  this part is handling so-called enemy manager, every enemy shoots bullets which visual, speed, direction and damage are specified by the specific enemy.  
   ### Boss Battles - when player achieves some score milestone, there should be a boss fight, however Bosses are as enemies and current engine sometimes recognize normal enemies as better match.
 
## User Interface (UI)
    
   ### Main Menu - you can select ship here, see credits and start the game 
   ### In-Game HUD - on the right side you can see your score, and leaderboard with top 5 players in global scale, on the left side, from head to bottom, you find health bar, then reload bar and statistics about enemies how many are there and how many have you killed already.
   ### Pause Menu - provides you with restart option and also with exit option which takes you back to menu
   ### Game Over Screen - there is an input field where you can submit your name and after closing the window it will be automatically sending to leaderboard data and resolved. Also, you can choose to go to menu or restart the game.

## Future

### Sounds and Music - there is set of sound effects which are comming in next versions and also background music
### Options - also cause the sound you could shut it down and for difficulty setting
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