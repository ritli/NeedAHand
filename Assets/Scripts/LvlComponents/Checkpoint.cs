using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Range(0,999)]
    [SerializeField]
    int id = -1, leggies = 0, armfeldts = 0;

    int state = 0;

    private void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Body>() != null)
        {
            transform.parent.GetComponent<CheckpointHolder>().CheckpointEnter(new Vector2(transform.position.x, transform.position.y), other.GetComponent<Body>().playerID);
            SetState(other.GetComponent<Body>().playerID);
        }
    }

    void SetState(int i)
    {
        if (state == 1 || state == 2)
        {
            i = 3;
        }

        state = i;

        GetComponent<Animator>().SetInteger("State", i);
    }

    public CheckpointData GetCPData()
    {
        CheckpointData data;
        data.pos = new Vector2(transform.position.x, transform.position.y);
        data.id = id;
        data.armCount = armfeldts;
        data.legCount = leggies;
        return data;
    }
}
