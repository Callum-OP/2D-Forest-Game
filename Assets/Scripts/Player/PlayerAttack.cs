using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing the player and user interface
public class PlayerAttack : MonoBehaviour
{
    [Header("Player Settings")]
    // Controls animation of player
    Animator anim;
    
    // Tells script there is a rigidbody, we can use variable rb to reference it in further script
    Rigidbody2D rb;

    // The time that user must press second key within to double tap
    private float tapSpeed = 0.5f;

    [Header("Attack Settings")]
    // Controls being able to double tap space for ranged attack
    private float lastAttackTapTime = 0;
    private bool DoubleTapAttack;

    // Controls single tap melee attacks
    public float attackTime;
    public float startTimeAttack;
    public Transform attackLocation;
    public float attackRange;
    public LayerMask enemies;
    // Used to store time of when damage object last spawned
    private float timeOfLastAttack;

    [Header("Projectile creation")]
    // Bullet prefab to be used
    public GameObject prefabToSpawn;
    // The key to press to create the projectiles
    public KeyCode keyToPress = KeyCode.Space;

    // Used to store time of when projectile last spawned
    private float timeOfLastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        // --Player details--
        // Rb equals the rigidbody on the player
        rb = GetComponent<Rigidbody2D>();
        // Anim equals the animator on the player
        anim = this.GetComponent<Animator>();
        // Enemies layer mask equals the enemies layer
        enemies = LayerMask.GetMask("Enemies");
        // Melee attacks will originate from the player
        attackLocation = this.gameObject.transform;
        // The value representing how far melee attacks can go
        attackRange = 1.1f;
        // The value representing how long it takes before the attack will begin
        startTimeAttack = 0f;

        // Set time of last spawned projectile
        timeOfLastSpawn =- Player.creationRate;
    }

    // Update is called once per frame
    void Update()
    {
        // --Attack Functionality--
        // Value for deciding the direction to shoot
        Vector2 shootDirection = new Vector2(0f,0f);

        // Set direction of character weapon and projectile
        if (PlayerMovement.faceUp == true) {
            shootDirection = new Vector2(0, 1);
        } else if (PlayerMovement.faceDown == true) {
            shootDirection = new Vector2(0, -1);
        } else if (PlayerMovement.faceRightSide == true) {
            shootDirection = new Vector2(1, 0);
        } else if (PlayerMovement.faceLeftSide == true) {
            shootDirection = new Vector2(-1, 0);
        } else {
            shootDirection = new Vector2(0, -1);
        }

        // Detect double taps of key to press
        if(Input.GetKeyDown(keyToPress)){
            if((Time.time - lastAttackTapTime) < tapSpeed){
                DoubleTapAttack = true;
            } else {
                DoubleTapAttack = false;
            }
            lastAttackTapTime = Time.time;
        }

        // Ranged attacks that fire projectile in a direction
        if(Input.GetKey(keyToPress) && Time.time >= timeOfLastSpawn + Player.creationRate && DoubleTapAttack == true && Player.arrows > 0) {
            Debug.Log("Double tap");
            Vector2 actualBulletDirection = true ? (Vector2)(Quaternion.Euler(0, 0, transform.eulerAngles.z) * shootDirection) : shootDirection;
            // Create the projectile and place it where player is
            GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
            newObject.transform.position = this.transform.position;

            // Push the created projectile, but only if they have a Rigidbody2D
            Rigidbody2D rigidbody2D = newObject.GetComponent<Rigidbody2D>();
            if (rigidbody2D != null)
            {
                rigidbody2D.AddForce(actualBulletDirection * Player.shootSpeed, ForceMode2D.Impulse);
                newObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, actualBulletDirection);
            }
            // Set the time projectile was last spawned
            timeOfLastSpawn = Time.time;
            Player.arrows = Player.arrows - 1;
        } 

        if (attackTime <= 0) {
            attackTime = startTimeAttack;
        }

        // Melee attacks that damage nearby enemies
        if(Input.GetKey(keyToPress) && attackTime <= 0 && DoubleTapAttack == false) {
            Debug.Log("Single tap");
            anim.SetBool("Is_Attacking", true);
            Collider2D[] damage = Physics2D.OverlapCircleAll( attackLocation.position, attackRange, enemies );
            for (int i = 0; i < damage.Length; i++)
            {
                // Only damage enemy once every second
                if (Time.time >= timeOfLastAttack + 1) {
                    // Create the projectile and place it where player is
                    GameObject damageObject = Instantiate<GameObject>(prefabToSpawn);
                    damageObject.transform.position = damage[i].transform.position;
                    timeOfLastAttack = Time.time;
                }

            }
        } else {
            attackTime -= Time.deltaTime;
            anim.SetBool("Is_Attacking", false);
        }

        // --Attack Animation--
        // Set whether character is attacking or not
        // And give enough time for attack animation to play
        if (Input.GetKey(keyToPress) && Time.time <= timeOfLastSpawn + 0.3 || Input.GetKey(keyToPress) && attackTime <= 0) {
            anim.SetBool("Is_Attacking", true);
            if (DoubleTapAttack) {
                anim.SetFloat("Is_Ranged", 1);
            } else {
                anim.SetFloat("Is_Ranged", 0);
            }
        } else {
            anim.SetBool("Is_Attacking", false);
            anim.SetFloat("Is_Ranged", 0.5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}

