using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LimbType
{
    Arm, Leg
}

public abstract class Limb : MonoBehaviour {

    LimbType limbtype;

    public abstract LimbType getLimb();
}
