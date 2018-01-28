using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region public
    // Functions
	public List<GameObject> GetPlayers()
	{
		return m_players;
	}
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
		Debug.Log("Initializing game.");
		p1Checkpoint = new CheckpointData();
		p1Checkpoint.id = -1;
		p2Checkpoint = new CheckpointData();
		p2Checkpoint.id = -1;

		//remove additional Managers
		GameManager[] managers = FindObjectsOfType<GameManager>();
		if (managers.Length > 1)
		{
			Destroy(gameObject);
		}
		m_Instance = managers[0];

		//start from menu or not
		if (startFromMenu == true)
			LoadLevel(1);


		//create players, hide them and store them in m_players
		m_players = new List<GameObject>();

        if (playerprefab == null)
        {
            playerprefab = Resources.Load<GameObject>("Player");
        }

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
        p1Checkpoint = new CheckpointData();
        p1Checkpoint.id = -1;
        p2Checkpoint = new CheckpointData();
        p2Checkpoint.id = -1;

        StartCoroutine(TransitionController._GetInstance().Transition(levelIndex));
		currentLevelIndex = levelIndex;
    
    }

    // Input?
    public void EndLevel()
    {
		//For now, return to main menu
		currentLevelIndex++;
		StartCoroutine(TransitionController._GetInstance().Transition(currentLevelIndex));

        p1Checkpoint = new CheckpointData();
        p1Checkpoint.id = -1;
        p2Checkpoint = new CheckpointData();
        p2Checkpoint.id = -1;
    }

    // Input/Output?
    public void SetCheckpoint(CheckpointData point, int playerID)
    {
		Debug.Log("Sets checkpoint for " + playerID);
		switch (playerID)
		{
			case 1:
				if (point.id > p1Checkpoint.id)
					p1Checkpoint = point;
				break;
			case 2:
				if (point.id > p2Checkpoint.id)
					p2Checkpoint = point;
				break;
		}
    }

    public void SpawnPlayers()
    {
		m_players.ForEach(player => {
			player.SetActive(true);
            CheckpointData p = player.GetComponent<Body>().playerID == 1 ? p1Checkpoint : p2Checkpoint;
            player.GetComponent<Body>().SetStartingLimbs(p.legCount, p.armCount, true);
            player.transform.position = p.pos;
		});

		//m_players.ForEach(player =>
		//{
		//	RespawnPlayer(player);
		//});
	}
	public void OnLevelFinishedLoading(Scene level, LoadSceneMode mode)
	{
		if(level.buildIndex != 0 & level.buildIndex != 1)
		{
			Invoke("SpawnPlayers", playerSpawnDelay);
		}
	}

	public void RespawnPlayer(GameObject player)
    {
		Camera.main.GetComponent<CameraController>().StartShake();
		player.gameObject.SetActive(false);
        player.GetComponent<Body>().PlayDeathSound();

        if (player.GetComponent<Body>().playerID == 2)
        {
            ParticleHandler.SpawnParticleSystem(player.transform.position, "p_bluedeath");
        }
        else
        {
            ParticleHandler.SpawnParticleSystem(player.transform.position, "p_death");
        }


        player.transform.position = (player.GetComponent<Body>().playerID == 1 ? p1Checkpoint.pos : p2Checkpoint.pos);
		StartCoroutine(DelaySpawn(player));
    }

    public void TrackLimb(Limb limb)
    {
        m_trackedLimbs.Add(new KeyValuePair<Limb, int>(limb, limb.gameObject.transform.parent.GetComponent<Body>().playerID));
    }

  

	private IEnumerator DelaySpawn(GameObject player)
	{
		yield return new WaitForSeconds(spawnDelay);
		player.SetActive(true);

        player.GetComponent<Body>().ResetAnimator();
        CheckpointData p = player.GetComponent<Body>().playerID == 1 ? p1Checkpoint : p2Checkpoint;
        player.GetComponent<Body>().SetStartingLimbs(p.legCount, p.armCount, true);
	}

	// Properties
	public float spawnDelay = 2;
	public bool startFromMenu = true;
	public GameObject playerprefab;
	public float playerSpawnDelay;
	#endregion

	#region private
	private List<GameObject> m_players = new List<GameObject>();
    private List<KeyValuePair<Limb, int>> m_trackedLimbs = new List<KeyValuePair<Limb, int>>();
    private object m_currentMap;
	private CheckpointData p1Checkpoint;
	private CheckpointData p2Checkpoint;
	private int currentLevelIndex;
	private static GameManager m_Instance;

    #endregion
}
