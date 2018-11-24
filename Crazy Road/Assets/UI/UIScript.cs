using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour {
	//Game State
	public static bool GameOver = false;
	public static bool Won = false;
	public static bool Entry = true;
	public static bool Paused = false;

	private static bool PlayAgain = false;
	public static Difficulty LastDificulty;
	private static Mode LastMode;

	private enum Mode
	{
		Normal,
		Survival
	}

	public enum Difficulty
	{
		Easy,
		Medium,
		Hard
	}

	//UI
	public GameObject Score;

	//GameWorlds
	public GameObject Normal;
	public GameObject Survival;

	//Panels
	public GameObject EntryPanel;
	public GameObject DifficultyPanel;
	public GameObject HelpPanel;
	public GameObject CreditsPanel;
	public GameObject GameOverPanel;
	public GameObject WinPanel;
	
	// Use this for initialization
	void Start () {
		GameOver = false;
		Won = false;
		Entry = true;
		Time.timeScale = 0f;
		if (PlayAgain)
		{
			if(LastMode == Mode.Survival)
			{
				Normal.SetActive(false);
				Survival.SetActive(true);
			}
			switch (LastDificulty)
			{
				case Difficulty.Easy:
					OnEasy();
					PlayAgain = false;
					break;
				case Difficulty.Medium:
					OnMedium();
					PlayAgain = false;
					break;
				case Difficulty.Hard:
					OnHard();
					PlayAgain = false;
					break;
			}
		}
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

	//Win - Game Over Panel Actions

	public void OnMenu()
	{
		Entry = true;
		Paused = GameOver = Won = false;
		EntryPanel.SetActive(true);
		SceneManager.LoadScene(0);
	}

	public void OnPlayAgain()
	{
		Time.timeScale = 1f;
		PlayAgain = true;
		SceneManager.LoadScene(0);
	}

	
	public void PlayGameOverSound()
	{
		GameOverPanel.GetComponent<AudioSource>().Play();
	}

	
	
	public void OnBack()
	{
		if (HelpPanel.activeSelf)
		{
			HelpPanel.SetActive(false);
		}
		else if (CreditsPanel.activeSelf)
		{
			CreditsPanel.SetActive(false);
		}
		EntryPanel.SetActive(true);
	}

	
	//Entry Panel Actions///
	public void OnNormalMode()
	{
		LastMode = Mode.Normal;
		Normal.SetActive(true);
		Survival.SetActive(false);
		EntryPanel.SetActive(false);
		DifficultyPanel.SetActive(true);
	}

	public void OnSurvivalMode()
	{
		LastMode = Mode.Survival;
		Normal.SetActive(false);
		Survival.SetActive(true);
		EntryPanel.SetActive(false);
		DifficultyPanel.SetActive(true);
	}
	public void OnHelp()
	{
		HelpPanel.SetActive(true);
		EntryPanel.SetActive(false);
	}

	public void OnCredits()
	{
		CreditsPanel.SetActive(true);
		EntryPanel.SetActive(false);
	}

	public void OnQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	///// Difficulty Panel Actions //////
	public void OnEasy()
	{
		
		Time.timeScale = 1f;
		CarGeneratorScript.CarInterval = new System.TimeSpan(0, 0, 0, 3);
		LastDificulty = Difficulty.Easy;
		Entry = false;
		if (LastMode == Mode.Survival)
		{
			Score.SetActive(true);
			Score.GetComponent<ScoreScript>().resetScore();
		}

		if (PlayAgain)
		{
			EntryPanel.SetActive(false);
		}
		else
		{
			DifficultyPanel.GetComponent<Animator>().SetBool("close", true);
		}
	}

	public void OnMedium()
	{
		Time.timeScale = 1f;
		CarGeneratorScript.CarInterval = new System.TimeSpan(0, 0, 0, 2, 200);
		LastDificulty = Difficulty.Medium;
		if (LastMode == Mode.Survival)
		{
			Score.SetActive(true);
			Score.GetComponent<ScoreScript>().resetScore();
		}
		Entry = false;
		if (PlayAgain)
		{
			EntryPanel.SetActive(false);
		}
		else
		{
			DifficultyPanel.GetComponent<Animator>().SetBool("close", true);
		}
	}

	public void OnHard()
	{
		Time.timeScale = 1f;
		CarGeneratorScript.CarInterval = new System.TimeSpan(0, 0, 0, 1, 500);
		LastDificulty = Difficulty.Hard;
		if (LastMode == Mode.Survival)
		{
			Score.SetActive(true);
			Score.GetComponent<ScoreScript>().resetScore();
		}

		Entry = false;
		if (PlayAgain)
		{
			EntryPanel.SetActive(false);
		}
		else
		{
			DifficultyPanel.GetComponent<Animator>().SetBool("close", true);
		}
	}

}
