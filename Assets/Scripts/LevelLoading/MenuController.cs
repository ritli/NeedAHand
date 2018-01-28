using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{


    public void Load(int sceneIndex)
    {
        GameManager._GetInstance().LoadLevel(sceneIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
