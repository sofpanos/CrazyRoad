using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PanelScript : MonoBehaviour {

	public void PlayGameOverSound()
	{
		GetComponent<AudioSource>().Play();
	}

	public void DifficultyChosen()
	{
		gameObject.SetActive(false);
	}
}
