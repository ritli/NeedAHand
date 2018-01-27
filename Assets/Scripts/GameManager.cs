using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region public
    // Functions

	public static GameManager _GetInstance()
	{
		return m_Instance;
	}

	private void Start()
	{
		GameManager[] managers = FindObjectsOfType<GameManager>();
		if (managers.Length < 1)
		{
			Destroy(gameObject);
		}
		m_Instance = managers[0];
		Debug.Log("One GameManager is present.");
	}

	// Input?
	public void LoadLevel(int levelIndex)
    {
		StartCoroutine(TransitionController._GetInstance().Transition(levelIndex));
    }

    // Input?
    public void EndLevel()
    {
		//For now, return to main menu
		StartCoroutine(TransitionController._GetInstance().Transition(1));
	}

    // Input/Output?
    public void SetCheckpoint()
    {
        
    }
	public void SetStartPoint(GameObject point)
	{

	}
	public void SetEndPoint(GameObject point)
	{

	}

    public void SpawnPlayers()
    {
        //GameObject player = (GameObject)Instantiate(Resources.Load("playerPrefab"));
        //m_players.Add(player);
    }

    public void RespawnPlayer(GameObject player)
    {

    }

    // Properties

    #endregion

    #region private
    private List<GameObject> m_players = new List<GameObject>();
    private object m_currentMap;
	private static GameManager m_Instance;

    #endregion
}
