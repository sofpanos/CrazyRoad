using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class PanelScript : MonoBehaviour {

	public void PlayGameOverSound()
	{
		GetComponent<AudioSource>().Play();
		if (UIScript.LastMode == UIScript.Mode.Survival)
		{
			transform.GetChild(1).GetComponent<Text>().text = "Final Score: " + ScoreScript.FinalScore;
		}
		else
		{
			transform.GetChild(1).GetComponent<Text>().text = "";
		}
	}

	public void DifficultyChosen()
	{
		gameObject.SetActive(false);
	}
}
