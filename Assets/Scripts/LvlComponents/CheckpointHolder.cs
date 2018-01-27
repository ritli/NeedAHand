using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckpointHolder : MonoBehaviour
{
    private List<CheckpointData> m_checkpoints = new List<CheckpointData>();
	// Use this for initialization
	void Start ()
    {
		foreach(Transform child in transform)
        {
            if (child.GetComponent<Checkpoint>() != null)
                m_checkpoints.Add(child.GetComponent<Checkpoint>().GetCPData());
        }

        GameManager._GetInstance().SetCheckpoint(m_checkpoints[0], 1);
        GameManager._GetInstance().SetCheckpoint(m_checkpoints[0], 2);
    }

    void sortCheckpointData()
    {
        m_checkpoints.OrderBy(o => o.pos.x);
        for (int i = 0; i < m_checkpoints.Count; i++)
        {
            CheckpointData tmp = m_checkpoints[i];
            tmp.id = i;
            m_checkpoints[i] = tmp;
        }
    }

    public void CheckpointEnter(Vector2 pos, int playerId)
    {
        GameManager._GetInstance().SetCheckpoint(m_checkpoints.Find(x => x.pos == pos), playerId);
    }

}
