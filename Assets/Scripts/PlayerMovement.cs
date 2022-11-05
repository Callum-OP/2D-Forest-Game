using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// Allows for scene management
using UnityEngine.SceneManagement;

public class TTotalScores 
{
    // Controls time value
    public static float totalTime;
    // Controls hearts used value
    public static int totalHearts;
    // Controls score value
    public static int totalScore;
}

public class TScores 
{
    // Controls time value
    public static float time;
    // Controls hearts used value
    public static int hearts;
    // Controls score value
    public static int score;
    // Controls level count value
    public static int level = 0;
}

public class PlayerMovement : MonoBehaviour
{
    // Controls the displayed health text
    public TextMeshProUGUI healthText;
    // Controls the displayed Scores.score text
    public TextMeshProUGUI scoreText;
    // Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;
    // Controls overall velocity multiplier
    public float speed = 1f;
    // Controls animation of player
    public Animator anim;
    // Controls horizontal speed value
    public float hf = 0.0f;
    // Controls vertical speed value
    public float vf = 0.0f;
    // Controls health value
    public int health;
    // Used to add to time value
    private float levelTime;

    // Start is called before the first frame update
    void Start()
    {
        // Rb equals the rigidbody on the player
        rb = GetComponent<Rigidbody2D>();
        // Anim equals the animator on the player
        anim = this.GetComponent<Animator>();
        // Set the health value to one hundred
		health = 100;
        // Set the score value to zero
		Scores.score = 0;
        // Set the time value to zero
        Scores.time = 0;
        // Set the hearts used value to zero
        Scores.hearts = 0;
        // Add one to the level count value
        Scores.level = Scores.level + 1;

		SetHealthText ();
		SetScoreText ();
    }

    // Update is called once per frame
    void Update()
    {
        levelTime += Time.deltaTime;
        Scores.time = Mathf.Round(levelTime * 1) / 1;

        // Right arrow key changes value to 1, left arrow key changes value to -1
        float xMove = Input.GetAxisRaw("Horizontal");
        // Up arrow key changes value to 1, Down arrow key changes value to -1
        float zMove = Input.GetAxisRaw("Vertical");
        // Creates velocity in direction of value equal to arrow keypress
        rb.velocity = new Vector3(xMove, zMove) * speed; 

        hf = xMove > 0.01f ? xMove : xMove < -0.01f ? 1 : 0;
        vf = zMove > 0.01f ? zMove : zMove < -0.01f ? 1 : 0;
        if (xMove < -0.01f) {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("Is_Moving_Side", true);
        } else
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("Is_Moving_Side", true);
        }
        if (xMove == 0) {
            anim.SetBool("Is_Moving_Side", false);
        }

        anim.SetFloat("Horizontal", hf);
        anim.SetFloat("Vertical", zMove);
        anim.SetFloat("Speed", vf);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy")) {
            // Remove ten from the variable health
			health = health - 10;
			// Run the SetHealthText() function
			SetHealthText ();
        }
        if(other.gameObject.CompareTag("Coin")) {
            // Destroy other object
            other.gameObject.SetActive(false);
            // Add ten to the variable score
			Scores.score = Scores.score + 10;
			// Run the SetHealthText() function
			SetScoreText ();
        }
        if(other.gameObject.CompareTag("Chest")) {
            // Destroy other object
            other.gameObject.SetActive(false);
            // Add one hundred to the variable score
			Scores.score = Scores.score + 100;
			// Run the SetHealthText() function
			SetScoreText ();
        }
        if(other.gameObject.CompareTag("Fruit")) {
            // If injured
            if(health < 100) {
                // Destroy other object
                other.gameObject.SetActive(false);
                // Add ten to the variable health
                health = health + 30;
                // Add 1 to hearts used value
                Scores.hearts = Scores.hearts + 1;
            }
			// Run the SetHealthText() function
			SetHealthText ();
        }
    }

    void SetHealthText()
	{
        // Max health is 100
        if(health >= 100) {
            health = 100;
        }

        // Display current health
		healthText.text = "Health: " + health.ToString();

        // When out of health destroy player
		if (health <= 0) 
		{
            // Locate and load scene called Death Scene
            SceneManager.LoadScene("Death Scene");
		}
	}

    void SetScoreText()
	{
        // Display current Scores.score
		scoreText.text = "Gold: " + Scores.score.ToString();

        // When Scores.score is more than 100
		if (Scores.score >= 10) 
		{
            // Locate and load scene called Scoreboard
            SceneManager.LoadScene("Scoreboard");
		}
	}
}
