using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    public int maxHealth = 100;
    int currentHealth;
    bool isDeath = false;
    [SerializeField] private GameObject thisEnemy;

    public int heartlv = 0;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
    }

    public void TakeDamage(int damage, int direction, bool isCounter)
    {
        if (!isDeath)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Debug.Log("Died!");
                //GetComponent<Collider2D>().enabled = false;
                SoundEffectManager.playSound("MonsterDie");
                isDeath = true;
                animator.SetTrigger("Dead");
                if(this.GetComponent<ToxicEnemy>() != null)
                    animator.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                animator.SetTrigger("Hurt");
                SoundEffectManager.playSound("MonsterHit");
                //GetComponent<Rigidbody2D>().AddForce(new Vector2(2000 * direction * (isCounter?2:1), 0));
            }
        }
    }

    void Die()
    {
        Destroy(thisEnemy);
        int randomrate = Random.Range(1, 101);
        int randomheart = Random.Range(1, 101);
        if (heartlv == 3 && randomrate < 25)
        {
            if(randomheart <= 70)
            {
                Instantiate(heart3, thisEnemy.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(heart4, thisEnemy.transform.position, Quaternion.identity);
            }
        }
        else if (heartlv == 2 && randomrate < 25)
        {
            if (randomheart <= 15)
            {
                Instantiate(heart3, thisEnemy.transform.position, Quaternion.identity);
            }
            else if (randomheart <= 40)
            {
                Instantiate(heart2, thisEnemy.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(heart1, thisEnemy.transform.position, Quaternion.identity);
            }
        }
        else if (heartlv == 1 && randomrate < 25)
        {
            if (randomheart <= 70)
            {
                Instantiate(heart1, thisEnemy.transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(heart2, thisEnemy.transform.position, Quaternion.identity);
            }
        }
        this.enabled = false;
    }
}
