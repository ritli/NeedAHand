using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if((other.gameObject.GetComponent(typeof(Body)) as Body) != null)
        {
            GameManager._GetInstance().RespawnPlayer(other.gameObject);
        }
    }
}
