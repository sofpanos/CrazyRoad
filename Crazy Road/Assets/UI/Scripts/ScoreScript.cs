using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScoreScript : MonoBehaviour {
	public GameObject Player;
	public static int FinalScore;
	private float StartPosition;
	private float PositionScore;
	private float multiplier = 1;
	
	// Use this for initialization
	void Start () {
		StartPosition = Player.transform.position.y;
	}

	public void resetScore()
	{
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
			FinalScore =(int)PositionScore;
		}

		PositionScore = (Player.transform.position.y - StartPosition) * multiplier;
		GetComponent<Text>().text = "Score: " + (int)PositionScore;
	}
}
