using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            if(collision.GetComponent<Bandit>().respawmPoint != this.transform.position)
            {
                collision.GetComponent<Bandit>().respawmPoint = this.transform.position;
                SoundEffectManager.playSound("CheckPoint");
            }
        }
    }
}
