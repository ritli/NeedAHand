using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    #region public
    // Functions
    public static void init()
    {
        LoadLevel();
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

    public static void SpawnPlayer()
    {

    }

    // Properties
    #endregion

    #region private
    private static List<Body> m_players = new List<Body>();
    private static object m_currentMap;
    #endregion
}
