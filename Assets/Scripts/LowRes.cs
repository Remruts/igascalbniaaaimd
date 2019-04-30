using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class LowRes : MonoBehaviour
{
	public int targetWidth;
	public int targetHeight;
	public bool stretch = false;
	public bool clearBK = true;

	private RenderTexture tex;
	private Camera cam;
	private Rect screenRect;

	public static LowRes screen;

	private void Start()
	{
		screen = this;
		screenRect = new Rect(0, 0, Screen.width, Screen.height);
		cam = GetComponent<Camera>();
		tex = new RenderTexture(targetWidth, targetHeight, 24);
		tex.filterMode = FilterMode.Point;
		cam.targetTexture = tex;
	}
	public Vector3 ScreenToCamPoint(Vector3 aScreenPoint)
	{
		aScreenPoint.x -= screenRect.x;
		aScreenPoint.y -= screenRect.y;
		aScreenPoint.x *= targetWidth / screenRect.width;
		aScreenPoint.y *= targetHeight / screenRect.height;
		return aScreenPoint;
	}

	public Vector3 WorldToScreen(Vector3 worldPoint){
		worldPoint -= cam.transform.position;
		worldPoint.y /= targetHeight * screenRect.height;
		worldPoint.x /= targetWidth * screenRect.width;

		worldPoint.x += screenRect.x;
		worldPoint.y += screenRect.y;

		return worldPoint;
	}

	void OnGUI ()
	{
		if (Event.current.type != EventType.Repaint)
			return;
		screenRect = new Rect(0,0,Screen.width, Screen.height);
		if (!stretch)
		{
			float sa = screenRect.width / screenRect.height;
			float ta = (float)tex.width / tex.height;
			if (sa > ta)
			{ // pillar box
				screenRect.width = screenRect.height * ta;
				screenRect.x = (Screen.width - screenRect.width) * 0.5f;
			}
			else
			{ // letter box
				screenRect.height = screenRect.width / ta;
				screenRect.y = (Screen.height - screenRect.height) * 0.5f;
			}
		}
		if (clearBK)
			GL.Clear(true, true, Color.black);
		GUI.DrawTexture(screenRect, tex);
	}
}
