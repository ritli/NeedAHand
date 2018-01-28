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

        m_checkpoints.OrderBy(p => p.id);

        if (m_checkpoints[0].id < 0)
        {
            for (int i = 0; i < 100; i++)
                print("CHECKPOINT ID < 0. U DUN FUCKED UP BOI.");
        }

        GameManager._GetInstance().SetCheckpoint(m_checkpoints[0], 1);
        GameManager._GetInstance().SetCheckpoint(m_checkpoints[0], 2);
    }

    public void CheckpointEnter(Vector2 pos, int playerId)
    {
        GameManager._GetInstance().SetCheckpoint(m_checkpoints.Find(x => x.pos == pos), playerId);
    }

}
