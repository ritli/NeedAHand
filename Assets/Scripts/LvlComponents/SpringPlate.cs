using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPlate : MonoBehaviour
{
    [Range(1000, 10000)]
    [SerializeField]
    private float springForce = 1000;
    [Range(-1, 1)]
    [SerializeField]
    private int xDir = 0, yDir = 0;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
            other.GetComponent<Rigidbody2D>().AddForce((Vector2.right * springForce * xDir) + (Vector2.up * springForce * yDir));
    }
}
