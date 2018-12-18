using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class PanelScript : MonoBehaviour {
	public static bool PlaySound = true;
	public void PlayGameOverSound()
	{
		if(PlaySound)
		{
			GetComponent<AudioSource>().Play();
		}
		else
		{
			PlaySound = true;
		}
		if (UIScript.LastMode == UIScript.Mode.Survival)
		{
			transform.GetChild(1).GetComponent<Text>().text = "Final Score: " + ScoreScript.FinalScore;
			transform.GetChild(2).gameObject.SetActive(true);
		}
		else
		{
			transform.GetChild(2).gameObject.SetActive(false);
			transform.GetChild(1).GetComponent<Text>().text = "";
		}
	}

	public void DifficultyChosen()
	{
		gameObject.SetActive(false);
	}
}
