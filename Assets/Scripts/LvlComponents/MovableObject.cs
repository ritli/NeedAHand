using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovableObject : MonoBehaviour
{
    [Range(0,10)]
    [SerializeField]
    private int mass;
    public int Mass
    {
        get { return mass; }
    }
}
