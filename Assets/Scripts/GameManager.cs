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
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
	private void Start()
	{
		Debug.Log("initializing game.");
		//remove additional Managers
		GameManager[] managers = FindObjectsOfType<GameManager>();
		if (managers.Length < 1)
		{
			Destroy(gameObject);
		}
		m_Instance = managers[0];

		//start from menu or not
		if (startFromMenu == true)
			LoadLevel(1);


		//create players, hide them and store them in m_players
		m_players = new List<GameObject>();

		GameObject p1 = Instantiate(playerprefab);
		p1.GetComponent<Body>().playerID = 1;
		p1.SetActive(false);
		p1.name = "Player1";
		m_players.Add(p1.gameObject);

		GameObject p2 = Instantiate(playerprefab);
		p2.GetComponent<Body>().playerID = 2;
		p2.SetActive(false);
		p2.name = "Player2";
		m_players.Add(p2.gameObject);
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
    public void SetCheckpoint(GameObject point)
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
		m_players.ForEach(player => {
			player.SetActive(true);
		});
    }
	void OnLevelFinishedLoading(Scene level, LoadSceneMode mode)
	{
		Debug.Log("Spawning players");

		if(level.buildIndex != 0 & level.buildIndex != 1)
		{
			SpawnPlayers();
		}
		//TODO: Place players at start
	}

	public void RespawnPlayer(GameObject player)
    {

    }

	// Properties
	public bool startFromMenu = true;
	public GameObject playerprefab;

	#endregion

	#region private
	private List<GameObject> m_players = new List<GameObject>();
    private object m_currentMap;
	private static GameManager m_Instance;

    #endregion
}
