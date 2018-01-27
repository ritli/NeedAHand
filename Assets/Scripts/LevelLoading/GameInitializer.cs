using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

	public bool startFromMenu = true;


	// Use this for initialization of the game
	void Start () {
		Debug.Log("initializing game.");
		if(startFromMenu == true)
		GameManager._GetInstance().LoadLevel(1);

		//Destroy after init
		Destroy(gameObject);
	}	
}
