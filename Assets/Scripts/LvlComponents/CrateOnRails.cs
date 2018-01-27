using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// NOTE: Needs to be child of a PressurePlate object to work properly
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CrateOnRails : MonoBehaviour {

    [Range(0f,25f)]
    [SerializeField]
    private float m_range;
    [SerializeField]
    private float m_speed;
    [Range(-1, 1)]
    [SerializeField]
    private int m_xDir, m_yDir;
    [SerializeField]
    private bool m_looping;

    private Vector3 m_startPos;
    [SerializeField]
    private bool m_acitve = false;
    private bool m_returning = false;

    Rigidbody2D rb;

	// Use this for initialization
	void Start ()
    {
        m_startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
	}
	
	public void Activate()
    {
        m_acitve = true;
    }

    public void Deactivate()
    {
        m_acitve = false;
    }

    void FixedUpdate()
    {
        if(m_acitve)
        {
            float dist = Vector3.Distance(m_startPos, transform.position);
            if((dist < m_range) && !m_returning)
            {
                rb.velocity = new Vector3(m_speed * m_xDir, m_speed * m_yDir, 0f);
            }
            else if(m_returning && dist > 0.3f)
            {
                rb.velocity = new Vector3(m_speed * m_xDir, m_speed * m_yDir, 0) * -1;
            }
            else if(m_looping && ((!m_returning && dist > m_range) || (m_returning && dist < 0.3f)))
            {
                m_returning = !m_returning;
                rb.velocity = new Vector3(m_speed * m_xDir, m_speed * m_yDir, 0f) * (m_returning ? 1 : -1);
            }
        }
    }


}
