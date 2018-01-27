﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {

    Body parent;

    public void Start()
    {
        if (transform.parent)
        {
            parent = transform.parent.GetComponent<Body>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Limb>())
        {
            Limb l = collision.GetComponent<Limb>();

            if (!l.getConnected() && l.getLifetime() > 0.1f)
            {
                parent.AddLimb(l.getLimb());
                Destroy(collision.gameObject);
            }
        }


    }

}