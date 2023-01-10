using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDroneController : MonoBehaviour
{
    [Header("Target")]
    // This is the target the enemy is going to move towards
	public Transform target;

    [Header("Movement")]
    // Controls chasing velocity multiplier
	public float chaseSpeed;

	[Header("Drops on death")]
	// The other prefab to be used when enemy dies
	public GameObject rarePrefabToSpawn;
	// Int used for probability of dropping rare prefab
	private double chanceToDrop;

	// Controls animation of player
    private Animator anim;
    // Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;
    // Store previous position of enemy
	Vector3 previousPosition;
	// Store last direcction of enemy
    Vector3 lastMoveDirection;

    // Start is called before the first frame update
    void Start()
    {
		// --Enemy Details--
        // Rb equals the rigidbody on the enemy
        rb = GetComponent<Rigidbody2D>();
        // Anim equals the animator on the enemy
        anim = this.GetComponent<Animator>();

		// --Difficulty Settings--
		if (Settings.difficulty == 1) { 
			// Set chasing velocity multiplier to 0.45
			chaseSpeed = 0.45f;
			// Set chance to drop rare item to 0.45 (45%)
			chanceToDrop = 0.45;
			//
		} else if (Settings.difficulty == 3) { 
			// Set chasing velocity multiplier to 0.65
			chaseSpeed = 0.65f;
			// Set chance to drop rare item to 0.05 (5%)
			chanceToDrop = 0.05;
		} else {
			// Set chasing velocity multiplier to 0.55
			chaseSpeed = 0.55f;
			// Set chance to drop rare item to 0.2 (20%)
			chanceToDrop = 0.20;
		}

        // Set the last place and direction of enemy
		previousPosition = transform.position;
        lastMoveDirection = Vector3.zero;
	}

	// FixedUpdate is called once per frame
	void FixedUpdate ()
	{
		target = GameObject.FindWithTag("Player").GetComponent<Transform>();
		// Do nothing if the target hasn't been assigned or it was detroyed for some reason
		if(target == null)
			return;

        // If player is within range then chase the player
        if (Vector2.Distance(transform.position, target.position) <= 5){
            rb.MovePosition(Vector2.Lerp(transform.position, target.position, Time.fixedDeltaTime * chaseSpeed));
        }

		// Work out direction enemy is moving
		if(transform.position != previousPosition) {
            lastMoveDirection = (transform.position - previousPosition).normalized;
            previousPosition = transform.position;
		}

		// Set which direction enemy should be facing
		if (lastMoveDirection.x < lastMoveDirection.y && lastMoveDirection.x < lastMoveDirection.y) {
			anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", true);
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
		} 
		if (lastMoveDirection.x > lastMoveDirection.y && lastMoveDirection.x > -lastMoveDirection.y) {
			anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", true);
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
		} 
		if (lastMoveDirection.y < lastMoveDirection.x && lastMoveDirection.y < -lastMoveDirection.x) {
			anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", true);
            anim.SetBool("Face_Side", false);
		} 
		if (lastMoveDirection.y > lastMoveDirection.x && lastMoveDirection.y > -lastMoveDirection.x) {
			anim.SetBool("Face_Up", true);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", false);
		}
	}

	// When enemy collides with object
	private void OnTriggerEnter2D(Collider2D other)
    {
		// If hit by player projectile then enemy dies
        if (other.gameObject.CompareTag("Bullet")) {
			// Stop the chase music
			GetComponent<AudioSource>().Stop();
			// Untag enemy so they can't deal damage
			gameObject.tag = "Untagged";
			// Stop moving
			chaseSpeed = 0f;
			// Stop other animations
			anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", false);
			// Play death animation
			anim.SetBool("Is_Dead", true);
			// Destroy projectile
            Destroy(other.gameObject, 0f);
			// Destroy enemy
            Destroy(gameObject, 0.5f);
			// Add 1 to enemies defeated
			Scores.defeats = Scores.defeats + 1;
			// Low precent chance to drop rare item on the ground where enemy died
			 if(Random.value < chanceToDrop) { 
				// Spawn rare item
				GameObject rareObject = Instantiate<GameObject>(rarePrefabToSpawn);
				rareObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.04f, this.transform.position.z);
			}
        }
    }
}

