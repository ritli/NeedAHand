using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegLimb : Limb {

    // Use this for initialization
    void Start () {
        if (transform.parent)
        {
            GetComponent<HingeJoint2D>().connectedBody = transform.parent.GetComponent<Rigidbody2D>();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override LimbType getLimb()
    {
        return LimbType.Leg;
    }
}
