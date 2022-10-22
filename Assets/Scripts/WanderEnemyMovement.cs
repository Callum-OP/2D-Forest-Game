using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderEnemyMovement : MonoBehaviour
{
    //This is the target the object is going to move towards
	public Transform target;


    //Controls wandering velocity multiplier
    public float wanderSpeed = 0.1f;
    //Controls chasing velocity multiplier
    public float chaseSpeed = 1f;
    //Time between changing direction
    public float directionChangeInterval = 0.1f;
    //Distance from target that enemy will attack
    public float attackingDistance = 1; 
    //If enemy will stay nearby original position
	public bool keepNearStartingPoint = true;

    private Vector2 direction;
	private Vector3 startingPoint;

    //Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //rb equals the rigidbody on the enemy
        rb = GetComponent<Rigidbody2D>();

        //if target is far away enough
        if(Vector2.Distance(transform.position, target.position) > attackingDistance){
            //we don't want directionChangeInterval to be 0, so we force it to a minimum value
            if(directionChangeInterval < 0.1f)
            {
                directionChangeInterval = 0.1f;
            }
                
            // we note down the initial position of the GameObject in case it has to hover around that (see keepNearStartingPoint)
            startingPoint = transform.position;
		    
            StartCoroutine(ChangeDirection());
        }
    }

    // Calculates a new direction
	private IEnumerator ChangeDirection()
	{
		while(true)
		{
			direction = Random.insideUnitCircle; //change the direction the enemy is going

			// if we need to keep near the starting point...
			if(keepNearStartingPoint)
			{
				// we measure the attackingDistance from it...
				float distanceFromStart = Vector2.Distance(startingPoint, transform.position);
				if(distanceFromStart > 1f + (wanderSpeed * 0.1f)) // and if it's too much...
				{
					//... we get a direction that points back to the starting point
					direction = (startingPoint - transform.position).normalized;
				}
			}

			// this will make Unity wait for some time before continuing the execution of the code
			yield return new WaitForSeconds(directionChangeInterval);
		}
	}

	// FixedUpdate is called once per frame
	void FixedUpdate ()
	{
		//do nothing if the target hasn't been assigned or it was detroyed for some reason
		if(target == null)
			return;
		
        //if target is close enough
        if(Vector2.Distance(transform.position, target.position) <= attackingDistance){

            //Move towards the target
            rb.MovePosition(Vector2.Lerp(transform.position, target.position, Time.fixedDeltaTime * chaseSpeed));
        } 

        rb.AddForce(direction * wanderSpeed);
	}
}

