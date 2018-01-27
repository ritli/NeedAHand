using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When GOs w/ Body >= reqMass in trigger activate all Door & CrateOnRails children.
/// If deactivate on leave == true: When mass of GOs w/ Body in trigger < reqMass deactivate all Door & CrateOnRails children.
/// </summary>
public class PressurePlate : MonoBehaviour
{

    [Range(1, 100)]
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
            m_massOnPlate += other.GetComponent<Body>().Mass;
        }

        if (m_massOnPlate >= reqMass)
            activate();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Body>() != null)
        {
            m_massOnPlate -= other.GetComponent<Body>().Mass;
        }

        if (deactivateOnLeave && m_massOnPlate < reqMass)
        {
            deactivate();
        }
    }

    private void activate()
    {
        foreach(Transform child in transform)
        {
            if (child.GetComponent<CrateOnRails>() != null)
                child.GetComponent<CrateOnRails>().Activate();
            else if (child.GetComponent<Door>() != null)
                child.GetComponent<Door>().Activate();
        }
    }

    private void deactivate()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CrateOnRails>() != null)
                child.GetComponent<CrateOnRails>().Deactivate();
            else if (child.GetComponent<Door>() != null)
                child.GetComponent<Door>().Deactivate();
        }
    }
}
