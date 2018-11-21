using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenScript : MonoBehaviour {
	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	public void startSiren()
	{
		anim.SetBool("siren", true);
	}
}
