using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    // The source of audio sounds
    new AudioSource audio;

    void Start()
    {
        // Audio equals the audio source on the projectile
        audio = GetComponent<AudioSource>();
    }

    // When projectile hits objects with collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Play audio if enemy hit
        if (other.gameObject.CompareTag("Enemy") == true) {
                // Play audio
                audio.Play();
            }

        // Don't destroy projectile when player fires it
        if (other.gameObject.CompareTag("Player") == false) {
            // Don't destroy projectile if it hits any collectables
            if (other.gameObject.CompareTag("Coin") == false && other.gameObject.CompareTag("Fruit") == false && other.gameObject.CompareTag("Gem") == false) {
                // Destroy projectile if it hits any other object
                Destroy(gameObject, 0.1f);
            }
        }
    }
}
