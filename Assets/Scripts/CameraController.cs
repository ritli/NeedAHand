using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float maxSize = 8;
	public float minSize = 4;
	public float moveSpeed = 6;
	public float zoomSpeed = 3;
	public float padding = 5;

	public Transform player1;
	public Transform player2;

	
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveCamera();
		SetCameraSize();
	}
	void MoveCamera()
	{
		//Interpolate centerposition with current position
		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, GetCenterPosition(), moveSpeed * Time.deltaTime);
	}
	Vector3 GetCenterPosition()
	{
		//finds centerpoint between players and oplaces the camera at that position
		Vector3 centerPoint = player1.position + player2.position;
		centerPoint *= 0.5f;

		return new Vector3(centerPoint.x, centerPoint.y, Camera.main.transform.position.z);
	}
	void SetCameraSize()
	{
		//Scales the screen to allow both players to be seen
		float minSizeX = Screen.width / Screen.height;

		//Padding
		float width = Mathf.Abs((player1.position.x - player2.position.x) + padding) * 0.5f;
		float height = Mathf.Abs((player1.position.y - player2.position.y) + padding) * 0.5f;
		float camSizeX = Mathf.Max(width, minSizeX);

		//Interpolate screen size
		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Mathf.Max(height, camSizeX * Screen.height / Screen.width), zoomSpeed * Time.deltaTime);
		
		//Limit screen size
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minSize, maxSize);
	}
}
