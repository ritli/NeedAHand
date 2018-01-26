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
    
    // Properties
    #endregion

    #region private
    private static List<Body> m_players = new List<Body>();
    private static object m_currentMap;
    #endregion
}
