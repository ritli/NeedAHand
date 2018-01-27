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
    [Range(0,5f)]
    [SerializeField]
    private float cdSeconds = 0;

    private bool m_onCooldown = false;
    private float m_cdProgress = 0;
    private List<Rigidbody2D> m_stiffsOnSpring = new List<Rigidbody2D>();


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
            m_stiffsOnSpring.Add(other.GetComponent<Rigidbody2D>());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
            m_stiffsOnSpring.Remove(other.GetComponent<Rigidbody2D>());
    }

    void Update()
    {
        if(m_onCooldown)
        {
            if(m_cdProgress>=cdSeconds)
            {
                m_cdProgress = 0;
                m_onCooldown = false;
                return;
            }
            m_cdProgress += Time.deltaTime;
        }
        else
        {
            if (m_stiffsOnSpring.Count > 0)
            {
                foreach (Rigidbody2D rb in m_stiffsOnSpring)
                    rb.AddForce((Vector2.right * springForce * xDir) + (Vector2.up * springForce * yDir));
                GetComponent<Animator>().SetTrigger("Jump");
                m_onCooldown = true;
            }
        }
    }
}
