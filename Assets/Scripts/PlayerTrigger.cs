﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {

    Body parent;
    public AudioClip[] pickup;
    float limbCD = 0;

    public void Update()
    {
        limbCD += Time.deltaTime;
    }
    public void Start()
    {
        

        if (transform.parent)
        {
            parent = transform.parent.GetComponent<Body>();
        }
    }

    public void PlayRandomSound(AudioClip[] clips)
    {
        transform.parent.GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Length)]);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Limb>())
        {
            Limb l = collision.GetComponent<Limb>();

            if ((!l.getConnected() && l.getLifetime() > 0.2f || (l.getPlayerID() != parent.visualPlayerID)) && limbCD > 0.05f)
            {
                limbCD = 0;
                print("Added limb");

                PlayRandomSound(pickup);

                l.id = collision.gameObject.GetComponent<Limb>().id;
                parent.AddLimb(l.getLimb());
                Destroy(collision.gameObject);
            }
        }


    }

}
