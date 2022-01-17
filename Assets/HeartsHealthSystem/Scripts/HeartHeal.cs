using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartHeal : MonoBehaviour
{

    [SerializeField] private int healAmount;

    private void OnTriggerEnter2D(Collider2D collider) {
        Bandit player = collider.GetComponent<Bandit>();
        if (player != null) {
            // We hit the Player
            player.Heal(healAmount);
            Destroy(gameObject);
        }
    }

}