using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startpoint : MonoBehaviour
{
    void Start()
    {
        GameManager._GetInstance().SetStartPoint(gameObject);
    }
}
