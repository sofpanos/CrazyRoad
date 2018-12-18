using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreScript : MonoBehaviour {
	public GameObject Player;
	public static int FinalScore;
	private DateTime StartTime;
	private float StartPosition;
	private float PositionScore;
	private float multiplier = 1;
	private static float maxspeed = 0;
	
	// Use this for initialization
	void Start () {
		StartTime = DateTime.Now;
		StartPosition = Player.transform.position.y;
	}

	public void resetScore()
	{
		StartTime = DateTime.Now;
		StartPosition = Player.transform.position.y;
		switch (UIScript.LastDificulty)
		{
			case UIScript.Difficulty.Easy:
				multiplier = 1;
				break;
			case UIScript.Difficulty.Medium:
				multiplier = 10;
				break;
			case UIScript.Difficulty.Hard:
				multiplier = 100;
				break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (UIScript.GameOver)
		{
			TimeSpan time = DateTime.Now - StartTime;
			FinalScore =(int)PositionScore + (int)(time.TotalSeconds/5);
		}

		TimeSpan time2 = DateTime.Now - StartTime;
		float distance = Player.transform.position.y - StartPosition;
		maxspeed = Math.Max(distance / (float)time2.TotalSeconds, maxspeed);
		PositionScore = (Player.transform.position.y - StartPosition) * multiplier;
		GetComponent<Text>().text = "Score: " + (int)PositionScore;
	}
}
