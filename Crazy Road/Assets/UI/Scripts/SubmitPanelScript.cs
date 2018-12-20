using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using System;

public class ScoresPanelScript : MonoBehaviour
{

	private const string ScoreboardURL = "https://users.it.teithe.gr/~it144287/CrazyRoad/Score.php";

	//Submit Panel Objects
	public GameObject SubmitPanel;
	public GameObject HighscoresPanel;
	public GameObject GameOverPanel;
	public GameObject PinPanel;
	public GameObject UsernamePanel;
	//Submit Text Objects
	public Text SubmitText;
	//Submit Input Objects
	public GameObject UsernameInput;
	public GameObject PINInput;
	//Highscores Table Object
	public GameObject NamesAndScores;
	//Submit Button
	public GameObject SubmitButton;

	private bool LoggedIn = false;
	private string username = "";
	private int pin = 0;

	private const string LoginMessage = "Login/Sign up to Submit your score";
	private const string LoggedinMessage = "Your Score is being submitted please wait";


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	
	public void OnSubmitClicked()
	{
		StartCoroutine(OnSubmit());
	}
	public IEnumerator OnSubmit()
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

	private void HandleSubmitResponseText(string responseText)
	{
		if (responseText == "Score Submitted")
		{
			SubmitText.text = responseText;
			LoggedIn = true;
		}
		else if (responseText.StartsWith("SQL Error"))
		{
			SubmitText.text = responseText;
			username = "";
			pin = 0;
		}
		else if (responseText.Equals("Authentication Failed"))
		{
			SubmitText.text = "Wrong Password or Username";
		}
		else
		{
			SubmitText.text = "Something Went Wrong Please Contact support";
		}
		OnScoreBoard();
		DateTime start = DateTime.Now;
		while (DateTime.Now - start < new TimeSpan(0, 0, 5))
		{

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
		httpForm.AddField("username", UsernameInput.GetComponent<InputField>().text);
		httpForm.AddField("pin", int.Parse(PINInput.GetComponent<InputField>().text));

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
				((Transform)children.Current).gameObject.GetComponent<Text>().text = pair.Key;
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
		SubmitPanel.SetActive(false);
		HighscoresPanel.SetActive(true);
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
}
