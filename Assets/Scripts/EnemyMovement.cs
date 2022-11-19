using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Target")]
    // This is the target the enemy is going to move towards
	public Transform target;
    // Attack distance from target
    private float distance = 1; 

    [Header("Movement")]
    // Controls wandering velocity multiplier
	public float wanderSpeed = 0.5f;
    // Controls chasing velocity multiplier
	public float chaseSpeed = 1f;

	[Header("Stops")]
    // Location enemy will go to
	public Vector2[] waypoints;
	private Vector2[] newWaypoints;
	private int currentTargetIndex;

	[Header("Drops on death")]
    // The prefab to be used when enemy dies
    public GameObject prefabToSpawn;

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
        // Rb equals the rigidbody on the enemy
        rb = GetComponent<Rigidbody2D>();
        // Anim equals the animator on the player
        anim = this.GetComponent<Animator>();

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
		// Do nothing if the target hasn't been assigned or it was detroyed for some reason
		if(target == null)
			return;

        Vector2 currentTarget = newWaypoints[currentTargetIndex];
		
        // If player target is close enough
        if(Vector2.Distance(transform.position, target.position) <= distance){

            // Move towards the player target
            rb.MovePosition(Vector2.Lerp(transform.position, target.position, Time.fixedDeltaTime * chaseSpeed));
        } else {
            // Move towards the waypoint target
            rb.MovePosition(transform.position + ((Vector3)currentTarget - transform.position).normalized * wanderSpeed * Time.fixedDeltaTime);
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
        if(other.gameObject.CompareTag("Bullet")) {
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
            Destroy(other.gameObject);
			// Destroy enemy
            Destroy(gameObject, 1f);
			// Add 1 to enemies defeated
			Scores.defeats = Scores.defeats + 1;
			// Drop a coin on the ground where enemy died
            GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
			newObject.transform.position = this.transform.position;
        }
    }

    public void Reset()
	{
		waypoints = new Vector2[1];
		Vector2 thisPosition = transform.position;
		waypoints [0] = new Vector2 (2f, .5f) + thisPosition;
	}
}
