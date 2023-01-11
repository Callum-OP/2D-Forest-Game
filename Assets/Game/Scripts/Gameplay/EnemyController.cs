using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Target")]
    // This is the target the enemy is going to move towards
	public Transform target;
    // Attack distance from target
    private float distance; 

    [Header("Movement")]
    // Controls wandering velocity multiplier
	public float wanderSpeed;
    // Controls chasing velocity multiplier
	public float chaseSpeed;

	[Header("Stops")]
    // Location enemy will go to
	public Vector2[] waypoints;
	private Vector2[] newWaypoints;
	private int currentTargetIndex;

	[Header("Drops on death")]
    // The prefab to be used when enemy dies
    public GameObject prefabToSpawn;
	// The other prefab to be used when enemy dies
	public GameObject rarePrefabToSpawn;
	// Int used for probability of dropping rare prefab
	private double chanceToDrop;
	// Bool used to determine if items have been spawned yet
	private bool itemsDropped;

	// Controls animation of enemy
    private Animator anim;
    // Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;
	// The source of audio sounds
    new AudioSource audio;
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
		// Audio equals the audio source on the enemy
        audio = GetComponent<AudioSource>();
		// No items will have been dropped yet
		itemsDropped = false;

		// --Difficulty Settings--
		if (Settings.difficulty == 1) {
			// Set distance enemy will attack from to 0.5
    		distance = 0.5f; 
			// Set wandering velocity multiplier to 0.3
			wanderSpeed = 0.3f;
			// Set chasing velocity multiplier to 0.9
			chaseSpeed = 0.9f;
			// Set chance to drop rare item to 0.45 (45%)
			chanceToDrop = 0.45;
			//
		} else if (Settings.difficulty == 3) {
			// Set distance enemy will attack from to 0.9
    		distance = 0.9f; 
			// Set wandering velocity multiplier to 0.7
			wanderSpeed = 0.7f;
			// Set chasing velocity multiplier to 1.3
			chaseSpeed = 1.3f;
			// Set chance to drop rare item to 0.05 (5%)
			chanceToDrop = 0.05;
		} else {
			// Set distance enemy will attack from to 0.7
    		distance = 0.7f; 
			// Set wandering velocity multiplier to 0.5
			wanderSpeed = 0.5f;
			// Set chasing velocity multiplier to 1.1
			chaseSpeed = 1.1f;
			// Set chance to drop rare item to 0.2 (20%)
			chanceToDrop = 0.20;
		}

		// If in survival mode
		if (Settings.gameMode == "Survival") {
			distance = 1000f;
		}

		// Set the waypoints to be followed
		currentTargetIndex = 0;
		newWaypoints = new Vector2[waypoints.Length+1];
		int w = 0;
		for(int i=0; i<waypoints.Length; i++)
		{
			newWaypoints[i] = waypoints[i];
			w = i;
		}

		// Add the starting position at the end, only if there is at least another point in the queue - otherwise it's on index 0
		int v = (newWaypoints.Length > 1) ? w+1 : 0;
		newWaypoints[v] = transform.position;
		waypoints = newWaypoints;
		
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

        Vector2 currentTarget = newWaypoints[currentTargetIndex];
		
		// If player is within range start moving between waypoints
		// If player is closer and within attack distance then chase the player
        if(Vector2.Distance(transform.position, target.position) <= distance){
            // Move towards the player target
            rb.MovePosition(Vector2.Lerp(transform.position, target.position, Time.fixedDeltaTime * chaseSpeed));
			// Play the chase music
			audio.mute = false;
        } else if (Vector2.Distance(transform.position, target.position) <= 5){
            // Move towards the waypoint target
            rb.MovePosition(transform.position + ((Vector3)currentTarget - transform.position).normalized * wanderSpeed * Time.fixedDeltaTime);
			// Stop the chase music
			audio.mute = true;
        }

        // If waypoint target is close enough
		if(Vector2.Distance(transform.position, currentTarget) <= .1f)
		{
			// New waypoint has been reached
			currentTargetIndex = (currentTargetIndex<newWaypoints.Length-1) ? currentTargetIndex +1 : 0;
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
			audio.Stop();
			// Untag enemy so they can't deal damage
			gameObject.tag = "Untagged";
			// Stop moving
			chaseSpeed = 0f;
			wanderSpeed = 0f;
			// Stop other animations
			anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", false);
			// Play death animation
			anim.SetBool("Is_Dead", true);
			// Destroy projectile
            Destroy(other.gameObject, 0.1f);
			// Destroy enemy
            Destroy(gameObject, 0.5f);
			// Add 1 to enemies defeated
			Scores.defeats = Scores.defeats + 1;
			if (itemsDropped == false) {
				// Drop items spread out on the ground where enemy died
				if(Random.value > chanceToDrop) { 
					// High percent chance
					// Spawn item
					GameObject newObject1 = Instantiate<GameObject>(prefabToSpawn);
					newObject1.transform.position = new Vector3(this.transform.position.x - 0.03f, this.transform.position.y - 0.04f, this.transform.position.z);
					// Spawn item
					GameObject newObject2 = Instantiate<GameObject>(prefabToSpawn);
					newObject2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.04f, this.transform.position.z);
					// Spawn item
					GameObject newObject3 = Instantiate<GameObject>(prefabToSpawn);
					newObject3.transform.position = new Vector3(this.transform.position.x + 0.03f, this.transform.position.y - 0.04f, this.transform.position.z);
					// Spawn item
					GameObject newObject4 = Instantiate<GameObject>(prefabToSpawn);
					newObject4.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
					// Spawn item
					GameObject newObject5 = Instantiate<GameObject>(prefabToSpawn);
					newObject5.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.06f, this.transform.position.z);
					itemsDropped = true;
				} else {
					// Low percent chance
					// Spawn item
					GameObject newObject1 = Instantiate<GameObject>(prefabToSpawn);
					newObject1.transform.position = new Vector3(this.transform.position.x - 0.03f, this.transform.position.y - 0.04f, this.transform.position.z);
					// Spawn item
					GameObject newObject2 = Instantiate<GameObject>(prefabToSpawn);
					newObject2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.04f, this.transform.position.z);
					// Spawn item
					GameObject newObject3 = Instantiate<GameObject>(prefabToSpawn);
					newObject3.transform.position = new Vector3(this.transform.position.x + 0.03f, this.transform.position.y - 0.04f, this.transform.position.z);
					// Spawn item
					GameObject newObject4 = Instantiate<GameObject>(prefabToSpawn);
					newObject4.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
					// Spawn item
					GameObject newObject5 = Instantiate<GameObject>(prefabToSpawn);
					newObject5.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.06f, this.transform.position.z);
					// Spawn rare item
					GameObject rareObject = Instantiate<GameObject>(rarePrefabToSpawn);
					rareObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.04f, this.transform.position.z);
					itemsDropped = true;
				}
			}
        }
    }

    public void Reset()
	{
		waypoints = new Vector2[1];
		Vector2 thisPosition = transform.position;
		waypoints [0] = new Vector2 (2f, .5f) + thisPosition;
	}
}
