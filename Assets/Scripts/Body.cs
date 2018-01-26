using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Body : MonoBehaviour {

    public List<Limb> limbs;

    public float baseJumpForce = 100;
    //Force added per leg
    public float jumpMultiplier = 50;

    public float xSpeed = 10;

    Rigidbody2D rigidbody;

    

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        InputUpdate();
	}

    void InputUpdate()
    {
        float xVelocity = Input.GetAxis("Horizontal");

        rigidbody.AddForce(xVelocity * transform.right * xSpeed, ForceMode2D.Force);

        if (Input.GetButtonDown("Vertical"))
        {
            rigidbody.AddForce(Vector2.up * (baseJumpForce + GetLegCount * jumpMultiplier), ForceMode2D.Impulse);
        }

    }

    int GetLegCount
    {
        get
        {
            int i = 0;
            foreach (Limb l in limbs)
            {
                if (l.GetComponent<LegLimb>())
                {
                    i++;
                }
            }

            return i;
        }
    }

    int GetArmCount
    {
        get
        {
            int i = 0;
            foreach (Limb l in limbs)
            {
                if (l.GetComponent<ArmLimb>())
                {
                    i++;
                }
            }

            return i;
        }
    }

}
