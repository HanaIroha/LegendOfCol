using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicEnemy : MonoBehaviour
{
    BoxCollider2D thisEnemyHitBox;
    public int damage;
    public int forceNumber;
    void Start()
    {
        thisEnemyHitBox = this.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            collision.GetComponent<Bandit>().TakeHitDamage(damage, forceNumber);
        }
    }
}
