using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour {

    Body parent;
    public AudioClip[] pickup;


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

            if (!l.getConnected() && l.getLifetime() > 0.2f)
            {
                PlayRandomSound(pickup);

                parent.AddLimb(l.getLimb());
                Destroy(collision.gameObject);
            }
        }


    }

}
