using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    // Prefab to be used
    public GameObject prefabToSpawn;
    // Prefab to be used
    public GameObject rarePrefabToSpawn;

    // Record and count down the time
    private float WaveTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Have multiple waves that spawn randomly within a certain timeframe
        WaveTimer = ((Random.value + 1) * 5);
    }

    // Update is called once per frame
    void Update()
    {
        // When timer has counted down, begin the wave and reset timer with a random time within a certain timeframe
        WaveTimer -= Time.deltaTime;
        if (WaveTimer <= 0f)
        {
            if (Random.value > 0.05) {
                // Create the enemy and place it where spawn point is
                GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
                newObject.transform.position = this.transform.position;
            } else {
                // Create the enemy and place it where spawn point is
                GameObject newObject = Instantiate<GameObject>(rarePrefabToSpawn);
                newObject.transform.position = this.transform.position;
            }
            WaveTimer = ((Random.value + 2) * 10);
        }
    }
}
