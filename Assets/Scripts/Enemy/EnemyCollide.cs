using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollide : MonoBehaviour
{
    // When enemy collides with object
	private void OnTriggerEnter2D(Collider2D other)
    {
		// If hit by player projectile then enemy dies
        if(other.gameObject.CompareTag("Bullet")) {
			// Untag enemy so they can't deal damage
			gameObject.tag = "Untagged";
        }
    }
}
