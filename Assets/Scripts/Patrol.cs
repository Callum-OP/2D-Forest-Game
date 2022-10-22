using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Patrol : MonoBehaviour
{
	[Header("Movement")]
	public float speed = 5f;
	public float directionChangeInterval = 3f;

	[Header("Stops")]
	public Vector2[] waypoints;

	private Vector2[] newWaypoints;
	private int currentTargetIndex;

	// Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;

	void Start ()
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
	
	public void FixedUpdate ()
	{
		Vector2 currentTarget = newWaypoints[currentTargetIndex];

		rb.MovePosition(transform.position + ((Vector3)currentTarget - transform.position).normalized * speed * Time.fixedDeltaTime);

		if(Vector2.Distance(transform.position, currentTarget) <= .1f)
		{
			//new waypoint has been reached
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