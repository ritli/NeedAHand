﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    private bool[] m_playersAtGoal = { false, false };

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Body>() != null)
            m_playersAtGoal[other.GetComponent<Body>().playerID - 1] = true;

        if (m_playersAtGoal[0] && m_playersAtGoal[1])
            GameManager._GetInstance().EndLevel();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Body>() != null)
            m_playersAtGoal[other.GetComponent<Body>().playerID - 1] = false;
    }
}
