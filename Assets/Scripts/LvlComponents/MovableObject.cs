using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovableObject : MonoBehaviour
{
    [Range(0,10)]
    [SerializeField]
    private int mass;
    public int Mass
    {
        get { return mass; }
    }

    [Range(0, 10)]
    public int PowerRequiredToBreak = 1;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.GetComponent<Limb>())
        {
            if (col.collider.GetComponent<Limb>().getThrowForce() >= PowerRequiredToBreak)
            {
                ParticleHandler.SpawnParticleSystem(transform.position, "p_cratedeath");

                Destroy(gameObject);
            }
        }
    }

}
