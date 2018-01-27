using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    #region public
    // Functions

    public static void init()
    {
        
    }

    // Input?
    public static void LoadLevel()
    {

    }

    // Input?
    public static void EndLevel()
    {

    }

    // Input/Output?
    public static void SetCheckpoint()
    {
        
    }

    public static void SpawnPlayers()
    {
        //GameObject player = (GameObject)Instantiate(Resources.Load("playerPrefab"));
        //m_players.Add(player);
    }

    public static void RespawnPlayer(GameObject player)
    {

    }

    // Properties
    #endregion

    #region private
    private static List<GameObject> m_players = new List<GameObject>();
    private static object m_currentMap;
    #endregion
}
