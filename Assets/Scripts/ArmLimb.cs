using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLimb : Limb {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override LimbType getLimb()
    {
        return LimbType.Arm;
    }
}
