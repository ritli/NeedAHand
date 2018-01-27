using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLimb : Limb {

    float spawntime;
    bool connected = false;

    // Use this for initialization
    void Start()
    {
        if (transform.parent)
        {
            connected = true;

            GetComponent<HingeJoint2D>().connectedBody = transform.parent.GetComponent<Rigidbody2D>();
        }

        spawntime = Time.time;
    }

    public override LimbType getLimb()
    {
        return LimbType.Arm;
    }

    public override bool getConnected()
    {
        return connected;
    }

    public override void setConnected(bool connected)
    {
        this.connected = connected;
    }

    public override float getLifetime()
    {
        return Time.time - spawntime;
    }
}
