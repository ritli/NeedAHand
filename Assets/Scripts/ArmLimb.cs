﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmLimb : Limb {

    float spawntime;
    bool connected = false;
    Animator animator;
    int throwForce;
    int playerID;

    // Use this for initialization
    void Start()
    {
        if (transform.parent)
        {
            connected = true;

            GetComponent<HingeJoint2D>().connectedBody = transform.parent.GetComponent<Rigidbody2D>();
        }

        id = System.Guid.NewGuid();
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

    public override void Throw(int force, int id)
    {
        throwForce = force;
        playerID = id;

        foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
        {
           // c.enabled = false;
            c.gameObject.layer = LayerMask.NameToLayer("WorldLimb");
        }

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

    public override int getPlayerID()
    {
        return playerID;
    }
}
