using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class EnemyMovement : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>

		[Header("Target")]
        // This is the target the enemy is going to move towards
        public Vector2 target;
        // This is the target the enemy is going to move towards
        public Transform player;
        // Attack distance from target
        private float distance; 

        [Header("Stops")]
        // Location enemy will go to
        public Vector2[] waypoints;
        private Vector2[] newWaypoints;
        private int currentTargetIndex;

        [Header("Components")]
        // A star pathfinding ai script
		IAstarAI ai;
        // Controls animation of enemy
        private Animator anim;

        [Header("Position")]
        // Store previous position of enemy
        Vector3 previousPosition;
        // Store last direcction of enemy
        Vector3 lastMoveDirection;

        void Start() {
            // Anim equals the animator on the enemy
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

		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update () {
			if (target != null && ai != null) ai.destination = target;
		}

        void FixedUpdate () {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            // Do nothing if the target hasn't been assigned or it was detroyed for some reason
            if(target == null)
                return;

            Vector2 currentTarget = newWaypoints[currentTargetIndex];
            
            // If player is too far away then don't move
            if (Vector2.Distance(transform.position, player.position) >= 50){
                target = this.transform.position;
            }
            // If player is not within range start moving between waypoints
            else if (Vector2.Distance(transform.position, player.position) >= 6){
                target = currentTarget;
            } else {
                target = player.position;
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
            // If hit by player projectile then enemy gets knocked back
            if (other.gameObject.CompareTag("Bullet")) {
                this.gameObject.AddComponent<Rigidbody2D>();
                Rigidbody2D rb = this.gameObject.GetComponent<Rigidbody2D>();
                Vector3 direction = (transform.position - player.position).normalized;
                rb.gravityScale = 0.0f;
                rb.AddForce(direction * 50);
            }
        }
	}
}
