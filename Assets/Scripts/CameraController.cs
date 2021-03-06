﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public float maxSize = 8;
	public float minSize = 4;
	public float moveSpeed = 6;
	public float zoomSpeed = 3;
	public float minSizeY = 5;
	public float padding = 5;

	public Transform player1;
	public Transform player2;
	[Range(0, 0.4f)]
	public float shakeForce;
	public float shakeTime;
	private Vector3 targetPos;

	void Start()
	{
		//Invoke("DelayedStart", 0.1f);
		List<GameObject> players = GameManager._GetInstance().GetPlayers();
		player1 = players[0].transform;
		player2 = players[1].transform;

	}

	void DelayedStart()
	{
		foreach (Body b in FindObjectsOfType<Body>())
		{
			switch (b.playerID)
			{
				case 1:
					player1 = b.transform;
					break;
				case 2:
					player2 = b.transform;
					break;

				default:
					break;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		MoveCamera();
		SetCameraSize();
	}
	void MoveCamera()
	{
		//Interpolate centerposition with current position
		if (player1.gameObject.activeSelf || player2.gameObject.activeSelf)
			targetPos = GetCenterPosition();
		else
			targetPos = Camera.main.transform.position;
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, moveSpeed * Time.deltaTime);
	}
	Vector3 GetCenterPosition()
	{
		//finds centerpoint between players and oplaces the camera at that position
		Vector3 centerPoint = new Vector3();
		int activePlayers = 0;
		if (player1.gameObject.activeSelf)
		{
			activePlayers++;
			centerPoint += player1.position;
		}
		if (player2.gameObject.activeSelf)
		{
			activePlayers++;
			centerPoint += player2.position;
		}

		if (activePlayers == 2)
		{
			centerPoint *= 0.5f;
		}

		return new Vector3(centerPoint.x, centerPoint.y, Camera.main.transform.position.z);
	}
	void SetCameraSize()
	{
		//Scales the screen to allow both players to be seen
		float minSizeX = Screen.width / Screen.height;
		float minSizeY = Screen.height / Screen.width;

		//Padding
		float width;
		float height;

		//special cases for when player1 is south or east to player2
		if (player1.position.x < player2.position.x)
			width = Mathf.Abs((player2.position.x - player1.position.x) + padding) * 0.5f;
		else
			width = Mathf.Abs((player1.position.x - player2.position.x) + padding) * 0.5f;

		if (player1.position.y < player2.position.y)
			height = Mathf.Abs((player2.position.y - player1.position.y) + padding) * 0.5f;
		else
			height = Mathf.Abs((player1.position.y - player2.position.y) + padding) * 0.5f;

		float camSizeX = Mathf.Max(width, minSizeX);
		float camSizeY = Mathf.Max(height, minSizeY);


		//Interpolate screen size
		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Mathf.Max(height, camSizeX * Screen.height / Screen.width, camSizeY), zoomSpeed * Time.deltaTime);

		//Limit screen size
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minSize, maxSize);
	}
	public void StartShake()
	{
		StartCoroutine(ShakeScreen(shakeForce, shakeTime));
	}
	private IEnumerator ShakeScreen(float maxVector, float shakeTime)
	{
		Debug.Log("Shaking");
		float time = shakeTime;

		for (;;)
		{
			Vector3 rndVector = new Vector3(Random.Range(2, -2), Random.Range(-1, 1), 0);
			rndVector *= maxVector;

			Camera.main.transform.position += rndVector;

			if (shakeTime < 0)
			{
				yield break;
			}
			shakeTime -= Time.deltaTime;

			yield return null;
		}
	}
}
