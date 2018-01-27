using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateRails : MonoBehaviour {

    [SerializeField]
    private bool deactivateOnLeave = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Activation condition?
    }

    private void activate()
    {

    }
}
