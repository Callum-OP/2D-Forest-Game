using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableAnimation : MonoBehaviour
{
    [Header("Target")]
    // This is the target the enemy is going to move towards
	public Transform target;

    // Controls animation of item
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        // Anim equals the animator on the enemy
        anim = this.GetComponent<Animator>();

        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
		// Do nothing if the target hasn't been assigned or it was detroyed for some reason
		if(target == null)
			return;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) >= 5){
            anim.enabled = false;
        } else {
            anim.enabled = true;
        }
    }
}
