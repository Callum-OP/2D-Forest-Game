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
	public class EnemyChase : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>

		[Header("Target")]
        // This is the target the enemy is going to move towards
        public Vector2 target;
        // This is the target the enemy is going to move towards
        public Transform player;
        // Attack distance from target
        private float distance; 

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
    
            target = player.position;

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
