using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    [Range(1, 10)]
    [SerializeField]
    private int reqMass = 1;
    [SerializeField]
    private bool deactivateOnLeave = false;

    private int m_massOnPlate = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Activation condition?
        if(other.GetComponent<Body>() != null)
        {
            //m_massOnPlate += other.GetComponent<Body>().Mass();
        }

    }

    private void activate()
    {

    }
}
