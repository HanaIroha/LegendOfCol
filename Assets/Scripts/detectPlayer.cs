using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class detectPlayer : MonoBehaviour
{
    public float? playerPosition;
    public float? playerAirPosition;
    public Vector3 playerPositionVector;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            playerPosition = collision.gameObject.transform.position.x;
            playerAirPosition = collision.gameObject.transform.position.x;
            playerPositionVector = collision.gameObject.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            playerPosition = collision.gameObject.transform.position.x;
            playerPositionVector = collision.gameObject.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            playerPosition = null;
            playerAirPosition = null;
            playerPositionVector = Vector3.zero;
        }
    }
}
