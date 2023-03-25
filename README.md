# 2D-Forest-Game
A 2D Game using C# on VS Code and the Unity game engine where the player can explore a forest. The objective is to collect all gems in the world and the gems are found within caves hidden throughout the map, the player must find and enter these locations and then find the gem within each one. Enemies will be patrolling the forest and caves, they will chase the player if they get too close using an Astar pathfinding algorithm.

The player has melee attacks as well as ranged attacks with a bow that they can use to kill enemies. Enemies will die in one hit, enemies drop mutliple coins on death and can rarely also drop hearts.

The Player as well as enemies have animations for running in all four directions and the player has animations for when they attack. 

Background music plays during game time.

I have yet to add sound effects play when coins, hearts and gems are picked up.

Below is a list of the current modes available

There is a choice of three difficulties, easy, normal and hard. Below is a list of settings for different sprites and objects which may change depending on the difficulty.

## Easy difficulty:
Player has 5 hearts representing health.
Enemies deal 1 heart damage.
Hearts heal the player for 1 heart.
Coins will give 10 gold to the player.
Player is faster at moving and attacking.
Enemies are much more likely to drop hearts.

## Normal difficulty:
Player has 4 hearts representing health.
Enemies deal 1 heart damage.
Hearts heal the player for 1 heart.
Coins will give 10 gold to the player.
Player is an average speed at moving and attacking.
Enemies have a reasonable chance to drop hearts.

## Hard difficulty:
Player has 5 hearts representing health.
Enemies deal 1 heart damage.
Hearts heal the player for 1 heart.
Coins will give 10 gold to the player.
Player is slower at moving and attacking.
Enemies have an unlikely chance to drop hearts.
