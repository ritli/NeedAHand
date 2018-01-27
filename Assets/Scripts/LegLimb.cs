using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegLimb : Limb {

    bool connected = false;
    public float spawntime;

    // Use this for initialization
    void Start () {
        if (transform.parent)
        {
            connected = true;

            GetComponent<HingeJoint2D>().connectedBody = transform.parent.GetComponent<Rigidbody2D>();
        }
        else
        {
            Destroy(GetComponent<HingeJoint2D>());

        }

        spawntime = Time.time;
    }

    public override LimbType getLimb()
    {
        return LimbType.Leg;
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
