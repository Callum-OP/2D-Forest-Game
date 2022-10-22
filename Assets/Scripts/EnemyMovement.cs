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

    // Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //rb equals the rigidbody on the enemy
        rb = GetComponent<Rigidbody2D>();

        
		currentTargetIndex = 0;

		newWaypoints = new Vector2[waypoints.Length+1];
		int w = 0;
		for(int i=0; i<waypoints.Length; i++)
		{
			newWaypoints[i] = waypoints[i];
			w = i;
		}

		//Add the starting position at the end, only if there is at least another point in the queue - otherwise it's on index 0
		int v = (newWaypoints.Length > 1) ? w+1 : 0;
		newWaypoints[v] = transform.position;
		//waypoints = newWaypoints;
	}

	// FixedUpdate is called once per frame
	void FixedUpdate ()
	{
		//do nothing if the target hasn't been assigned or it was detroyed for some reason
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
	}

    public void Reset()
	{
		waypoints = new Vector2[1];
		Vector2 thisPosition = transform.position;
		waypoints [0] = new Vector2 (2f, .5f) + thisPosition;
	}
}
