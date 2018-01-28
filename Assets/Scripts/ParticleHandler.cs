using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    public static void SpawnParticleSystem(Vector3 position, string name)
    {
        GameObject g = Resources.Load<GameObject>(name);

        Instantiate(g, position, Quaternion.identity);
    }
}
