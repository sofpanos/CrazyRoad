using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using System;

public class AJAXScript : MonoBehaviour {

	private const string ScoreboardURL = "https://users.it.teithe.gr/~it144287/CrazyRoad/Score.php";

	//Submit Fields
	public GameObject SubmitPanel;
	public GameObject HighscoresPanel;
	public GameObject SubmitText;
	public GameObject UsernameInput;
	public GameObject PINInput;
	public GameObject NamesAndScores;
	public GameObject GameOverPanel;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnSubmitClicked()
	{
		StartCoroutine(OnSubmit());
	}
	public IEnumerator OnSubmit()
	{
		WWWForm httpForm = new WWWForm();

		httpForm.AddField("requestType", "submit");
		httpForm.AddField("username", UsernameInput.GetComponent<InputField>().text);
		httpForm.AddField("pin", int.Parse(s: PINInput.GetComponent<InputField>().text));
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
				HandleSubmitResponseText(httpRequest.downloadHandler.text);
			}
		}

	}

	private void HandleSubmitResponseText(string responseText)
	{
		if(responseText == "Score Submitted")
		{
			SubmitText.GetComponent<Text>().text = responseText;
			
		}
		else if(responseText.StartsWith("SQL Error"))
		{
			SubmitText.GetComponent<Text>().text = responseText;
		}
		else if(responseText.Equals("Authentication Failed"))
		{
			SubmitText.GetComponent<Text>().text = "Wrong Password or Username";
		}
		else
		{
			SubmitText.GetComponent<Text>().text = "Something Went Wrong Please Contact support";
		}
		OnScoreBoard();
		DateTime start = DateTime.Now;
		while(DateTime.Now - start < new TimeSpan(0, 0, 5))
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
			if(!httpRequest.isNetworkError && !httpRequest.isHttpError)
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
		foreach(KeyValuePair<String, int> pair in scores)
		{
			if (children.MoveNext())
			{
				((Transform)children.Current).gameObject.GetComponent<Text>().text = pair.Key;
			}
			if(children.MoveNext())
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
		foreach(string pair in keyValuePairs)
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
