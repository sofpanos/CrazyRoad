using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

	public void PlayGameOverSound()
	{
		GetComponent<AudioSource>().Play();
	}
}
