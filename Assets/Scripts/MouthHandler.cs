using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthHandler : MonoBehaviour {

    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        CloseMouthLoop();
    }
	
    public void OpenMouth()
    {
        animator.SetInteger("OpenState", Random.Range(1, 3));
    }

    public void CloseMouth()
    {
        animator.SetInteger("OpenState", 0);
        animator.SetInteger("State", Random.Range(0, 6));
    }

    public void CloseMouthLoop()
    {
        animator.SetInteger("OpenState", 0);
        animator.SetInteger("State", Random.Range(0, 6));

        Invoke("CloseMouthLoop", Random.Range(2, 5));
    }
}
