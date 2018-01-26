using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LimbType
{
    Arm, Leg
}

public abstract class Limb : MonoBehaviour {

    bool connected;
    LimbType limbtype;

    public abstract LimbType getLimb();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Body>())
        {
            collision.collider.GetComponent<Body>().AddLimb(this);
        }
    }
}
