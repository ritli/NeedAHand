using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedParticle : MonoBehaviour {

    public float Lifetime = 5;

	void Start () {
        Invoke("DestroyDelayed", Lifetime);
	}
    void DestroyDelayed()
    {
        Destroy(gameObject);
    }
}
