using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [Header("Drops on death")]
    // The prefab to be used when enemy dies
    public GameObject prefabToSpawn;
	// The other prefab to be used when enemy dies
	public GameObject rarePrefabToSpawn;
	// Int used for probability of dropping rare prefab
	private double chanceToDrop;
	// Bool used to determine if items have been spawned yet
	private bool itemsDropped;

    [Header("Components")]    
    // Controls animation of enemy
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Anim equals the animator on the enemy
        anim = this.GetComponent<Animator>();

        // No items will have been dropped yet
		itemsDropped = false;
        // Set chance to drop rare item to 0.2 (20%)
		chanceToDrop = 0.20;
    }

    // When enemy collides with object
	private void OnTriggerEnter2D(Collider2D other)
    {
		// If hit by player projectile then enemy dies
        if (other.gameObject.CompareTag("Bullet")) {
			// Untag enemy so they can't deal damage
			gameObject.tag = "Untagged";

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
					newObject1.transform.position = new Vector3(this.transform.position.x - 0.15f, this.transform.position.y - 0.2f, this.transform.position.z);
					// Spawn item
					GameObject newObject2 = Instantiate<GameObject>(prefabToSpawn);
					newObject2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.2f, this.transform.position.z);
					// Spawn item
					GameObject newObject3 = Instantiate<GameObject>(prefabToSpawn);
					newObject3.transform.position = new Vector3(this.transform.position.x + 0.15f, this.transform.position.y - 0.2f, this.transform.position.z);
					// Spawn item
					GameObject newObject4 = Instantiate<GameObject>(prefabToSpawn);
					newObject4.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
					// Spawn item
					GameObject newObject5 = Instantiate<GameObject>(prefabToSpawn);
					newObject5.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.3f, this.transform.position.z);
					itemsDropped = true;
				} else {
					// Low percent chance
					// Spawn item
					GameObject newObject1 = Instantiate<GameObject>(prefabToSpawn);
					newObject1.transform.position = new Vector3(this.transform.position.x - 0.15f, this.transform.position.y - 0.2f, this.transform.position.z);
					// Spawn item
					GameObject newObject2 = Instantiate<GameObject>(prefabToSpawn);
					newObject2.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.2f, this.transform.position.z);
					// Spawn item
					GameObject newObject3 = Instantiate<GameObject>(prefabToSpawn);
					newObject3.transform.position = new Vector3(this.transform.position.x + 0.15f, this.transform.position.y - 0.2f, this.transform.position.z);
					// Spawn item
					GameObject newObject4 = Instantiate<GameObject>(prefabToSpawn);
					newObject4.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
					// Spawn item
					GameObject newObject5 = Instantiate<GameObject>(prefabToSpawn);
					newObject5.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.3f, this.transform.position.z);
					// Spawn rare item
					GameObject rareObject = Instantiate<GameObject>(rarePrefabToSpawn);
					rareObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.2f, this.transform.position.z);
					itemsDropped = true;
				}
			}
        }
    }
}
