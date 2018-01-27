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
    private bool deactivateOnLeave = false, revertOnLeave = false;

    private List<GameObject> m_objOnTrigger = new List<GameObject>();
    private bool m_active = false;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        int massOnTrig = 0;

        foreach (GameObject obj in m_objOnTrigger)
            massOnTrig += obj.GetComponent<Body>().Mass;

        if (m_active && massOnTrig < reqMass)
        {
            m_active = false;
            if (deactivateOnLeave)
                deactivate();
            else if (revertOnLeave)
                revert();
        }
        else if (!m_active && massOnTrig >= reqMass)
        {
            m_active = true;
            activate();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Activation condition?
        if(other.GetComponent<Body>() != null)
        {
            if (!m_objOnTrigger.Contains(other.gameObject))
                m_objOnTrigger.Add(other.gameObject);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Body>() != null)
        {
            m_objOnTrigger.Remove(other.gameObject);
        }
    }

    private void activate()
    {
        animator.Play("PressurePlateDown");

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
        animator.Play("PressurePlateUp");

        foreach (Transform child in transform)
        {
            if (child.GetComponent<CrateOnRails>() != null)
                child.GetComponent<CrateOnRails>().Deactivate();
            else if (child.GetComponent<Door>() != null)
                child.GetComponent<Door>().Deactivate();
        }
    }
    
    private void revert()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CrateOnRails>() != null)
                child.GetComponent<CrateOnRails>().Revert();
            else if (child.GetComponent<Door>() != null)
                child.GetComponent<Door>().Revert();
        }
    }
}
