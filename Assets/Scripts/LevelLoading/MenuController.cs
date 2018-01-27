using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void Load(int sceneIndex)
	{
		GameManager._GetInstance().LoadLevel(sceneIndex);
	}
	public void Quit()
	{
		Application.Quit();
	}
}
