using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

public class SurvivalPlayer : MonoBehaviour
{
    [Header("User Interface")]
    // Images used for rendering the five hearts in the UI
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Image heart4;
    public Image heart5;
    // Sprites used to change the display of the hearts in the UI
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Image used beside score text
    public Image scoreImage;
    // Sprites used to display image beside score in UI
    public Sprite scoreSprite;
    // Controls the displayed score text
    public TextMeshProUGUI scoreText;

    // Controls the current level text
    public TextMeshProUGUI timeSurvived;

    [Header("Audio Sounds")]
    // Sound when picking up coin
    public AudioClip coinPickup;
    // Sound when picking up fruit
    public AudioClip fruitPickup;
    // Sound when hurt
    public AudioClip hurt;
    // The source of audio sounds
    new AudioSource audio;

    [Header("Player Settings")]
    // Controls overall velocity multiplier
    public float speed;
    // Controls animation of player
    public Animator anim;
    // Controls horizontal speed value when key is pressed
    public float hf = 0.0f;
    // Controls vertical speed value when key is pressed
    public float vf = 0.0f;
    // Controls health value
    public int health;
    // Controls max health value
    public int maxHealth;
    // Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;
    // Tells script there is a sprite renderer
    SpriteRenderer sr;
    // Used to add to time value
    private float levelTime;

    // Values for what direction player should be facing
    private bool faceUp = false;
    private bool faceDown = false;
    private bool faceLeftSide = false;
    private bool faceRightSide = false;

    [Header("Projectile creation")]
    // Bullet prefab to be used
    public GameObject prefabToSpawn;
    // The key to press to create the projectiles
    public KeyCode keyToPress = KeyCode.Space;

    [Header("Projectile options")]
    // The time it takes to create projectile so long as the key is pressed
    public float creationRate;
    // The speed the projectile will travel at
    public float shootSpeed;
    // Used to store time of when projectile last spawned
    private float timeOfLastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        // --Player details--
        // Rb equals the rigidbody on the player
        rb = GetComponent<Rigidbody2D>();
        // Sr equals the sprite renderer on the player
        sr = GetComponent<SpriteRenderer>();
        // Audio equals the audio source on the player
        audio = GetComponent<AudioSource>();
        // Anim equals the animator on the player
        anim = this.GetComponent<Animator>();
        // Set time of last spawned projectile
        timeOfLastSpawn =- creationRate;

        //-- Difficulty settings
        if (Settings.difficulty == 1) {
            // Set the speed value to 1.1
            speed = 1.1f;
            // Set the health value to 5
            maxHealth = 5;
            health = maxHealth;
            // Set the time it takes to create projectile to 1
            creationRate = 0.5f;
            //Set the speed projectile will travel at to 3
            shootSpeed = 3f;
        } else if (Settings.difficulty == 3) {
            // Set the speed value to 1
            speed = 1f;
            // Set the health value to 3
            maxHealth = 3;
            health = maxHealth;
            // Set the time it takes to create projectile to 1
            creationRate = 1.5f;
            //Set the speed projectile will travel at to 1
            shootSpeed = 1f;
        } else {
            // Set the speed value to 1
            speed = 1f;
            // Set the health value to 5
            maxHealth = 4;
            health = maxHealth;
            // Set the time it takes to create projectile to 1
            creationRate = 1f;
            //Set the speed projectile will travel at to 2
            shootSpeed = 2f;
        }

        // --Scoreboard and game details--
        // Set the score value to zero
		Scores.score = 0;
        // Set the gems value to zero
        Scores.gems = 0;
        // Set the time value to zero
        Scores.time = 0;
        // Set the hearts used value to zero
        Scores.hearts = 0;

        // Set up the values for the User Interface
		SetHealthText();
		SetScoreText();
        SetTimeSurvivedText();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the current level time
        levelTime += Time.deltaTime;
        // Show the time taken so far in seconds
        Scores.time = Mathf.Round(levelTime * 1) / 1;
        SetTimeSurvivedText();
        // Values for storing arrow key input
        float xMove = 0;
        float zMove = 0;

        // Getting input value from arrow keys
        // Using if statement so user can not move diagonally only horizontal or vertical vertical
        if (Input.GetAxisRaw("Horizontal") != 0) {
            // Right arrow key changes value to 1, left arrow key changes value to -1
            xMove = Input.GetAxisRaw("Horizontal");
            zMove = 0;
        } 
        if (Input.GetAxisRaw("Vertical") != 0) {
            // Up arrow key changes value to 1, down arrow key changes value to -1
            zMove = Input.GetAxisRaw("Vertical");
            xMove = 0;
        }

        // Creates velocity in direction of value equal to arrow keypress
        rb.velocity = new Vector3(xMove, zMove) * speed;

        // Set which direction the character should be facing
        // And set animation value for if moving sideways
        if (xMove == 1) {
            faceUp = false;
            faceDown = false;
            faceLeftSide = false;
            faceRightSide = true;
            anim.SetBool("Is_Moving_Side", true);
            zMove = 0;
        } else if (xMove == -1) {
            faceUp = false;
            faceDown = false;
            faceLeftSide = true;
            faceRightSide = false;
            anim.SetBool("Is_Moving_Side", true);
            zMove = 0;
        } else {
            anim.SetBool("Is_Moving_Side", false);
        }
        if (zMove == 1) {
            faceUp = true;
            faceDown = false;
            faceLeftSide = false;
            faceRightSide = false;
            xMove = 0;
        } else if (zMove == -1) {
            faceUp = false;
            faceDown = true;
            faceLeftSide = false;
            faceRightSide = false;
            xMove = 0;
        }

        // Stores 1 if character is moving in that direction
        hf = xMove > 0.01f ? xMove : xMove < -0.01f ? 1 : 0;
        vf = zMove > 0.01f ? zMove : zMove < -0.01f ? 1 : 0;

        // Set direction of character animation
        if (faceUp == true) {
            anim.SetBool("Face_Up", true);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", false);
            anim.SetFloat("VerticalDirection", 1);
        } else if (faceDown == true) {
            anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", true);
            anim.SetBool("Face_Side", false);

            anim.SetFloat("VerticalDirection", -1);
        } else if (faceRightSide == true) {
            anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", true);
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        } else if (faceLeftSide == true) {
            anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", true);
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }

        // Value for deciding the direction to shoot
        Vector2 shootDirection = new Vector2(0f,0f);

        // Set direction of character weapon and projectile
        if (faceUp == true) {
            shootDirection = new Vector2(0, 1);
        } else if (faceDown == true) {
            shootDirection = new Vector2(0, -1);
        } else if (faceRightSide == true) {
            shootDirection = new Vector2(1, 0);
        } else if (faceLeftSide == true) {
            shootDirection = new Vector2(-1, 0);
        } else {
            shootDirection = new Vector2(0, -1);
        }

        // Set values for whether character is moving in a direction
        anim.SetFloat("Horizontal", hf);
        anim.SetFloat("Vertical", vf);

        // Fire projectile in a direction
        if(Input.GetKey(keyToPress) && Time.time >= timeOfLastSpawn + creationRate) {
            Vector2 actualBulletDirection = true ? (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * shootDirection) : shootDirection;
            // Create the projectile and place it where player
            GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
            newObject.transform.position = this.transform.position;

            // Push the created projectile, but only if they have a Rigidbody2D
            Rigidbody2D rigidbody2D = newObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.AddForce(actualBulletDirection * shootSpeed, ForceMode2D.Impulse);
                newObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, actualBulletDirection);
            }
            // Set the time projectile was last spawned
            timeOfLastSpawn = Time.time;
        }

        // Set whether character is attacking or not
        // And give enough time for attack animation to play
        if (Input.GetKey(keyToPress) && Time.time <= timeOfLastSpawn + 0.3) {
            anim.SetBool("Is_attacking", true);
        } else {
            anim.SetBool("Is_attacking", false);
        }
    }

    // When player collides with other objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemies will deal damage to player health
        if(other.gameObject.CompareTag("Enemy")) {
            // Remove ten from the variable health
			health = health - 1;
            // Play hurt sound
            audio.clip = hurt;
            audio.Play ();
			// Run the SetHealthText() function
			SetHealthText ();
            // Run Injured() coroutine
            StartCoroutine (Injured());
        }
        // Coins will give score to player
        if(other.gameObject.CompareTag("Coin")) {
            // Destroy other object
            other.gameObject.SetActive(false);
            // Add ten to the variable score
			Scores.score = Scores.score + 10;
            // Play coin pickup sound
            audio.clip = coinPickup;
            audio.Play ();
			// Run the SetScoreText() function
			SetScoreText ();
        }
        // Fruit will heal player health
        if(other.gameObject.CompareTag("Fruit")) {
            // If injured
            if(health < maxHealth) {
                // Destroy other object
                other.gameObject.SetActive(false);
                // Add ten to the variable health
                health = health + 1;
                // Add 1 to hearts used value
                Scores.hearts = Scores.hearts + 1;
                // Play fruit pickup sound
                audio.clip = fruitPickup;
                audio.Play ();
            }
			// Run the SetHealthText() function
			SetHealthText ();
        }
    }
    // Used to set the text in the UI to the correct amount of health player has
    void SetHealthText()
	{
        // Change hearts in UI based on max health
        switch (maxHealth) {
            // If max health is 3
            case 3:
                // Show first 3 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = false;
                heart5.enabled = false;
                break;
            // If max health is 4
            case 4:
                // Show first 4 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = false;
                break;
            // If max health is 5
            case 5:
                // Show first 5 hearts
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                heart4.enabled = true;
                heart5.enabled = true;
                break;
        }

            
        // Change hearts in UI based on current health
        switch (health) {
            // If health is 0
            case 0:
                // Display current health
                heart1.sprite = emptyHeart;
                heart2.sprite = emptyHeart;
                heart3.sprite = emptyHeart;
                heart4.sprite = emptyHeart;
                heart5.sprite = emptyHeart;
                // Locate and load scene called Survival Scoreboard
                SceneManager.LoadScene("Survival Scoreboard");
                break;
            // If health is 1
            case 1:
                heart1.sprite = fullHeart;
                heart2.sprite = emptyHeart;
                heart3.sprite = emptyHeart;
                heart4.sprite = emptyHeart;
                heart5.sprite = emptyHeart;
                break;
            // If health is 2
            case 2:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = emptyHeart;
                heart4.sprite = emptyHeart;
                heart5.sprite = emptyHeart;
                break;
            // If health is 3
            case 3:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = emptyHeart;
                heart5.sprite = emptyHeart;
                break;
            // If health is 4
            case 4:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = emptyHeart;
                break;
            // If health is 5
            case 5:
                heart1.sprite = fullHeart;
                heart2.sprite = fullHeart;
                heart3.sprite = fullHeart;
                heart4.sprite = fullHeart;
                heart5.sprite = fullHeart;
                break;
        }
	}
    // Used to set the text in the UI to the correct number of gold collected
    void SetScoreText()
	{
        // Display image for score
        scoreImage.sprite = scoreSprite;
        // Display current Scores.score
		scoreText.text = Scores.score.ToString();
	}
    // Used to set the text in the UI to the time survived
    void SetTimeSurvivedText()
	{
        // Display current time survived
		timeSurvived.text = "Time Survived " + Scores.time.ToString();
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