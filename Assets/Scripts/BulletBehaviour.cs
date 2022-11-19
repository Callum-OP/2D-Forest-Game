using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    // When projectile hits objects with collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Don't destroy projectile when player fires it
        if (other.gameObject.CompareTag("Player") == false) {
            // Don't destroy projectile if it hits any collectables
            if (other.gameObject.CompareTag("Coin") == false && other.gameObject.CompareTag("Fruit") == false && other.gameObject.CompareTag("Gem") == false) {
                // Destroy projectile if it hits any other object
                Destroy(gameObject);
            }
        }
    }
}
