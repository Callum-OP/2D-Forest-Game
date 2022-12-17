using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    // Prefab to be used
    public GameObject prefabToSpawn;

    // Record and count down the time
    private float Wave1Timer;
    private float Wave2Timer;
    private float Wave3Timer;

    // Start is called before the first frame update
    void Start()
    {
        // Have multiple waves that spawn randomly within a certain timeframe
        Wave1Timer = ((Random.value + 1) * 5);
        Wave2Timer = ((Random.value + 5) * 10);
        Wave3Timer = ((Random.value + 10) * 10);
    }

    // Update is called once per frame
    void Update()
    {
        // When timer has counted down, begin the wave and reset timer with a random time within a certain timeframe
        Wave1Timer -= Time.deltaTime;
        if (Wave1Timer <= 0f)
        {
            // Create the enemy and place it where spawn point is
            GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
            newObject.transform.position = this.transform.position;
            Wave1Timer = ((Random.value + 1) * 10);
        }
        Wave2Timer -= Time.deltaTime;
        if (Wave2Timer <= 0f)
        {
            // Create the enemy and place it where spawn point is
            GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
            newObject.transform.position = this.transform.position;
            Wave2Timer = ((Random.value + 5) * 10);
        }
        Wave3Timer -= Time.deltaTime;
        if (Wave3Timer <= 0f)
        {
            // Create the enemy and place it where spawn point is
            GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
            newObject.transform.position = this.transform.position;
            Wave3Timer = ((Random.value + 10) * 10);
        }
    }
}
