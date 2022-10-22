using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    //Controls velocity multiplier
    public float speed = 1f;
    //Controls the displayed health text
    public TextMeshProUGUI healthText;
        //Controls the displayed score text
    public TextMeshProUGUI scoreText;
    //Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;

    public Animator anim;
    public float hf = 0.0f;
    public float vf = 0.0f;

    //Controls health value
    private int health;
    //Controls score value
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player

        anim = this.GetComponent<Animator>();
        
        // Set the health value to one hundred
		health = 100;
        // Set the score value to zero
		score = 0;

		SetHealthText ();
		SetScoreText ();
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
        float zMove = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1
        rb.velocity = new Vector3(xMove, zMove) * speed; // Creates velocity in direction of value equal to keypress (WASD). rb.velocity.zMove deals with falling + jumping by setting velocity to zMove. 

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
			score = score + 10;
			// Run the SetHealthText() function
			SetScoreText ();
        }
        if(other.gameObject.CompareTag("Chest")) {
            // Destroy other object
            other.gameObject.SetActive(false);
            // Add ten to the variable score
			score = score + 100;
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
            Destroy(gameObject);
		}
	}

    void SetScoreText()
	{
        //Display current score
		scoreText.text = "Gold: " + score.ToString();
	}
}
