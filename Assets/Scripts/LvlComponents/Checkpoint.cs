using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Range(0,999)]
    [SerializeField]
    int id = -1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Body>() != null)
            transform.parent.GetComponent<CheckpointHolder>().CheckpointEnter(new Vector2(transform.position.x, transform.position.y), other.GetComponent<Body>().playerID);
    }

    public CheckpointData GetCPData()
    {
        CheckpointData data;
        data.pos = new Vector2(transform.position.x, transform.position.y);
        data.id = id;
        return data;
    }
}
