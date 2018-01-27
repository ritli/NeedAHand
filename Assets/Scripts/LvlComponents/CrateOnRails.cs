using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateOnRails : MonoBehaviour {

    private Vector3 m_startPos;
    private bool m_acitve = false;
    private float m_range, m_speed;
    private int m_xDir, m_yDir;
    private bool m_looping;
    private bool m_returning = false;

	// Use this for initialization
	void Start ()
    {
        m_startPos = transform.position;
	}
	
	public void Activate(float range, float speed, int xDir, int yDir, bool loop)
    {
        m_acitve = true;
        m_range = range;
        m_speed = speed;
        m_xDir = xDir;
        m_yDir = yDir;
        m_looping = loop;
    }

    void FixedUpdate()
    {
        /*
        if(m_acitve)
        {
            Vector3 offset = m_startPos - transform.position;
            float dist = offset.sqrMagnitude;
            if(dist<m_range)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3(m_speed * m_xDir, m_speed * m_yDir, 0f);
            }
            else if(m_returning && dist > 0.3f)
            {
                rigidbody.Velocity = Vector3(m_speed * m_xDir * -1, m_speed * m_yDir * -1, 0);
            }
            else if(m_looping && ((!m_returning && dist>m_range) || (m_returning && dist<0.3f)))
            {
                m_returning = !m_returning;
                m_returning? rigidbody.velocity = Vector3(m_speed * m_xDir * -1, m_speed * m_yDir * -1, 0):rigidbody.Velocity = Vector3(m_speed * m_xDir, m_speed * m_yDir, 0);
            }
        }
        */
    }


}
