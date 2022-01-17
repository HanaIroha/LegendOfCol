using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject PowerUpObject;
    public bool isForever = false;
    public string namez;
    private void Start()
    {
        if (!isForever)
            Destroy(PowerUpObject, 5f);
        if (PlayerPrefs.HasKey(namez))
        {
            Destroy(PowerUpObject, 1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            collision.GetComponent<Bandit>().attackDamage += 1;
            SoundEffectManager.playSound("PlayerPowerUp");
            PlayerPrefs.SetInt(namez, 1);
            Destroy(PowerUpObject);
        }
    }
}
