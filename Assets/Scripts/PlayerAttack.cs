using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    public float attackTime;
    public float startTimeAttack;

    public Transform attackLocation;
    public float attackRange;
    public LayerMask enemies;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if( attackTime <= 0 )
        {
            if( Input.GetButton("Jump"))
            {
                anim.SetBool("Is_attacking", true);
                Collider2D[] damage = Physics2D.OverlapCircleAll( attackLocation.position, attackRange, enemies );

                for (int i = 0; i < damage.Length; i++)
                {
                    Destroy( damage[i].gameObject );
                }
            }
            attackTime = startTimeAttack;
        }   else
        {
            attackTime -= Time.deltaTime;
            anim.SetBool("Is_attacking", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackLocation.position, attackRange);
    }
}