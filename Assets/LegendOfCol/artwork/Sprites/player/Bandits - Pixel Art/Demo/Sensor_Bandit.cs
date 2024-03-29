﻿using UnityEngine;
using System.Collections;

public class Sensor_Bandit : MonoBehaviour {

    //public GameObject player;
    private int m_ColCount = 0;

    private float m_DisableTimer;

    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        m_ColCount++;
        //if (other.gameObject.CompareTag("Pallete"))
        //{
        //    player.transform.parent = other.gameObject.transform;
        //}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_ColCount--;
        //if (other.gameObject.CompareTag("Pallete"))
        //{
        //    player.transform.parent = null;
        //}
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }

}
