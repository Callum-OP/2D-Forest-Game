using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{
    [Header("Opened Chest")]
    // The prefab to be used when enemy dies
    public GameObject prefabToSpawn;

    // When something collides with this object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) {
            // Spawn opened chest
			GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
			newObject.transform.position = this.transform.position;
        }
    }
}
