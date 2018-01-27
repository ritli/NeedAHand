using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Body>() != null)
            transform.parent.GetComponent<CheckpointHolder>().CheckpointEnter(new Vector2(transform.position.x, transform.position.y), other.GetComponent<Body>().playerID);
    }

    public CheckpointData GetCPData()
    {
        CheckpointData data;
        data.pos = new Vector2(transform.position.x, transform.position.y);
        data.id = -1;
        return data;
    }
}
