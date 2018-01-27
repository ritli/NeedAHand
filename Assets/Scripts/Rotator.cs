using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	private Transform target;
	public Vector3 rotationSpeeds;
	private void Start()
	{
		target = gameObject.transform;
	}

	// Update is called once per frame
	void Update () {
		target.Rotate(rotationSpeeds);
	}
}
