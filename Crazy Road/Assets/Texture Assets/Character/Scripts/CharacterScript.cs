using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {
	public float MaxSpeed = 10f;
	private Rigidbody2D body;
	private Animator anim;
	public Camera mainCam;
	
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		mainCam.transform.position = new Vector3(transform.position.x, transform.position.y, mainCam.transform.position.z);
	}

	private void RotatePlayer()
	{
		float angle = Mathf.Atan2(-body.velocity.x, body.velocity.y) * Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	private void FixedUpdate()
	{
		float moveX = Input.GetAxis("Horizontal") * MaxSpeed;
		float moveY = Input.GetAxis("Vertical") * MaxSpeed;
		body.velocity = new Vector2(moveX, moveY);
		RotatePlayer();
		anim.SetFloat("speed", Mathf.Abs(body.velocity.magnitude));
	}
}
