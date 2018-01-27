using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LimbType
{
    Arm, Leg
}

public abstract class Limb : MonoBehaviour {

    public abstract float getLifetime();
    public abstract bool getConnected();
    public abstract void setConnected(bool connected);
    public abstract void Throw(int force);
    public abstract int getThrowForce();

    LimbType limbtype;

    public abstract LimbType getLimb();
}
