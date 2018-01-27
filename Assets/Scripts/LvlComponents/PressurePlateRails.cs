using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateRails : MonoBehaviour {

    [SerializeField]
    private bool deactivateOnLeave = false;

    [SerializeField]
    private float distance = 0f;
    [SerializeField]
    private float moveSpeed = 0f;
    [Range(-1, 1)]
    [SerializeField]
    private int xDir = 0;
    [Range(-1, 1)]
    [SerializeField]
    private int yDir = 0;
    [SerializeField]
    private bool loop = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Activation condition?
    }

    private void activate()
    {
        GetComponentInChildren<CrateOnRails>().Activate(distance, moveSpeed, xDir, yDir, loop);
    }
}
