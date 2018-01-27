using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateLinker : MonoBehaviour
{
    [SerializeField]
    PressurePlate[] linkedPlates;
    [SerializeField]
    private bool deactivateOnLeave = false, revertOnLeave = false;

    private bool m_active = false;
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_active || deactivateOnLeave || revertOnLeave)
        {
            bool checkLinked = true;
            foreach (PressurePlate plate in linkedPlates)
                checkLinked = plate.Active ? checkLinked : false;

            if (checkLinked != m_active)
            {
                if (checkLinked)
                    activate();
                else if (deactivateOnLeave || revertOnLeave)
                {
                    if (deactivateOnLeave)
                        deactivate();
                    else
                        revert();
                }
                m_active = checkLinked;
            }
        }
    }

    private void activate()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CrateOnRails>() != null)
                child.GetComponent<CrateOnRails>().Activate();
        }
    }

    private void deactivate()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CrateOnRails>() != null)
                child.GetComponent<CrateOnRails>().Deactivate();
        }
    }

    private void revert()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CrateOnRails>() != null)
                child.GetComponent<CrateOnRails>().Revert();
        }
    }
}
