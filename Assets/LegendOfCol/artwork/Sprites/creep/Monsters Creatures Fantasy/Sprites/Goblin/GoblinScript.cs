using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinScript : MonoBehaviour
{
    public Animator animator;
    public int facingRight = 1;
    public float speed;
    public float distance;
    public Rigidbody2D unit_body;
    public float startPosition;
    public detectPlayer autoDetect;
    public detectPlayer AttackZone;

    public int attackDamage = 15;
    public Transform attackPoint;
    public Transform attackPoint2;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;
    void Start()
    {
        unit_body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            if (autoDetect.playerPosition != null)
            {
                float playerDirection = (float)autoDetect.playerPosition - transform.position.x;
                if (playerDirection < 0)
                {
                    if (facingRight != -1)
                    {
                        flip();
                    }
                }
                else
                {
                    if (facingRight != 1)
                    {
                        flip();
                    }
                }
                //if (System.Math.Abs(playerDirection) < 2.5)
                //{
                //    if ((facingRight == -1 && playerDirection < 0) || (facingRight == 1 && playerDirection > 0))
                //    {
                //        int isAttack2 = Random.Range(1, 5);
                //        //Debug.Log(isAttack2.ToString());
                //        if(isAttack2 == 4)
                //            animator.SetTrigger("Attack2");
                //        else
                //            animator.SetTrigger("Attack1");
                //    }
                //}
            }
            if(AttackZone.playerPosition != null)
            {
                int isAttack2 = Random.Range(1, 5);
                //Debug.Log(isAttack2.ToString());
                if (isAttack2 == 4)
                    animator.SetTrigger("Attack2");
                else
                    animator.SetTrigger("Attack1");
            }
            if ((transform.position.x - startPosition >= distance && facingRight == 1)
                || (transform.position.x - startPosition <= -distance && facingRight == -1))
            {
                flip();
            }
            unit_body.velocity = new Vector2(speed * Time.deltaTime * facingRight, 0);
            animator.SetFloat("Speed", 1);
        }
    }
    void flip()
    {
        facingRight *= -1;
        Vector3 a = transform.localScale;
        a.x = a.x * -1;
        transform.localScale = a;
    }

    void attack()
    {
        SoundEffectManager.playSound("GoblinAttack");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Bandit>().TakeDamage(attackDamage, transform.localScale.x > 0 ? 1 : -1,1);
            }
        }
        else
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, playerLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Bandit>().TakeDamage(attackDamage, transform.localScale.x > 0 ? 1 : -1,1);
            }
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        if (attackPoint2 == null)
            return;
        Gizmos.DrawWireSphere(attackPoint2.position, attackRange);
    }
}
