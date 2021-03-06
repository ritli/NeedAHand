﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Body : MonoBehaviour {

    [Range(1,2)]
    public int playerID = 1;

    public int visualPlayerID = 1;

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
	public GameObject[] moveEffets;
	

    CircleCollider2D collider;
    Vector2 colliderOffset;
    float colliderSize;

    Animator animator;
	private ParticleSystem m_MoveEffect;
    GameObject eyes;
    bool growlSoundPlayed = false;
    bool InAir = false;
    float airMultiplier = 1;

    float xMultiplier = 0;

    MouthHandler mouth;
    Vector2 lastInput;

    AudioSource audio;
    bool canSwitchCharacter = true;

    void Start () {
        mouth = GetComponentInChildren<MouthHandler>();
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();



        if (playerID == 2)
        {
            visualPlayerID = 2;

            animator.SetInteger("Blue", 1);
			m_MoveEffect = Instantiate(moveEffets[1]).GetComponent<ParticleSystem>();
        }
		else
		{
            visualPlayerID = 1;

            m_MoveEffect = Instantiate(moveEffets[0]).GetComponent<ParticleSystem>();
		}
		m_MoveEffect.transform.SetParent(gameObject.transform);
		m_MoveEffect.transform.localPosition = Vector3.zero;

        eyes = transform.Find("Eyes").gameObject;
        collider = GetComponent<CircleCollider2D>();
        colliderOffset = collider.offset;
        colliderSize = collider.radius;
    }
	
    public void SetStartingLimbs(int LegCount, int ArmCount, bool clearCurrentLimbs)
    {
        if (clearCurrentLimbs)
        {
            PurgeLimbs();
        }

        for (int i = 0; i < LegCount; i++)
        {
            GameManager._GetInstance().TrackLimb(AddLimb(LimbType.Leg), playerID);
        }
        for (int i = 0; i < ArmCount; i++)
        {
            GameManager._GetInstance().TrackLimb(AddLimb(LimbType.Arm), playerID);
        }
    }

    private void PurgeLimbs()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Limb>() != null)
            {
                if (limbs.Contains(child.GetComponent<Limb>()))
                    limbs.Remove(child.GetComponent<Limb>());
                GameManager._GetInstance().UntrackLimb(child.GetComponent<Limb>());
                Destroy(child.gameObject);
            }
        }
    }

    public void ResetAnimator()
    {
        if (visualPlayerID == 2)
        {
            animator.SetInteger("Blue", 1);
        }
    }

    public void PlayDeathSound()
    {
        AudioClip[] clips = deathSounds;

        if (Camera.main.GetComponent<AudioSource>())
        {
            Camera.main.GetComponent<AudioSource>().PlayOneShot((clips[Random.Range(0, clips.Length)]));
        }
        else
        {
            Camera.main.gameObject.AddComponent<AudioSource>().PlayOneShot((clips[Random.Range(0, clips.Length)]));
        }
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

    IEnumerator SwitchControls()
    {
        canSwitchCharacter = false;

        string s = "Player2Arrow";

        if (visualPlayerID == 1)
        {
            s = "Player1Arrow";
        }

        GameObject arrow = Resources.Load<GameObject>(s);

      

        switch (playerID)
        {
            case 1:
                playerID = 2;

                arrow = Instantiate(arrow, transform.position + Vector3.up, transform.rotation, transform);

                if (playerID != visualPlayerID)
                {
                    arrow.GetComponentInChildren<TMPro.TextMeshPro>().text = "Player 2\n(Controller)";
                }


                break;
            case 2:
                playerID = 1;

                
                arrow = Instantiate(arrow, transform.position + Vector3.up, transform.rotation, transform);

                if (playerID != visualPlayerID)
                {
                    arrow.GetComponentInChildren<TMPro.TextMeshPro>().text = "Player 1\n(Keyboard)";
                }

                break;
            default:
                break;
        }

        TMPro.TextMeshPro text = arrow.GetComponentInChildren<TMPro.TextMeshPro>();
        SpriteRenderer sprite = arrow.GetComponent<SpriteRenderer>();

        float alpha = 0, toAlpha = 1;
        int mult = 1;


        do
        {
            alpha += Time.deltaTime * mult;

            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

            yield return new WaitForEndOfFrame();
        } while (alpha < toAlpha);

        yield return new WaitForSeconds(1f);

        toAlpha = 0;
        mult = -1;

        do
        {
            alpha += Time.deltaTime * mult;

            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

            yield return new WaitForEndOfFrame();
        } while (alpha > toAlpha);

        Destroy(arrow.gameObject);
        canSwitchCharacter = true;
    }

    void ShowTarget(bool isActive)
    {
        if (!target && isActive)
        {
            target = Instantiate(targetPrefab, transform);
            target.transform.localPosition = Vector2.zero;

            switch (visualPlayerID)
            {
                case 1:
                    target.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case 2:
                    target.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                default:
                    break;
            }
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
                    /*
                    foreach (string s in Input.GetJoystickNames())
                    {
                        print(s);
                    }


                    if (Input.GetJoystickNames().Length < 1)
                    {
                        input = Vector2.MoveTowards(lastInput, input, 0.2f);

                        lastInput = input;
                    }
                    */
                    break;

                default:
                    break;
            }

            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

            eyes.transform.localPosition = input.normalized * 0.05f;
            mouth.transform.localPosition = input.normalized * 0.03f  + Vector2.down * 0.2f;

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
            collider.radius = Mathf.Lerp(collider.radius, colliderSize + 0.1f, 0.6f);

        }
        else
        {
            animator.SetBool("InAir", false);

            collider.offset = Vector2.Lerp(collider.offset, colliderOffset, 0.6f);
            collider.radius = Mathf.Lerp(colliderSize + 0.1f, colliderSize, 0.6f);

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
                xMultiplier = 0.2f;


                if (visualPlayerID == 2)
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
        if (Input.GetKeyDown(KeyCode.Tab) && canSwitchCharacter)
        {
            print("Switching");
            StartCoroutine(SwitchControls());
        }

        if (Input.GetKeyDown(KeyCode.F1) && playerID == 1)
        {
            GameManager._GetInstance().LoadLevel(2);

        }

        if (Input.GetKeyDown(KeyCode.F2) && playerID == 1)
        {
            GameManager._GetInstance().LoadLevel(3);

        }



        float xVelocity = Input.GetAxis("p" + playerID + "Horizontal");
        float gravityCompensation = OnGround ? -Physics2D.gravity.y * 0f : 0;
   
       


        if (Mathf.Abs(xVelocity) > 0.1f)
        {
            xVelocity *= xMultiplier;

            xMultiplier += Time.deltaTime * 4;


            mouth.transform.localPosition = xVelocity * Vector2.right * 0.10f + Vector2.down * 0.2f;

            eyes.transform.localPosition = xVelocity * Vector2.right * 0.15f;
        }

        else
        {
            xMultiplier -= Time.deltaTime * 4;


        }

        xMultiplier = Mathf.Clamp01(xMultiplier);

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


            if (visualPlayerID == 2)
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

                mouth.OpenMouth();

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

    }

    System.Guid RemoveLimb(LimbType limbtype)
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
                            System.Guid oldId = transform.GetChild(t).GetComponent<Limb>().id;
                            Destroy(transform.GetChild(t).gameObject);

                            return oldId;
                        }
                    }
                    
                }

            }
        }

        return System.Guid.Empty;
    }

    void ThrowLimb(LimbType limbtype)
    {
        GameObject objectToSpawn = null;
        System.Guid limbID = RemoveLimb(limbtype);

        //If player does not have limb of this type no limb is thrown
        if (limbID == System.Guid.Empty)
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

        launchedLimb.GetComponent<Limb>().id = limbID;
        launchedLimb.GetComponent<Limb>().Throw(GetArmCount, visualPlayerID);

        Vector2 dir = Vector2.zero;

        dir = target.transform.position - transform.position;

        launchedLimb.GetComponent<Rigidbody2D>().mass = 1;

        launchedLimb.GetComponent<Rigidbody2D>().AddForce(dir.normalized * (throwBaseForce + throwMultiplier * GetArmCount), ForceMode2D.Impulse);
        Destroy(launchedLimb.GetComponent<HingeJoint2D>());

        PlayRandomSound(throwSounds);

    }

    float limbCD = 0;

    public System.Guid AddLimb(LimbType limb)
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
                while (randomLoc.y > -0.5)
                {
                    randomLoc = Random.insideUnitCircle.normalized;
                }

                break;
            default:
                break;
        }

        randomLoc = randomLoc.normalized;



            if (limb == LimbType.Leg)
            {
                randomLoc.x *= 0.45f;

                randomLoc.y *= 0.35f;
            }
            else
            {
                randomLoc.x *= 0.4f;
                randomLoc.y *= 0.25f;
            }
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

        return g.GetComponent<Limb>().id;
    }

    int GetLegCount
    {
        get
        {
            int i = 0;

            for (int t = 0; t < limbs.Count; t++)
            {
                if (limbs[t] == null)
                {
                    limbs.RemoveAt(t);
                }
            }

            foreach (Limb l in limbs)
            {
                if (l != null)
                {

                    if (l.GetComponent<LegLimb>())
                    {
                        i++;
                    }
                }

            }

            return i;
        }
    }

    int GetArmCount
    {
        get
        {
            for (int t = 0; t < limbs.Count; t++)
            {
                if (limbs[t] == null)
                {
                    limbs.RemoveAt(t);
                }
            }

            int i = 0;
            foreach (Limb l in limbs)
            {
                if (l != null)
                {

                    if (l.GetComponent<ArmLimb>())
                    {
                        i++;
                    }
                }
            }

            return i;
        }
    }



}

