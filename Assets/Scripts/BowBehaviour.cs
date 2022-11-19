using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowBehaviour : MonoBehaviour
{
    [Header("Projectile creation")]
    // Bullet prefab to be used
    public GameObject prefabToSpawn;
    // The key to press to create the projectiles
    public KeyCode keyToPress = KeyCode.Space;

    [Header("Projectile options")]
    // The rate of creation, as long as the key is pressed
    public float creationRate = .5f;
    // The speed at which the object is shot
    public float shootSpeed = 1.5f;
    // Used to store time of when projectile last spawned
    private float timeOfLastSpawn;
    // Used to know if player is shooting or not
    private bool shoot;

    // Use this for initialization
    void Start()
    {
        // Player is not shooting yet
        shoot = false;
        // Set time of last spawned projectile
        timeOfLastSpawn = -creationRate;
    }

    // Update is called once per frame
    void Update()
    {
        // Value for deciding the direction to shoot
        Vector2 shootDirection = new Vector2(0f,0f);

        //if (Movement.movingHorizontal == true) {
            //shootDirection = new Vector2(Movement.hSpeed, 0);
        //} else if (Movement.movingVertical == true) {
            //shootDirection = new Vector2(0, Movement.vSpeed);
        //} else {
            //shootDirection = new Vector2(0, -1);
        //}

        if ((shoot == true && Time.time >= timeOfLastSpawn + creationRate) || (Input.GetKey(keyToPress) && Time.time >= timeOfLastSpawn + creationRate))
        {
            Vector2 actualBulletDirection = true ? (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * shootDirection) : shootDirection;

            GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
            newObject.transform.position = this.transform.position;
            //newObject.transform.eulerAngles = new Vector3(0f, 0f, Utils.Angle(actualBulletDirection));

            // push the created objects, but only if they have a Rigidbody2D
            Rigidbody2D rigidbody2D = newObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.AddForce(actualBulletDirection * shootSpeed, ForceMode2D.Impulse);
                newObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, actualBulletDirection);
            }

            timeOfLastSpawn = Time.time;
        }
    }
}
