using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateDoor : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        // Activation condition?
    }

    private void activate()
    {
        GetComponentInChildren<Door>().Activate();
    }
}
