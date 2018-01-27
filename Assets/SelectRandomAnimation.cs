using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRandomAnimation : MonoBehaviour {

    [Range(0,10)]
    public int range;

	void Start () {
        GetComponent<Animator>().SetInteger("State", (int)Random.Range(0, range));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
