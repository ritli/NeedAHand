using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Body : MonoBehaviour {

    [Range(1,2)]
    public int playerID = 1;

    public List<Limb> limbs;

    public float baseJumpForce = 100;
    //Force added per leg
    public float jumpMultiplier = 50;

    public float throwBaseForce = 10;
    //Force added per arm
    public float throwMultiplier = 5;

    public float xSpeed = 10;

    public LayerMask layermask;

    private int m_mass = 1;
    public int Mass
    {
        get { return m_mass; }
    }

    GameObject target;
    GameObject throwingArm;

    Rigidbody2D rigidbody;

    public GameObject legPrefab;
    public GameObject armPrefab;
    public GameObject targetPrefab;
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();

    }
	
    bool OnGround
    {
        get
        {
            return Physics2D.Raycast(transform.position, Vector2.down, 1f, layermask);
        }
    }

	// Update is called once per frame
	void Update () {
        InputUpdate();
	}

    void Throw()
    {
        if (throwingArm)
        {

        }
    }

    void ShowTarget(bool isActive)
    {
        if (!target && isActive)
        {
            target = Instantiate(targetPrefab, transform);
            target.transform.localPosition = Vector2.zero;
        }
        else if (target)
        {
            target.transform.localPosition = Vector2.up;

            Vector2 input = new Vector2(Input.GetAxis("p" + playerID + "Horizontal"), Input.GetAxis("p" + playerID + "Vertical"));

            float deadzone = 0.25f;

            if (input.magnitude < deadzone)
            {
                input = Vector2.zero;
            }
      

            Vector3 offset = (Vector2)transform.position - ((Vector2)transform.position + input);


            switch (playerID)
            {
                case 1:
                    input = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                    break;
                case 2:

                    break;

                default:
                    break;
            }

            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            target.transform.localPosition = input.normalized * 3;
        }
        if (!isActive && target)
        {
            Destroy(target);
        }
    }

    void InputUpdate()
    {
        float xVelocity = Input.GetAxis("p" + playerID + "Horizontal");
        float gravityCompensation = OnGround ? -Physics2D.gravity.y * 0f : 0;

        if (Input.GetAxis("p" + playerID + "ThrowTrigger") < 0.5f)
        {
            transform.Translate(xVelocity * Vector2.right * xSpeed + gravityCompensation * Vector2.up, Space.World);
        }

        if (Input.GetButtonDown("p" + playerID + "Vertical") && OnGround && Input.GetAxis("p" + playerID + "ThrowTrigger") < 0.5f)
        {
            rigidbody.AddForce(Vector2.up * (baseJumpForce + GetLegCount * jumpMultiplier), ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("p" + playerID + "Throw"))
        {

        }

        if (Input.GetAxis("p" + playerID + "ThrowTrigger") > 0.5f)
        {
            print("Showing target");

            ShowTarget(true);

            if (Input.GetButtonDown("p" + playerID + "Vertical")){
                ThrowLimb(LimbType.Leg);
            }
            if (Input.GetButtonDown("p" + playerID + "Throw"))
            {
                ThrowLimb(LimbType.Arm);
            }
        }
        else
        {
            ShowTarget(false);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            AddLimb(armPrefab.GetComponent<Limb>().getLimb());
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddLimb(legPrefab.GetComponent<Limb>().getLimb());
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
                    if (transform.GetChild(t).GetComponent<Limb>()) {
                        if (transform.GetChild(t).GetComponent<Limb>().getLimb() == limbtype)
                        {
                            Destroy(transform.GetChild(t).gameObject);

                            return true;
                        }
                    }
                    
                }

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

        Vector2 dir = Vector2.zero;

        switch (playerID)
        {
            case 1:
                dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                break;
            case 2:
                dir =target.transform.position - transform.position;

                break;
            default:
                break;
        }

        launchedLimb.GetComponent<Rigidbody2D>().mass = 1;

        launchedLimb.GetComponent<Rigidbody2D>().AddForce(dir.normalized * (throwBaseForce + throwMultiplier * GetArmCount), ForceMode2D.Impulse);
        Destroy(launchedLimb.GetComponent<HingeJoint2D>());

    }

    public void AddLimb(LimbType limb)
    {
        GameObject objectToSpawn = null;

        bool isTrigger = true;

        switch (limb)
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
        limbs.Add(g.GetComponent<Limb>());

        g.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        g.GetComponent<Limb>().setConnected(true);

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

