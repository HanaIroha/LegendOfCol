using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SkeletonBoss1 : MonoBehaviour
{
    public Animator animator;
    public int facingRight = 1;
    public float speed;
    public float distance;
    public Rigidbody2D unit_body;
    public float startPosition;
    public detectPlayer autoDetect;
    public detectPlayer autoAirDetect;
    public detectPlayer AttackZone;

    public int attackDamage = 15;
    public Transform attackPoint;
    public Transform attackPoint2;
    public float attackRange = 0.5f;
    public LayerMask playerLayers;

    private float period = 0.0f;
    private bool isCombo = false;
    private float comboTime = 0.0f;
    private int comboType;
    private int comboStep = 1;
    private float randomComboTime;
    private float playerDirection = 0;
    private bool bossAlert = false;
    private float bossRandomAttackTime = 0;

    public GameObject smoke;
    public GameObject creep;

    void Start()
    {
        unit_body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPosition = transform.position.x;
        randomComboTime = Random.Range(5f, 8f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (autoDetect.playerPosition != null)
        {
            playerDirection = (float)autoDetect.playerPosition - transform.position.x;
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
            //if (System.Math.Abs(playerDirection) < 2)
            //{
            //    if ((facingRight == -1 && playerDirection < 0) || (facingRight == 1 && playerDirection > 0))
            //    {
            //        int isShield = Random.Range(1, 3);
            //        //Debug.Log(isAttack2.ToString());
            //        if (isShield == 2)
            //            animator.SetTrigger("Shield");
            //        else
            //            animator.SetTrigger("Attack1");
            //    }
            //}
        }
        if (!isCombo)
        {
            if (period > randomComboTime)
            {
                randomComboTime = Random.Range(4f, 7.5f);
                comboType = Random.Range(1, 4);
                //comboType = 3;
                period = 0;
                isCombo = true;
                bossAlert = false;
                comboStep = 1;
                comboTime = 0;
            }
            else if (CrossPlatformInputManager.GetButton("Attack") && System.Math.Abs(playerDirection) < 1f)
            {
                int dodgeChange = Random.Range(1, 4);
                if (dodgeChange == 1)
                {
                    comboType = 6;
                    period = 0;
                    isCombo = true;
                    comboStep = 1;
                    comboTime = 0;
                }
            }
            period += UnityEngine.Time.deltaTime;
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                if (autoAirDetect.playerAirPosition != null)
                {
                    int isBlockChoice = Random.Range(1, 6);
                    if (isBlockChoice != 1)
                        animator.SetTrigger("Attack2");
                    else
                        animator.SetTrigger("Shield");
                    autoAirDetect.playerAirPosition = null;
                }
                else if (AttackZone.playerPosition != null)
                {
                    int isShield = Random.Range(1, 3);
                    int isAirAttack = Random.Range(1, 5);
                    //Debug.Log(isAttack2.ToString());
                    //isShield = 2;
                    if (isShield == 2)
                        animator.SetTrigger("Shield");
                    else
                    {
                        if(isAirAttack==4)
                            animator.SetTrigger("Attack2");
                        else
                            animator.SetTrigger("Attack1");
                    }
                }
                //if ((transform.position.x - startPosition >= distance && facingRight == 1)
                //    || (transform.position.x - startPosition <= -distance && facingRight == -1))
                //{
                //    flip();
                //}
                unit_body.velocity = new Vector2(speed * Time.deltaTime * facingRight, 0);
                animator.SetFloat("Speed", 1);
            }
        }
        else
        {
            if (comboType == 1)
            {
                if (comboStep == 1)
                {
                    if(System.Math.Abs(playerDirection) <= 6)
                        unit_body.velocity = new Vector2(200 * Time.deltaTime * facingRight * -1, 0);
                    if (System.Math.Abs(playerDirection) >= 7)
                        unit_body.velocity = new Vector2(50 * Time.deltaTime * facingRight, 0);
                    animator.SetFloat("Speed", 1);
                    comboTime += UnityEngine.Time.deltaTime;
                    if (comboTime > 1.5f && !bossAlert)
                    {
                        SoundEffectManager.playSound("BossAttackAlert");
                        bossAlert = true;
                        bossRandomAttackTime = Random.Range(2.5f, 3.5f);
                    }
                    if (bossAlert && comboTime > bossRandomAttackTime )
                    {
                        comboTime = 0.0f;
                        comboStep = 2;
                    }
                }
                else if (comboStep >= 2)
                {
                    comboTime += UnityEngine.Time.deltaTime;
                    if (comboStep == 2)
                    {
                        animator.SetTrigger("Attack1");
                        comboStep = 3;
                    }
                    if (System.Math.Abs(playerDirection) >= 0.75 && comboTime>0.25f)
                        unit_body.velocity = new Vector2(700 * Time.deltaTime * facingRight, 0);
                    if (comboTime > 1f || System.Math.Abs(playerDirection) <= 0.75)
                    {
                        unit_body.velocity = new Vector2(0, 0);
                        isCombo = false;
                    }
                }
            }
            else if (comboType == 2)
            {
                if (comboStep == 1)
                {
                    if (System.Math.Abs(playerDirection) <= 6)
                        unit_body.velocity = new Vector2(200 * Time.deltaTime * facingRight * -1, 0);
                    if (System.Math.Abs(playerDirection) >= 7)
                        unit_body.velocity = new Vector2(50 * Time.deltaTime * facingRight, 0);
                    animator.SetFloat("Speed", 1);
                    comboTime += UnityEngine.Time.deltaTime;
                    if (comboTime > 1f && !bossAlert)
                    {
                        SoundEffectManager.playSound("BossAttackAlert");
                        bossAlert = true;
                        bossRandomAttackTime = Random.Range(1.5f, 2f);
                    }
                    if (bossAlert && comboTime > bossRandomAttackTime)
                    {
                        comboTime = 0.0f;
                        comboStep = 2;
                    }
                }
                else if (comboStep >= 2)
                {
                    comboTime += UnityEngine.Time.deltaTime;
                    if (comboStep == 2)
                    {
                        animator.SetTrigger("Attack1");
                        comboStep = 3;
                    }
                    if (comboTime > 0.1f)
                    {
                        SoundEffectManager.playSound("Teleport");
                        unit_body.velocity = new Vector2(0, 0);
                        if (autoDetect.playerPosition > -3.85 && playerDirection <0)
                        {
                            Vector3 playerPos = autoDetect.playerPositionVector;
                            playerPos.x = playerPos.x - 1.5f;
                            playerPos.y += 2f;
                            this.transform.position = playerPos;
                        }
                        else if(autoDetect.playerPosition <3.55 && playerDirection >= 0)
                        {
                            Vector3 playerPos = autoDetect.playerPositionVector;
                            playerPos.x = playerPos.x + 1.5f;
                            playerPos.y += 2f;
                            this.transform.position = playerPos;
                        }
                        else
                        {
                            Vector3 playerPos = autoDetect.playerPositionVector;
                            if (this.transform.position.x - playerPos.x >= 0)
                            {
                                playerPos.x = playerPos.x + 1.5f;
                            }
                            else if (this.transform.position.x - playerPos.x < 0)
                            {
                                playerPos.x = playerPos.x - 1.5f;
                            }
                            playerPos.y += 2f;
                            this.transform.position = playerPos;
                        }
                        isCombo = false;
                    }
                }
            }
            else if (comboType == 3)
            {
                if (comboStep == 1)
                {
                    if (System.Math.Abs(playerDirection) <= 6)
                        unit_body.velocity = new Vector2(200 * Time.deltaTime * facingRight * -1, 0);
                    if (System.Math.Abs(playerDirection) >= 7)
                        unit_body.velocity = new Vector2(50 * Time.deltaTime * facingRight, 0);
                    animator.SetFloat("Speed", 1);
                    comboTime += UnityEngine.Time.deltaTime;
                    if (comboTime > 1f && !bossAlert)
                    {
                        SoundEffectManager.playSound("BossAttackAlert");
                        bossAlert = true;
                        bossRandomAttackTime = Random.Range(1.5f, 2f);
                    }
                    if (bossAlert && comboTime > bossRandomAttackTime)
                    {
                        comboTime = 0.0f;
                        comboStep = 2;
                    }
                }
                else if (comboStep >= 2)
                {
                    comboTime += UnityEngine.Time.deltaTime;
                    if (comboStep == 2)
                    {
                        animator.SetTrigger("Attack1");
                        comboStep = 3;
                    }
                    if (comboTime > 0.1f)
                    {
                        SoundEffectManager.playSound("Spawn");
                        unit_body.velocity = new Vector2(0, 0);
                        Vector3 playerPos = autoDetect.playerPositionVector;
                        playerPos.y += 4f;
                        Instantiate(creep, playerPos, Quaternion.identity);
                        Instantiate(smoke, playerPos, Quaternion.identity);
                        isCombo = false;
                    }
                }
            }
            else if (comboType == 6)
            {
                if (comboStep == 1)
                {
                    animator.SetTrigger("Attack1");
                    unit_body.velocity = new Vector2(1000 * Time.deltaTime * facingRight * -1, 0);
                    comboTime += UnityEngine.Time.deltaTime;
                    if (System.Math.Abs(playerDirection) >= 2)
                    {
                        unit_body.velocity = new Vector2(0, 0);
                        comboStep = 2;
                        comboTime = 0f;
                    }
                }
                else if (comboStep == 2)
                {
                    comboTime += UnityEngine.Time.deltaTime;
                    if (comboTime > 0.7f)
                    {
                        comboStep = 3;
                        comboTime = 0f;
                        isCombo = false;
                    }
                }
            }
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, playerLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Bandit>().TakeDamage(attackDamage, transform.localScale.x > 0 ? 1 : -1, 1);
            }
        }
        else
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Bandit>().TakeDamage(attackDamage, transform.localScale.x > 0 ? 1 : -1, 1);
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
