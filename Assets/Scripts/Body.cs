using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Body : MonoBehaviour {

    [Range(1,2)]
    public int playerID = 1;

    public List<Limb> limbs;

    public List<Vector3> localPositions;

    public float massCompensation = 10f;

    [Header("Jump vars")]
    public float baseJumpForce = 100;
    //Force added per leg
    public float jumpMultiplier = 50;

    [Header("Throw vars")]
    public float throwBaseForce = 10;
    //Force added per arm
    public float throwMultiplier = 5;

    [Header("Movement vars")]
    public float xSpeed = 10;

    public LayerMask layermask;
    bool jumping = false;

    public int baseMass = 5;

    private int m_mass = 1;
    public int Mass
    {
        get { return baseMass + GetArmCount + GetLegCount; }
    }

    GameObject target;
    GameObject throwingArm;

    Rigidbody2D rigidbody;

    public float skinWidth = 0.25f;

    public AudioClip[] jumpSounds;
    public AudioClip[] landSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] throwSounds;
    public AudioClip[] growlSounds;
    public AudioClip[] bluegrowlSounds;


    public GameObject legPrefab;
    public GameObject armPrefab;
    public GameObject targetPrefab;

    BoxCollider2D collider;
    Vector2 colliderOffset;
    Vector2 colliderSize;

    Animator animator;

    GameObject eyes;
    bool growlSoundPlayed = false;
    bool InAir = false;
    float airMultiplier = 1;

    AudioSource audio;

    void Start () {
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        if (playerID == 2)
        {
            animator.SetInteger("Blue", 1);
        }

        eyes = transform.Find("Eyes").gameObject;
        collider = GetComponent<BoxCollider2D>();
        colliderOffset = collider.offset;
        colliderSize = collider.size;
    }
	
    public void SetStartingLimbs(int LegCount, int ArmCount, bool clearCurrentLimbs)
    {
        if (clearCurrentLimbs)
        {
            int length = GetArmCount;

            for (int i = 0; i < length; i++)
            {
                RemoveLimb(LimbType.Arm);
            }

            length = GetLegCount;

            for (int i = 0; i < length; i++)
            {
                RemoveLimb(LimbType.Leg);
            }
        }

        for (int i = 0; i < LegCount; i++)
        {
            AddLimb(LimbType.Leg);
        }
        for (int i = 0; i < ArmCount; i++)
        {
            AddLimb(LimbType.Leg);
        }
    }

    public void ResetAnimator()
    {
        if (playerID == 2)
        {
            animator.SetInteger("Blue", 1);
        }
    }

    public void PlayDeathSound()
    {
        PlayRandomSound(deathSounds);
    }

    public void PlayRandomSound(AudioClip[] clips)
    {
        audio.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    bool OnGround
    {
        get
        {
            return Physics2D.Raycast(transform.position + new Vector3(0.5f, -1) * skinWidth, Vector2.down, 0.1f, layermask) || Physics2D.Raycast(transform.position + new Vector3(-0.5f, -1) * skinWidth, Vector2.down, 0.1f, layermask);
        }
    }

	void Update () {
        InputUpdate();
        AnimationUpdate();
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
                    if (Input.GetJoystickNames().Length < 2)
                    {
                        input = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    }
                    break;
                case 2:

                    break;

                default:
                    break;
            }

            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            eyes.transform.localPosition = input.normalized * 0.05f;

            target.transform.localPosition = input.normalized * 3;
        }
        if (!isActive && target)
        {
            Destroy(target);
        }
    }

    void ShowLimbs(bool show, bool onlyLegs)
    {
        Vector3 location;

        if (GetLegCount > 0 && show)
        {
            animator.SetBool("InAir", true);

            collider.offset = Vector2.Lerp(collider.offset, colliderOffset + Vector2.down * 0.05f, 0.6f);
            collider.size = Vector2.Lerp(collider.size, colliderSize + Vector2.up * 0.1f, 0.6f);

        }
        else
        {
            animator.SetBool("InAir", false);

            collider.offset = Vector2.Lerp(collider.offset, colliderOffset, 0.6f);
            collider.size = Vector2.Lerp(collider.size, colliderSize, 0.6f);
        }

        if (show)
        {
            for (int i = 0; i < limbs.Count; i++)
            {
                if (onlyLegs && limbs[i].getLimb() != LimbType.Leg)
                {
                    continue;
                }

                if (!limbs[i].GetComponent<Collider2D>().enabled)
                {
                    limbs[i].GetComponent<Rigidbody2D>().simulated = !onlyLegs;
                    limbs[i].transform.GetChild(0).GetComponent<Rigidbody2D>().simulated = !onlyLegs;
                    limbs[i].GetComponent<Collider2D>().enabled = true;
                    limbs[i].transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
                }
                
                location = Vector3.Lerp(limbs[i].transform.localPosition, localPositions[i], 0.6f);
                limbs[i].transform.localPosition = location;
                limbs[i].GetComponent<HingeJoint2D>().connectedAnchor = location;
                Color color = limbs[i].GetComponent<SpriteRenderer>().color;
                limbs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

                color.a = Mathf.Lerp(color.a, 1, 0.6f);

                limbs[i].GetComponent<SpriteRenderer>().color = color;
            }
        }
        else
        {
            for (int i = 0; i < limbs.Count; i++)
            {


                if (onlyLegs && limbs[i].getLimb() != LimbType.Leg)
                {
                    continue;
                }

                if (limbs[i].GetComponent<Collider2D>().enabled)
                {
                    limbs[i].GetComponent<Rigidbody2D>().simulated = false;
                    limbs[i].transform.GetChild(0).GetComponent<Rigidbody2D>().simulated = false;

                    limbs[i].GetComponent<Collider2D>().enabled = false;
                    limbs[i].transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
                }

                location = Vector3.Lerp(limbs[i].transform.localPosition, Vector3.zero, 10 * Time.deltaTime);

                Color color = limbs[i].GetComponent<SpriteRenderer>().color;
                color.a = Mathf.Lerp(color.a, 0, 10 * Time.deltaTime);
                limbs[i].GetComponent<SpriteRenderer>().color = color;
                limbs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

                limbs[i].transform.localPosition = location;
                limbs[i].GetComponent<HingeJoint2D>().connectedAnchor = location;
            }
        }
    }

    IEnumerator JumpRoutine()
    {
        jumping = true;
        float time = 0;

        while (time < 0.4f)
        {
            ShowLimbs(true, true);

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        jumping = false;
    }

    void AnimationUpdate()
    {
        if (!OnGround)
        {
            if (!InAir)
            {
                InAir = true;
            }


            animator.SetBool("InAir", true);
        }
        else
        {
            if (InAir)
            {
                PlayRandomSound(landSounds);
                InAir = false;



                if (playerID == 2)
                {
                    ParticleHandler.SpawnParticleSystem(transform.position + Vector3.down * 0.5f, "p_bluesplash");
                }
                else
                {
                    ParticleHandler.SpawnParticleSystem(transform.position + Vector3.down * 0.5f, "p_splash");
                }

            }

            if (Input.GetAxis("p" + playerID + "ThrowTrigger") < 0.5f)
            {
                animator.SetBool("InAir", false);
            }
        }
    }

    void InputUpdate()
    {
        float xVelocity = Input.GetAxis("p" + playerID + "Horizontal");
        float gravityCompensation = OnGround ? -Physics2D.gravity.y * 0f : 0;

        if (playerID == 2)
        {
            print(xVelocity);
        }

        if (Mathf.Abs(xVelocity) > 0.1f)
        {
            eyes.transform.localPosition = xVelocity * Vector2.right * 0.15f;
        }

        if (Input.GetAxis("p" + playerID + "ThrowTrigger") < 0.5f && Mathf.Abs(xVelocity) > 0.3f)
        {

            if (InAir)
            {
                airMultiplier -= Time.deltaTime;
                
            }
            else
            {
                airMultiplier += Time.deltaTime * 4;

            }

            airMultiplier = Mathf.Clamp01(0.25f + airMultiplier);
            Vector2 vel = (xVelocity * Vector2.right * xSpeed * airMultiplier);

            vel.y = rigidbody.velocity.y;
            rigidbody.velocity = vel;
        }

        if (Input.GetButtonDown("p" + playerID + "Vertical") && OnGround && Input.GetAxis("p" + playerID + "ThrowTrigger") < 0.5f)
        {
            PlayRandomSound(jumpSounds);


            if (playerID == 2)
            {
                ParticleHandler.SpawnParticleSystem(transform.position, "p_bluejump");
            }
            else
            {
                ParticleHandler.SpawnParticleSystem(transform.position, "p_jump");
            }
            rigidbody.AddForce(Vector2.up * (baseJumpForce + GetLegCount * jumpMultiplier + (GetLegCount + GetArmCount) * massCompensation), ForceMode2D.Impulse);
    
            StartCoroutine(JumpRoutine());
        }

        if (Input.GetButton("p" + playerID + "Throw"))
        {
            //ShowLimbs(true);
        }
        else
        {
            //ShowLimbs(false);
        }

        if (Input.GetAxis("p" + playerID + "ThrowTrigger") > 0.5f)
        {
            if (!growlSoundPlayed)
            {
                if (playerID == 2)
                {
                    PlayRandomSound(bluegrowlSounds);

                }
                else
                {
                    PlayRandomSound(growlSounds);

                }
                growlSoundPlayed = true;

                audio.pitch = 1f;

            }

            ShowLimbs(true, false);
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
            growlSoundPlayed = false;

            if (!jumping)
            {
                ShowLimbs(false, false);
            }

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
                localPositions.RemoveAt(i);

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

        launchedLimb.GetComponent<Limb>().Throw(GetArmCount);

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

        PlayRandomSound(throwSounds);

    }

    public void AddLimb(LimbType limb)
    {
        GameObject objectToSpawn = null;

        Vector3 randomLoc = Random.insideUnitCircle.normalized;

        bool loop = true;

        while (loop) {

        switch (limb)
        {
            case LimbType.Arm:
                objectToSpawn = armPrefab;                
                
                while(randomLoc.y < 0.2)
                {
                    randomLoc = Random.insideUnitCircle.normalized;
                }

                break;
            case LimbType.Leg:

                objectToSpawn = legPrefab;
                while (randomLoc.y > -0.4)
                {
                    randomLoc = Random.insideUnitCircle.normalized;
                }

                break;
            default:
                break;
        }

        randomLoc = randomLoc.normalized;
        randomLoc.x *= 0.4f;
        randomLoc.y *= 0.25f;

            if (limbs.Count == 0)
            {
                loop = false;
            }

            foreach(Limb l in limbs)
            {
                if (Vector2.Distance(l.transform.localPosition, randomLoc) > 0.04f)
                {
                    loop = false;
                }
            }

        }


        Vector3 location = randomLoc + transform.position;
        location.z = 0;

        GameObject g = Instantiate(objectToSpawn, location, Quaternion.Euler(0, 0, Random.Range(0, 360)), transform);

        g.GetComponent<HingeJoint2D>().enabled = true;
        g.GetComponent<HingeJoint2D>().connectedBody = rigidbody;
        g.GetComponent<HingeJoint2D>().connectedAnchor = g.transform.localPosition;
        g.GetComponent<Rigidbody2D>().gravityScale = 1;

        Vector3 offset = transform.position - g.transform.position;

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        limbs.Add(g.GetComponent<Limb>());
        localPositions.Add(g.transform.localPosition);

        g.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        g.GetComponent<Limb>().setConnected(true);


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

