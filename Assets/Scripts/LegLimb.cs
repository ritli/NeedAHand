using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegLimb : Limb {

    bool connected = false;
    public float spawntime;
    int throwForce;

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

    public override void Throw(int force)
    {
        throwForce = force;

        foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
        {
            c.enabled = false;
            c.gameObject.layer = LayerMask.NameToLayer("WorldLimb");
        }

        GetComponent<Animator>().SetBool("Thrown", true);

        Invoke("EnableCollider", 0.1f);
    }

    void EnableCollider()
    {
        foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
        {
            c.enabled = true;
        } 
    }

    public override int getThrowForce()
    {
        return throwForce;
    }
}
