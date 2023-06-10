using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

// Class for storing details about the player
public class Player {
    // Store the speed value
    public static float speed;
    // Store the time it takes to create projectile
    public static float creationRate;
    // Store the speed projectile will travel at
    public static float shootSpeed;
    // Store current health value
    public static int health;
    // Store max health value
    public static int maxHealth;
    // Store arrows value
    public static int arrows;
    // Store max arrows value
    public static int maxArrows;
}

// Class for managing the player and user interface
public class PlayerSettings : MonoBehaviour
{
    [Header("User Interface")]
    // Reference user interface
    public GameObject UserInterface;

    [Header("Audio Sounds")]
    // Sound when picking up coin
    public AudioClip coinPickup;
    // Sound when picking up gems
    public AudioClip gemPickup;
    // Sound when picking up fruit
    public AudioClip fruitPickup;
    // Sound when hurt
    public AudioClip hurt;
    // The source of audio sounds
    new AudioSource audio;
    
    [Header("Player Components")]
    // Tells script there is a sprite renderer
    SpriteRenderer sr;

    // Used to add to time value
    private float levelTime;

    // Start is called before the first frame update
    void Start()
    {
        // Sr equals the sprite renderer on the player
        sr = GetComponent<SpriteRenderer>();
        // Audio equals the audio source on the player
        audio = GetComponent<AudioSource>();

        Scores.gameWon = false;

        // --Difficulty Settings--
        if (Settings.difficulty == 1) {
            // Set the speed value to 8
            Player.speed = 8f;
            // Set the health value to 5
            Player.maxHealth = 5;
            Player.health = Player.maxHealth;
            // Set the time it takes to create projectile to 1
            Player.creationRate = 0.5f;
            // Set the speed projectile will travel at to 6.5
            Player.shootSpeed = 6.5f;
            // Set number of max arrows
            Player.maxArrows = 10;
            Player.arrows = Player.maxArrows;
        } else if (Settings.difficulty == 3) {
            // Set the speed value to 5
            Player.speed = 7f;
            // Set the health value to 3
            Player.maxHealth = 3;
            Player.health = Player.maxHealth;
            // Set the time it takes to create projectile to 1.5
            Player.creationRate = 1.5f;
            // Set the speed projectile will travel at to 5.5
            Player.shootSpeed = 5.5f;
            // Set number of max arrows
            Player.maxArrows = 5;
            Player.arrows = Player.maxArrows;
        } else {
            // Set the speed value to 1
            Player.speed = 7.5f;
            // Set the health value to 4
            Player.maxHealth = 4;
            Player.health = Player.maxHealth;
            // Set the time it takes to create projectile to 1
            Player.creationRate = 1f;
            // Set the speed projectile will travel at to 6
            Player.shootSpeed = 6f;
            // Set number of max arrows
            Player.maxArrows = 5;
            Player.arrows = Player.maxArrows;
        }

        // Store scores information globally
		Scores.score = 0;
        Scores.gems = 0;
        Scores.time = 0;
        Scores.hearts = 0;
        Scores.gemsToGet = 5;

        // Set up the values for the User Interface
		UserInterface.GetComponent<InGameUI>().SetHealthText();
        UserInterface.GetComponent<InGameUI>().SetArrowsText();
		UserInterface.GetComponent<InGameUI>().SetScoreText();
        UserInterface.GetComponent<InGameUI>().SetGemsText();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the current level time
        levelTime += Time.deltaTime;
        // Show the time taken so far in seconds
        Scores.time = Mathf.Round(levelTime * 1) / 1;

        // Keep values for the User Interface up to date
        UserInterface.GetComponent<InGameUI>().SetHealthText();
        UserInterface.GetComponent<InGameUI>().SetArrowsText();
		UserInterface.GetComponent<InGameUI>().SetScoreText();
        UserInterface.GetComponent<InGameUI>().SetGemsText();

        if (Player.health == 0) {
            // Load the death scene
            SceneManager.LoadScene("Death");
        }
    }

    // --Collisions With Triggers--
    // When player collides with other objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemies will deal damage to player health
        if(other.gameObject.CompareTag("Enemy")) {
            // Remove ten from the variable health
			Player.health = Player.health - 1;
            // Play hurt sound
            GetComponent<AudioSource>().clip = hurt;
            GetComponent<AudioSource>().Play();
			// Run the SetHealthText() function
			UserInterface.GetComponent<InGameUI>().SetHealthText();
            // Run Injured() coroutine
            StartCoroutine(Injured());
        }
        // Coins will give score to player
        if(other.gameObject.CompareTag("Coin")) {
            // Destroy other object
            other.gameObject.SetActive(false);
            // Add ten to the variable score
			Scores.score = Scores.score + 10;
            // Play coin pickup sound
            GetComponent<AudioSource>().clip = coinPickup;
            GetComponent<AudioSource>().Play();
			// Run the SetScoreText() function
			UserInterface.GetComponent<InGameUI>().SetScoreText();
        }
        // Chests will give score to player
        if(other.gameObject.CompareTag("Chest")) {
            // Destroy other object
            other.gameObject.SetActive(false);
            // Add one hundred to the variable score
			Scores.score = Scores.score + 100;
            // Play coin pickup sound
            GetComponent<AudioSource>().clip = coinPickup;
            GetComponent<AudioSource>().Play();
			// Run the SetScoreText() function
			UserInterface.GetComponent<InGameUI>().SetScoreText();
        }
        // Gems will be collected until none are left
        if(other.gameObject.CompareTag("Gem")) {
            // Destroy other object
            other.gameObject.SetActive(false);
            // Add one hundred to the variable score
			Scores.gems = Scores.gems + 1;
            // Play gem pickup sound
            GetComponent<AudioSource>().clip = gemPickup;
            GetComponent<AudioSource>().Play();
            // Run the SetGemsText() function
            UserInterface.GetComponent<InGameUI>().SetGemsText();
        }
        // Fruit will heal player health
        if(other.gameObject.CompareTag("Fruit")) {
            // If injured
            if(Player.health < Player.maxHealth) {
                // Destroy other object
                other.gameObject.SetActive(false);
                // Add ten to the variable health
                Player.health = Player.health + 1;
                // Add 1 to hearts used value
                Scores.hearts = Scores.hearts + 1;
                // Play fruit pickup sound
                GetComponent<AudioSource>().clip = fruitPickup;
                GetComponent<AudioSource>().Play ();
            }
			// Run the SetHealthText() function
			UserInterface.GetComponent<InGameUI>().SetHealthText();
        }
    }
    
    // Used to indicate to user that player has taken damage
    IEnumerator Injured()
    {
        // Make player look injured by turning them red
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        // Make player look normal again
        sr.color = Color.white;
    }     
}

