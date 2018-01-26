using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Body : MonoBehaviour {

    public List<Limb> limbs;

    public float baseJumpForce = 100;
    //Force added per leg
    public float jumpMultiplier = 50;

    public float throwBaseForce = 10;
    //Force added per arm
    public float throwMultiplier = 5;

    public float xSpeed = 10;

    public LayerMask layermask;

    Rigidbody2D rigidbody;

    public GameObject legPrefab;
    public GameObject armPrefab;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();

    }
	
    bool OnGround
    {
        get
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, 1f, layermask))
            {
                print("ONGROUND");
                return true;
            }

            return false;
        }
    }

	// Update is called once per frame
	void Update () {
        InputUpdate();
	}

    void InputUpdate()
    {
        float xVelocity = Input.GetAxis("Horizontal");
        float gravityCompensation = OnGround ? -Physics2D.gravity.y * 0.5f : 0;

        print(gravityCompensation);

        rigidbody.AddForce(xVelocity * Vector2.right * xSpeed + gravityCompensation * Vector2.up, ForceMode2D.Force);

        if (Input.GetButtonDown("Vertical"))
        {
            rigidbody.AddForce(Vector2.up * (baseJumpForce + GetLegCount * jumpMultiplier), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            AddLimb(armPrefab.GetComponent<Limb>());
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddLimb(legPrefab.GetComponent<Limb>());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ThrowLimb(LimbType.Arm);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ThrowLimb(LimbType.Leg);
        }
    }

    bool RemoveLimb(LimbType limbtype)
    {
        for (int i = 0; i < limbs.Count; i++)
        {
            if (limbs[i].getLimb() == limbtype)
            {
                limbs.RemoveAt(i);

                for (int t = 0; t < transform.childCount; t++)
                {
                    if (transform.GetChild(i).GetComponent<Limb>().getLimb() == limbtype)
                    {
                        Destroy(transform.GetChild(i).gameObject);
                    }
                }

                return true;
            }
        }

        return false;
    }

    void ThrowLimb(LimbType limbtype)
    {
        GameObject objectToSpawn = null;

        //If player does not have limb of this type no limb is thrown
        if (!RemoveLimb(limbtype))
        {
            print("No limb left of this type");

            return;
        }

        switch (limbtype)
        {
            case LimbType.Arm:
                objectToSpawn = armPrefab;

                break;
            case LimbType.Leg:
                objectToSpawn = legPrefab;

                break;
            default:
                break;
        }

         GameObject launchedLimb = Instantiate(objectToSpawn, (Vector3)Random.insideUnitCircle * 0.25f + transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        launchedLimb.GetComponent<Rigidbody2D>().AddForce(dir.normalized * (throwBaseForce + throwMultiplier * GetArmCount), ForceMode2D.Impulse);
        Destroy(launchedLimb.GetComponent<HingeJoint2D>());

    }

    public void AddLimb(Limb limb)
    {
        GameObject objectToSpawn = null;

        limbs.Add(limb);
        bool isTrigger = true;


        switch (limb.getLimb())
        {
            case LimbType.Arm:
                objectToSpawn = armPrefab;                
                
                break;
            case LimbType.Leg:
                isTrigger = true;
                objectToSpawn = legPrefab;

                break;
            default:
                break;
        }

        GameObject g = Instantiate(objectToSpawn, (Vector3)Random.onUnitSphere + transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)), transform);


        Vector3 offset = transform.position - g.transform.position;

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;


        g.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        g.GetComponent<Rigidbody2D>().gravityScale = 0;
        g.GetComponent<Collider2D>().isTrigger = isTrigger;
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
