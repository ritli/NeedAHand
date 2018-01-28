using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxHandler : MonoBehaviour {

    Camera camera;

    Vector3 cameraLastPos;

    Vector2 camVelocity;

    GameObject y;
    GameObject x;

    void Start () {
        camera = Camera.main;

	}
	
	void Update () {
        camVelocity = camera.transform.position - cameraLastPos;

        cameraLastPos = camera.transform.position;

        transform.position += (Vector3)camVelocity * 0.5f;

        if (Mathf.Abs(transform.position.x - camera.transform.position.x) > 20f){
            transform.position += Vector3.right * Mathf.Sign(camera.transform.position.x - transform.position.x) * 30;
        }
        if (Mathf.Abs(transform.position.y - camera.transform.position.y) > 20f)
        {
            transform.position += Vector3.up * Mathf.Sign(camera.transform.position.y - transform.position.y) * 30;
        }

    }
}
