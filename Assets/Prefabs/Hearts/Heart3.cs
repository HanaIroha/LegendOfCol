using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart3 : MonoBehaviour
{
    public GameObject thisheart;
    public bool isForever = false;
    private void Start()
    {
        if (!isForever)
            Destroy(thisheart, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            collision.GetComponent<Bandit>().Heal(3);
            SoundEffectManager.playSound("HeartEat");
            Destroy(thisheart);
        }
    }
}
