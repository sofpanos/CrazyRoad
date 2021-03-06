﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

public class AJAXScript : MonoBehaviour {

	private const string ScoreboardURL = "https://users.it.teithe.gr/~it144287/CrazyRoad/Score.php";

	//Submit Fields
	public GameObject SubmitPanel;
	public GameObject HighscoresPanel;
	public Text SubmitText;
	public GameObject UsernameInput;
	public GameObject PINInput;
	public GameObject NamesAndScores;
	public GameObject GameOverPanel;
	public GameObject PinPanel;
	public GameObject UsernamePanel;
	public GameObject SubmitButton;
	public GameObject ChangeUserButton;

	private static bool LoggedIn = false;
	private static string username = "";
	private static int pin = 0;
	private bool submitted = false;
	private DateTime SubmitTime = DateTime.MinValue;

	private const string LoginMessage = "Login/Sign up to Submit your score";
	private const string LoggedinMessage = "Click Submit to submit your score or Change User to switch to another user";


	void OnEnable()
	{
		UpdateSubmitUI();
	}

	public void OnSubmitClicked()
	{
		StartCoroutine(SubmitRequest());
	}

	private IEnumerator SubmitRequest()
	{
		WWWForm httpForm = new WWWForm();
		if (!LoggedIn)
		{
			username = UsernameInput.GetComponent<InputField>().text;
			pin = int.Parse(s: PINInput.GetComponent<InputField>().text);
		}
		httpForm.AddField("requestType", "submit");
		httpForm.AddField("username", username);
		httpForm.AddField("pin", pin);
		httpForm.AddField("score", ScoreScript.FinalScore);

		using (var httpRequest = UnityWebRequest.Post(ScoreboardURL, httpForm))
		{
			yield return httpRequest.SendWebRequest();

			if (!httpRequest.isNetworkError && !httpRequest.isHttpError)
			{
				HandleSubmitResponseText(httpRequest.downloadHandler.text);
			}
			else
			{
				SubmitText.text = httpRequest.error;
			}
		}

	}

	private void Update()
	{
		if (submitted)
		{
			if(SubmitTime == DateTime.MinValue)
			{
				SubmitTime = DateTime.Now;
			}
			else if(DateTime.Now - SubmitTime > new TimeSpan(0, 0, 5))
			{
				OnScoreBoard();
				SubmitPanel.SetActive(false);
				HighscoresPanel.SetActive(true);
				//reset values
				submitted = false;
				SubmitTime = DateTime.MinValue;
			}

		}
	}
	private void HandleSubmitResponseText(string responseText)
	{
		if (responseText == "Score Submitted" || responseText.StartsWith("Your Highscore"))
		{
			SubmitText.text = responseText;
			LoggedIn = true;
			submitted = true;
		}
		else if (responseText.StartsWith("SQL Error"))
		{
			SubmitText.text = responseText;
			username = "";
			pin = 0;
		}
		else if (responseText.Equals("Authorization Failed"))
		{
			SubmitText.text = "Wrong Password or Username, try again(Login). Or username already in use(SignUp).";
		}
		else
		{
			SubmitText.text = "Something Went Wrong Please Contact support";
			SubmitText.text = responseText;
		}
		
	}

	public void OnScoreBoard()
	{
		StartCoroutine(GetScoreBoard());
	}

	private IEnumerator GetScoreBoard()
	{
		WWWForm httpForm = new WWWForm();

		httpForm.AddField("requestType", "scoreboard");
		httpForm.AddField("username", username);
		httpForm.AddField("pin", pin);

		using (var httpRequest = UnityWebRequest.Post(ScoreboardURL, httpForm))
		{
			yield return httpRequest.SendWebRequest();
			if (!httpRequest.isNetworkError && !httpRequest.isHttpError)
			{
				HandleScoreboardResponseText(httpRequest.downloadHandler.text);
			}

		}
	}

	private void HandleScoreboardResponseText(string text)
	{
		text = text.Replace(",", "},{");
		Dictionary<string, int> scores = JSONParser(text);

		IEnumerator children = NamesAndScores.transform.GetEnumerator();
		foreach (KeyValuePair<String, int> pair in scores)
		{
			if (children.MoveNext())
			{
				Regex rx = new Regex(@"\\[uU]([0-9A-Fa-f]{4})");
				string result = rx.Replace(pair.Key, match => ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());

				((Transform)children.Current).gameObject.GetComponent<Text>().text = result;
			}
			if (children.MoveNext())
			{
				((Transform)children.Current).gameObject.GetComponent<Text>().text = pair.Value.ToString();
			}
		}
		while (children.MoveNext())
		{
			((Transform)children.Current).gameObject.GetComponent<Text>().text = "";
		}
	}

	private Dictionary<string, int> JSONParser(string text)
	{
		text = text.Replace("}", "");
		text = text.Replace("{", "");

		Dictionary<string, int> dict = new Dictionary<string, int>();
		string[] keyValuePairs = text.Split(',');
		foreach (string pair in keyValuePairs)
		{
			int seperator = pair.IndexOf(":");
			//Parsing Key
			string Kval = pair.Substring(0, seperator);
			Kval = Kval.Replace("\"", "");
			Kval = Kval.Trim();

			//Parsing Value
			string strVal = pair.Substring(seperator + 1);
			strVal = strVal.Trim();
			int val = int.Parse(strVal);

			dict.Add(Kval, val);
		}

		return dict;
	}

	public void OnBack()
	{
		PanelScript.PlaySound = false;
		GameOverPanel.SetActive(true);
		SubmitPanel.SetActive(true);
		HighscoresPanel.SetActive(false);
		gameObject.SetActive(false);
	}

	private void UpdateSubmitUI()
	{
		if (LoggedIn)
		{
			if (UsernamePanel.activeInHierarchy || PinPanel.activeInHierarchy || !ChangeUserButton.activeInHierarchy)
			{
				ChangeUserButton.SetActive(true);
				UsernamePanel.SetActive(false);
				PinPanel.SetActive(false);
			}
			SubmitText.text = LoggedinMessage;
		}
		else
		{
			if (!(UsernamePanel.activeInHierarchy && PinPanel.activeInHierarchy && !ChangeUserButton.activeInHierarchy))
			{
				ChangeUserButton.SetActive(false);
				UsernamePanel.SetActive(true);
				PinPanel.SetActive(true);
			}
			SubmitText.text = LoginMessage;
		}
	}

	public void OnChangeUser()
	{
		LoggedIn = false;
		username = "";
		pin = 0;
		UpdateSubmitUI();
	}
}
