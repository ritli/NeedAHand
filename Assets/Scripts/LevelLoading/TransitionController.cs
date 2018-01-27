using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour {

	public Image black;
	public float transitionSpeed;
	private bool isInTransition = false;

	private static TransitionController m_Instance;

	public static TransitionController _GetInstance()
	{
		return m_Instance;
	}

	// Use this for initialization
	void Start () {
		TransitionController[] controllers = FindObjectsOfType<TransitionController>();
		if (controllers.Length < 1)
		{
			Destroy(gameObject);
		}
		m_Instance = controllers[0];
		Debug.Log("One TransitionController is present.");


		//clear blackplate
		black.color = Color.clear;
	}

	public IEnumerator Transition(int index)
	{

		if (isInTransition)
		{
			yield break;
		}
		isInTransition = true;
		//transition out
		for (float f = 0f; f <= 1; f += transitionSpeed * Time.deltaTime)
		{
			Color c = black.color;
			c.a = f;
			black.color = c;
			yield return null;
		}

		//loadLevel
		for(int i = 0; i < SceneManager.sceneCount; i++)
		{
			if(SceneManager.GetSceneAt(i).buildIndex != 0)
			{
				SceneManager.UnloadSceneAsync(i);
			}
		}
		SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

		//transition in
		for (float f = 1f; f >= 0; f -= transitionSpeed * Time.deltaTime)
		{
			Color c = black.color;
			c.a = f;
			black.color = c;
			yield return null;
		}
		isInTransition = false;
	}
		//jag gillar furry porr hehehe jk detta är en kodrape (I'll leave this.. for now..) 27/1
}
