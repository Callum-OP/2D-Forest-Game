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
public class PlayerController : MonoBehaviour
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
    // Controls arrows value
    private int arrows;
    // Controls max arrows value
    private int maxArrows;
    // Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;
    // Tells script there is a sprite renderer
    SpriteRenderer sr;
    // Used to add to time value
    private float levelTime;
    // The time that user must press second key within to double tap
    private float tapSpeed = 0.5f;

    [Header("Sprint Settings")]
    // Controls being able to double tap arrow keys to sprint
    private float lastRTapTime = 0;
    private float lastLTapTime = 0;
    private float lastUTapTime = 0;
    private float lastDTapTime = 0;
    private bool DoubleTapH;
    private bool DoubleTapV;

    [Header("Attack Settings")]
    // Controls being able to double tap space for ranged attack
    private float lastAttackTapTime = 0;
    private bool DoubleTapAttack;
    // Controls single tap melee attacks
    public float attackTime;
    public float startTimeAttack;
    public Transform attackLocation;
    public float attackRange;
    public LayerMask enemies;
    // Used to store time of when damage object last spawned
    private float timeOfLastAttack;

    [Header("Direction Settings")]
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

        // --Difficulty Settings--
        if (Settings.difficulty == 1) {
            // Set the speed value to 1.1
            speed = 1.1f;
            // Set the health value to 5
            maxHealth = 5;
            health = maxHealth;
            // Set the time it takes to create projectile to 1
            creationRate = 0.5f;
            // Set the speed projectile will travel at to 3.5
            shootSpeed = 3.5f;
            // Set number of max arrows
            maxArrows = 10;
            arrows = maxArrows;
        } else if (Settings.difficulty == 3) {
            // Set the speed value to 1
            speed = 1f;
            // Set the health value to 3
            maxHealth = 3;
            health = maxHealth;
            // Set the time it takes to create projectile to 1.5
            creationRate = 1.5f;
            // Set the speed projectile will travel at to 2.5
            shootSpeed = 2.5f;
            // Set number of max arrows
            maxArrows = 5;
            arrows = maxArrows;
        } else {
            // Set the speed value to 1
            speed = 1f;
            // Set the health value to 4
            maxHealth = 4;
            health = maxHealth;
            // Set the time it takes to create projectile to 1
            creationRate = 1f;
            // Set the speed projectile will travel at to 3
            shootSpeed = 3f;
            // Set number of max arrows
            maxArrows = 5;
            arrows = maxArrows;
        }

        // --Global Details--
        // Store player information globally
        Player.speed = speed;
        Player.health = maxHealth;
        Player.maxHealth = maxHealth;
        Player.creationRate = creationRate;
        Player.shootSpeed = shootSpeed;
        Player.maxArrows = maxArrows;
        Player.arrows = arrows;
        // Store scores information globally
		Scores.score = 0;
        Scores.gems = 0;
        Scores.time = 0;
        Scores.hearts = 0;
        Scores.level = Scores.level + 1;
        Scores.gemsToGet = 20;
        // Set up the values for the User Interface
		UserInterface.GetComponent<InGameUI>().SetHealthText();
        UserInterface.GetComponent<InGameUI>().SetArrowsText();
		UserInterface.GetComponent<InGameUI>().SetScoreText();
        UserInterface.GetComponent<InGameUI>().SetGemsText();
    }

    // Update is called once per frame
    void Update()
    {
        // --Player Details--
        // Update values for player based on global details
        speed = Player.speed;
        maxHealth = Player.maxHealth;
        creationRate = Player.creationRate;
        shootSpeed = Player.shootSpeed;
        maxArrows = Player.maxArrows;
        arrows = Player.arrows;
        // Set the current level time
        levelTime += Time.deltaTime;
        // Show the time taken so far in seconds
        Scores.time = Mathf.Round(levelTime * 1) / 1;
        // Values for storing arrow key input
        float xMove = 0;
        float zMove = 0;

        // --Sprinting Functionality--
        // Detect double taps of each arrow key
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            if((Time.time - lastRTapTime) < tapSpeed){
                DoubleTapH = true;
            } else {
                DoubleTapH = false;
            }
            lastRTapTime = Time.time;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            if((Time.time - lastLTapTime) < tapSpeed){
                DoubleTapH = true;
            } else {
                DoubleTapH = false;
            }
            lastLTapTime = Time.time;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            if((Time.time - lastUTapTime) < tapSpeed){
                DoubleTapV = true;
            } else {
                DoubleTapV = false;
            }
            lastUTapTime = Time.time;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            if((Time.time - lastDTapTime) < tapSpeed){
                DoubleTapV = true;
            } else {
                DoubleTapV = false;
            }
            lastDTapTime = Time.time;
        }

        // --Movement Functionality--
        // Getting input value from arrow keys
        // Using if statement so user can not move diagonally only horizontal or vertical vertical
        if (Input.GetAxisRaw("Horizontal") != 0) {
            // Right arrow key changes value to 1, left arrow key changes value to -1
            xMove = Input.GetAxisRaw("Horizontal");
            zMove = 0;
            // If arrow key double tapped increase speed
            if (DoubleTapH) {
                speed = speed * 1.5f;
            }
        } 
        if (Input.GetAxisRaw("Vertical") != 0) {
            // Up arrow key changes value to 1, down arrow key changes value to -1
            zMove = Input.GetAxisRaw("Vertical");
            xMove = 0;
            // If arrow key double tapped increase speed
            if (DoubleTapV) {
                speed = speed * 1.5f;
            }
        }
        // Creates velocity in direction of value equal to arrow keypress
        rb.velocity = new Vector3(xMove, zMove) * speed;

        // --Movement Animation--
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

        // --Attack Functionality--
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

        // Detect double taps of key to press
        if(Input.GetKeyDown(keyToPress)){
            if((Time.time - lastAttackTapTime) < tapSpeed){
                DoubleTapAttack = true;
            } else {
                DoubleTapAttack = false;
            }
            lastAttackTapTime = Time.time;
        }

        // Ranged attacks that fire projectile in a direction
        if(Input.GetKey(keyToPress) && Time.time >= timeOfLastSpawn + creationRate && DoubleTapAttack == true && arrows > 0) {
            Debug.Log("Double tap");
            Vector2 actualBulletDirection = true ? (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * shootDirection) : shootDirection;
            // Create the projectile and place it where player is
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
            Player.arrows = arrows - 1;
            arrows = arrows - 1;
        } 

        if (attackTime <= 0) {
            attackTime = startTimeAttack;
        }

        // Melee attacks that damage nearby enemies
        if(Input.GetKey(keyToPress) && attackTime <= 0 && DoubleTapAttack == false) {
            Debug.Log("Single tap");
            anim.SetBool("Is_Attacking", true);
            Collider2D[] damage = Physics2D.OverlapCircleAll( attackLocation.position, attackRange, enemies );
            for (int i = 0; i < damage.Length; i++)
            {
                // Only damage enemy once every second
                if (Time.time >= timeOfLastAttack + 1) {
                    // Create the projectile and place it where player is
                    GameObject damageObject = Instantiate<GameObject>(prefabToSpawn);
                    damageObject.transform.position = damage[i].transform.position;
                    timeOfLastAttack = Time.time;
                }

            }
        } else {
            attackTime -= Time.deltaTime;
            anim.SetBool("Is_Attacking", false);
        }

        // --Attack Animation--
        // Set whether character is attacking or not
        // And give enough time for attack animation to play
        if (Input.GetKey(keyToPress) && Time.time <= timeOfLastSpawn + 0.3 || Input.GetKey(keyToPress) && attackTime <= 0) {
            anim.SetBool("Is_Attacking", true);
            if (DoubleTapAttack) {
                anim.SetFloat("Is_Ranged", 1);
            } else {
                anim.SetFloat("Is_Ranged", 0);
            }
        } else {
            anim.SetBool("Is_Attacking", false);
            anim.SetFloat("Is_Ranged", 0.5f);
        }
        // Keep values for the User Interface up to date
		// The source of audio sounds
        UserInterface.GetComponent<InGameUI>().SetHealthText();
        UserInterface.GetComponent<InGameUI>().SetArrowsText();
		UserInterface.GetComponent<InGameUI>().SetScoreText();
        UserInterface.GetComponent<InGameUI>().SetGemsText();
    }

    // --Collisions With Triggers--
    // When player collides with other objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemies will deal damage to player health
        if(other.gameObject.CompareTag("Enemy")) {
            // Remove ten from the variable health
			Player.health = health - 1;
            health = health - 1;
            // Play hurt sound
            GetComponent<AudioSource>().clip = hurt;
            GetComponent<AudioSource>().Play();
			// Run the SetHealthText() function
			UserInterface.GetComponent<InGameUI>().SetHealthText();
            // Run Injured() coroutine
            StartCoroutine(Injured());
        }
        // Enemies will deal damage to player health
        if(other.gameObject.CompareTag("EnemyBullet")) {
            // Remove ten from the variable health
			Player.health = health - 1;
            health = health - 1;
            // Play hurt sound
            GetComponent<AudioSource>().clip = hurt;
            GetComponent<AudioSource>().Play();
			// Run the SetHealthText() function
			UserInterface.GetComponent<InGameUI>().SetHealthText();
            // Run Injured() coroutine
            StartCoroutine(Injured());
            // Destroy projectile
            Destroy(other.gameObject);
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
            if(health < maxHealth) {
                // Destroy other object
                other.gameObject.SetActive(false);
                // Add ten to the variable health
                health = health + 1;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}

