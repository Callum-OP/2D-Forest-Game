using UnityEngine;
using System.Collections;

public class Wander : MonoBehaviour
{
	[Header("Movement")]
	public float speed = 1f;
	public float directionChangeInterval = 3f;
	public bool keepNearStartingPoint = true;


	private Vector2 direction;
	private Vector3 startingPoint;

	//Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;


	// Start is called at the beginning of the game
	private void Start()
	{
		//rb equals the rigidbody on the enemy
        rb = GetComponent<Rigidbody2D>();

		//we don't want directionChangeInterval to be 0, so we force it to a minimum value ;)
		if(directionChangeInterval < 0.1f)
		{
			directionChangeInterval = 0.1f;
		}
			
		// we note down the initial position of the GameObject in case it has to hover around that (see keepNearStartingPoint)
		startingPoint = transform.position;

		StartCoroutine(ChangeDirection());
	}




	// Calculates a new direction
	private IEnumerator ChangeDirection()
	{
		while(true)
		{
			direction = Random.insideUnitCircle; //change the direction the player is going

			// if we need to keep near the starting point...
			if(keepNearStartingPoint)
			{
				// we measure the distance from it...
				float distanceFromStart = Vector2.Distance(startingPoint, transform.position);
				if(distanceFromStart > 1f + (speed * 0.1f)) // and if it's too much...
				{
					//... we get a direction that points back to the starting point
					direction = (startingPoint - transform.position).normalized;
				}
			}

			// this will make Unity wait for some time before continuing the execution of the code
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}



	// FixedUpdate is called every frame when the physics are calculated
	private void FixedUpdate()
	{
		rb.AddForce(direction * speed);
	}
}