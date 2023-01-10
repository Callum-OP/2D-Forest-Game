using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    [Header("Target")]
    // This is the target the enemy is going to move towards
	public Transform target;
    // Attack distance from target
    private float distance; 

    [Header("Movement")]
    // Controls wandering velocity multiplier
	public float wanderSpeed;

	[Header("Stops")]
    // Location enemy will go to
	public Vector2[] waypoints;
	private Vector2[] newWaypoints;
	private int currentTargetIndex;

	[Header("Drops on death")]
    // The prefab to be used when enemy dies
    public GameObject prefabToSpawn;
    // Bool used to determine if items have been spawned yet
	private bool itemsDropped;

    [Header("Projectile options")]
    // The prefab to be used when enemy shoots
    public GameObject bulletToSpawn;
    // The time it takes to create projectile so long as the key is pressed
    public float creationRate;
    // The speed the projectile will travel at
    public float shootSpeed;
    // Used to store time of when projectile last spawned
    private float timeOfLastSpawn;
    // The time it takes to create boss minion so long as the key is pressed
    public float droneCreationRate;
    // Used to store time of when boss minion last spawned
    private float timeOfLastDroneSpawn;
    // Object used to spawn bullets
    private GameObject bulletObject1;
    // Object used to spawn bullets
    private GameObject bulletObject2;
    // Object used to spawn bullets
    private GameObject bulletObject3;

    [Header("Minion enemy options")]
    // The prefab to be used when enemy shoots
    public GameObject enemyToSpawn;

	// Controls animation of player
    private Animator anim;
    // Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;
    // Tells script there is a sprite renderer
    SpriteRenderer sr;
	// The source of audio sounds
    new AudioSource audio;
	// Store previous position of enemy
	Vector3 previousPosition;
	// Store last direcction of enemy
    Vector3 lastMoveDirection;

    private int hit = 0;

    // Start is called before the first frame update
    void Start()
    {
		// --Enemy Details--
        // Rb equals the rigidbody on the enemy
        rb = GetComponent<Rigidbody2D>();
        // Sr equals the sprite renderer on the enemy
        sr = GetComponent<SpriteRenderer>();
        // Anim equals the animator on the enemy
        anim = this.GetComponent<Animator>();
		// Audio equals the audio source on the enemy
        audio = GetComponent<AudioSource>();
        // Set time of last spawned projectile
        timeOfLastSpawn =- creationRate;
        // Set time of last spawned projectile
        timeOfLastDroneSpawn =- droneCreationRate;
        // No items will have been dropped yet
		itemsDropped = false;

		// --Difficulty Settings--
		if (Settings.difficulty == 1) {
			// Set distance enemy will attack from to 5
    		distance = 5f; 
			// Set wandering velocity multiplier to 1
			wanderSpeed = 1f;
            // Set the time it takes to create projectile to 1.5
            creationRate = 1.5f;
            // Set the time it takes to create minion to 8
            droneCreationRate = 7f;
            //Set the speed projectile will travel at to 0.5
            shootSpeed = 0.5f;
			//
		} else if (Settings.difficulty == 3) {
			// Set distance enemy will attack from to 5
    		distance = 5f; 
			// Set wandering velocity multiplier to 2
			wanderSpeed = 2f;
            // Set the time it takes to create projectile to 0.5
            creationRate = 0.5f;
            // Set the time it takes to create minion to 6
            droneCreationRate = 6f;
            //Set the speed projectile will travel at to 0.9
            shootSpeed = 0.9f;
		} else {
			// Set distance enemy will attack from to 5
    		distance = 5f; 
			// Set wandering velocity multiplier to 1.5
			wanderSpeed = 1.5f;
            // Set the time it takes to create projectile to 1
            creationRate = 1f;
            // Set the time it takes to create minion to 7
            droneCreationRate = 6.5f;
            //Set the speed projectile will travel at to 0.7
            shootSpeed = 0.7f;
		}

        // If in survival mode
		if (Settings.gameMode == "Survival") {
			distance = 1000f;
		}

		// Set the waypoints to be followed
		currentTargetIndex = 0;
		newWaypoints = new Vector2[waypoints.Length+1];
		int w = 0;
		for (int i=0; i<waypoints.Length; i++)
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
		if (target == null)
			return;

        Vector2 currentTarget = newWaypoints[currentTargetIndex];
		
        // If player is within range start moving between waypoints
        if (Vector2.Distance(transform.position, target.position) <= distance)
        {
            // Move towards the waypoint target
            rb.MovePosition(transform.position + ((Vector3)currentTarget - transform.position).normalized * wanderSpeed * Time.fixedDeltaTime);
			// Play the chase music
			audio.mute = false;
        } else {
            // Stop the chase music
			audio.mute = true;
        }

        // If waypoint target is close enough
		if (Vector2.Distance(transform.position, currentTarget) <= .1f)
		{
			// New waypoint has been reached
			currentTargetIndex = (currentTargetIndex<newWaypoints.Length-1) ? currentTargetIndex +1 : 0;
		}

		// Work out direction enemy is moving
		if (transform.position != previousPosition) {
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

        // Value for deciding the direction to shoot
        Vector2 shootDirection = new Vector2(0f,-1f);

        // Fire projectiles in a direction when player is close enough
        if(Vector2.Distance(transform.position, target.position) <= distance && Time.time >= timeOfLastSpawn + creationRate) {
            Vector2 actualBulletDirection = true ? (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * shootDirection) : shootDirection;
                // Create the projectiles and place it where enemy boss is
                bulletObject1 = Instantiate<GameObject>(bulletToSpawn);
                bulletObject1.transform.position = this.transform.position;
                bulletObject2 = Instantiate<GameObject>(bulletToSpawn);
                bulletObject2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.1f, this.transform.position.z);
                bulletObject3 = Instantiate<GameObject>(bulletToSpawn);
                bulletObject3.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.1f, this.transform.position.z);

                // Push the created projectiles, but only if they have a Rigidbody2D
                Rigidbody2D rigidbody2D = bulletObject1.GetComponent<Rigidbody2D>();
                if (rigidbody2D != null)
                {
                    rigidbody2D.AddForce(actualBulletDirection * shootSpeed, ForceMode2D.Impulse);
                    bulletObject1.transform.rotation = Quaternion.LookRotation(Vector3.forward, actualBulletDirection);
                }
                rigidbody2D = bulletObject2.GetComponent<Rigidbody2D>();
                if (rigidbody2D != null)
                {
                    rigidbody2D.AddForce(actualBulletDirection * shootSpeed, ForceMode2D.Impulse);
                    bulletObject2.transform.rotation = Quaternion.LookRotation(Vector3.forward, actualBulletDirection);
                }
                rigidbody2D = bulletObject3.GetComponent<Rigidbody2D>();
                if (rigidbody2D != null)
                {
                    rigidbody2D.AddForce(actualBulletDirection * shootSpeed, ForceMode2D.Impulse);
                    bulletObject3.transform.rotation = Quaternion.LookRotation(Vector3.forward, actualBulletDirection);
                }
                // Set the time projectiles were last spawned
                timeOfLastSpawn = Time.time;
                // Destroy projectiles if they have been spawned for too long
                Destroy(bulletObject1, 5f);
                Destroy(bulletObject2, 5f);
                Destroy(bulletObject3, 5f);
        }

        // Fire projectile in a direction when player is close enough
        if(Vector2.Distance(transform.position, target.position) <= distance && Time.time >= timeOfLastDroneSpawn + droneCreationRate) {
            Vector2 actualBulletDirection = true ? (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * shootDirection) : shootDirection;
                // Create the projectile and place it where enemy boss is
                GameObject enemyObject = Instantiate<GameObject>(enemyToSpawn);
                enemyObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.25f, this.transform.position.z);
                // Set the time projectile was last spawned
                timeOfLastDroneSpawn = Time.time;
        }
	}

	// When enemy collides with object
	private void OnTriggerEnter2D(Collider2D other)
    {
		// If hit by player projectile then enemy dies
        if (other.gameObject.CompareTag("Bullet")) {
            // Run Injured() coroutine
            StartCoroutine(Injured());
            // Add to number of times enemy boss has been hit
            hit++;
            if (hit >= 18) {
                // Stop the chase music
                audio.Stop();
                // Untag enemy so they can't deal damage
                gameObject.tag = "Untagged";
                // Stop moving
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
                    // Spawn item
                    GameObject newObject1 = Instantiate<GameObject>(prefabToSpawn);
                    newObject1.transform.position = new Vector3(this.transform.position.x - 0.04f, this.transform.position.y - 0.04f, this.transform.position.z);
                    // Spawn item
                    GameObject newObject2 = Instantiate<GameObject>(prefabToSpawn);
                    newObject2.transform.position = new Vector3(this.transform.position.x + 0.04f, this.transform.position.y - 0.04f, this.transform.position.z);
                    itemsDropped = true;
                }
            }
        }
    }

    // Used to indicate to user that enemy boss has taken damage
    IEnumerator Injured()
    {
        // Make enemy look injured by turning them red and playing animation
        sr.color = Color.red;
        anim.SetBool("Is_Hurt", true);
        yield return new WaitForSeconds(0.2f);
        // Make enemy look normal again
        sr.color = Color.white;
        anim.SetBool("Is_Hurt", false);
    }

    public void Reset()
	{
		waypoints = new Vector2[1];
		Vector2 thisPosition = transform.position;
		waypoints [0] = new Vector2 (2f, .5f) + thisPosition;
	}
}
