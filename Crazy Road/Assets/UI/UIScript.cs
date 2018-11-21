using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour {
	public static bool GameOver = false;
	public static bool Won = false;
	public GameObject GameOverPanel;
	public GameObject WinPanel;
	// Use this for initialization
	void Start () {
		GameOver = false;
		Won = false;
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (GameOver)
		{
			Time.timeScale = 0f;
			GameOverPanel.SetActive(true);
		}
		else if (Won)
		{
			Time.timeScale = 0f;
			WinPanel.SetActive(true);
		}
		
	}

	public void OnPlayAgain()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}

	public void OnQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void PlayGameOverSound()
	{
		GameOverPanel.GetComponent<AudioSource>().Play();
	}
}
