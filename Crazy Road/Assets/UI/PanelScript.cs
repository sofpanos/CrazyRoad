using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class PanelScript : MonoBehaviour {

	public void PlayGameOverSound()
	{
		GetComponent<AudioSource>().Play();
		transform.GetChild(1).GetComponent<Text>().text = "Final Score: " + ScoreScript.FinalScore;
	}

	public void DifficultyChosen()
	{
		gameObject.SetActive(false);
	}
}
