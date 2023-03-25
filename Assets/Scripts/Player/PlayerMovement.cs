using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for storing details about the players movement direction
public class PlayerDirection {

}

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    // Used to reference the body of the player
    Rigidbody2D rb;
    // Used to reference the animator of the player
    Animator anim;

    [Header("Movement Settings")]
    // Used to store input values
    float horizontal;
    float vertical;
    // Used to store movement values
    float hf;
    float vf;
    // Used to limit speed to 70% when diagonal
    float moveLimiter = 0.7f;
    // Speed player will be moving at
    float speed = 10.0f;

    [Header("Direction Settings")]
    // Values for what direction player should be facing
    public static bool faceUp = false;
    public static bool faceDown = false;
    public static bool faceLeftSide = false;
    public static bool faceRightSide = false;

    void Start ()
    {
        // Rb equals the rigidbody on the player
        rb = GetComponent<Rigidbody2D>();
        // Anim equals the rigidbody on the player
        anim = GetComponent<Animator>();

        // Set player speed
        speed = Player.speed;
    }

    void Update()
    {
        // Gives a value between -1 and 1 from input
        // For horizontal -1 is left and 1 is right
        horizontal = Input.GetAxisRaw("Horizontal");
        // For vertical -1 is down and 1 is up
        vertical = Input.GetAxisRaw("Vertical");

        // --Movement Animation--
        // Set which direction the character should be facing
        // And set animation value for if moving sideways
        if (horizontal == 1) {
            faceUp = false;
            faceDown = false;
            faceLeftSide = false;
            faceRightSide = true;
            anim.SetBool("Is_Moving_Side", true);
        } else if (horizontal == -1) {
            faceUp = false;
            faceDown = false;
            faceLeftSide = true;
            faceRightSide = false;
            anim.SetBool("Is_Moving_Side", true);
        } else {
            anim.SetBool("Is_Moving_Side", false);
        }
        if (vertical == 1) {
            faceUp = true;
            faceDown = false;
            faceLeftSide = false;
            faceRightSide = false;
        } else if (vertical == -1) {
            faceUp = false;
            faceDown = true;
            faceLeftSide = false;
            faceRightSide = false;
        }

        // Set direction of character animation
        if (faceUp == true) {
            anim.SetBool("Face_Up", true);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", false);
            anim.SetFloat("VerticalDirection", 1);
        } else if (faceDown == true) {
            anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", true);
            anim.SetBool("Face_Side", false);

            anim.SetFloat("VerticalDirection", -1);
        } else if (faceRightSide == true) {
            anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", true);
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        } else if (faceLeftSide == true) {
            anim.SetBool("Face_Up", false);
            anim.SetBool("Face_Down", false);
            anim.SetBool("Face_Side", true);
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }

        // Stores 1 if character is moving in that direction (vertical or horizontal)
        hf = horizontal > 0.01f ? horizontal : horizontal < -0.01f ? 1 : 0;
        vf = vertical > 0.01f ? vertical : vertical < -0.01f ? 1 : 0;

        // Set values for whether character is moving in a direction
        anim.SetFloat("Horizontal", hf);
        anim.SetFloat("Vertical", vf);
    }

    void FixedUpdate()
    {
        // If moving diagonally
    if (horizontal != 0 && vertical != 0)
    {
        // Limit the movement speed
        horizontal *= moveLimiter;
        vertical *= moveLimiter;
    } 
    rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}
