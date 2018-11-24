using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour {
	
	public Transform VehicleInFrontCheck;
	public LayerMask VehicleMask;
	public bool Flipped = false;
	public float VehicleCheckRadius = 0.4f;
	public float breakingForceStep = 0.45f;
	private GameObject Player;
	
	private float startingSpeed = 0;
	
	

	private float accelarationStep = 0.3f;
	private bool isThereVehicle = false;
	private Rigidbody2D body;

	private void Start()
	{
		body = GetComponent<Rigidbody2D>();
		Player = GameObject.Find("Player");
		
	}

	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.layer == Player.layer)
		{
			Player.GetComponent<AudioSource>().Play();
			UIScript.GameOver = true;
		}
	}

	private void Update()
	{
		if (Flipped)
		{
			if(transform.position.x < -14f)
			{
				Destroy(transform.gameObject);
			}
		}
		else
		{
			if(transform.position.x > 14f)
			{
				Destroy(transform.gameObject);
			}
		}
		if (startingSpeed < CarGeneratorScript.MinSpeed && startingSpeed > -CarGeneratorScript.MinSpeed)
		{
			startingSpeed = GetComponent<Rigidbody2D>().velocity.x;
		}
		
	}

	private void FixedUpdate()
	{
		if (UIScript.GameOver || UIScript.Won || UIScript.Paused || UIScript.Entry)
		{
			AudioSource siren = null;
			try
			{
				siren = GetComponent<AudioSource>();
			}
			catch (MissingComponentException e)
			{

			}
			if (siren != null)
			{
				siren.Stop();
			}
		}

		HandleVehicleInFront();
	}

	private void HandleVehicleInFront()
	{
		if (Flipped)
		{
			isThereVehicle = Physics2D.OverlapArea(new Vector2(VehicleInFrontCheck.transform.position.x + 1f, VehicleInFrontCheck.position.y + .1f),
			new Vector2(VehicleInFrontCheck.position.x - 1f, VehicleInFrontCheck.position.y - 0.1f), VehicleMask);
		}
		else
		{
			isThereVehicle = Physics2D.OverlapArea(new Vector2(VehicleInFrontCheck.position.x - 1f, VehicleInFrontCheck.position.y - .1f),
			new Vector2(VehicleInFrontCheck.position.x + 1f, VehicleInFrontCheck.position.y + 0.1f), VehicleMask);

		}
		if (isThereVehicle)
		{
			if(startingSpeed < 0)
			{
				body.velocity = new Vector2(body.velocity.x + breakingForceStep, 0);
			}
			else
			{
				body.velocity = new Vector2(body.velocity.x - breakingForceStep, 0);
			}
			
		}
		else if(Mathf.Abs(startingSpeed) > Mathf.Abs(body.velocity.x))
		{
			if(startingSpeed < 0)
			{
				body.velocity = new Vector2(-(Mathf.Abs(body.velocity.x) + accelarationStep), 0);
			}
			else
			{
				body.velocity = new Vector2(body.velocity.x + accelarationStep, 0);
			}
		}
	}

}
