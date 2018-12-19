using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundScript : MonoBehaviour {
	public static bool Mute = false;
	public Sprite MutedImage;
	public Sprite UnmutedImage;
	public void OnClick()
	{
		Mute = !Mute;

		if (Mute)
		{
			GetComponent<Image>().sprite = MutedImage;
			AudioListener.volume = 0f;
		}
		else
		{
			GetComponent<Image>().sprite = UnmutedImage;
			AudioListener.volume = 1f;
		}
	}
}
