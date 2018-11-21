using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleScript : MonoBehaviour {

	public Transform VehicleInFrontCheck;
	public LayerMask VehicleMask;
	public float VehicleCheckRadius = 0.2f;
	public float breakingForceStep = 0.2f;
	
	private float startingSpeed = 0;
	
	private float breakingForce = 0;

	private float accelarationStep = 0.3f;
	private bool isThereVehicle = false;
	private Rigidbody2D body;

	private void Start()
	{
		body = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			float dummy = 2f;
			//GameObject canvas = GameObject.Find("Canvas");
			//canvas.GetComponent<UIScript>().GameOver();
		}
	}

	private void Update()
	{
		if (GetComponent<SpriteRenderer>().flipY)
		{
			if(transform.position.x < -14)
			{
				Destroy(transform.gameObject);
			}
		}
		else
		{
			if(transform.position.x > 14)
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
		HandleVehicleInFront();
	}

	private void HandleVehicleInFront()
	{
		isThereVehicle = Physics2D.OverlapCircle(VehicleInFrontCheck.position, VehicleCheckRadius, VehicleMask);
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
			breakingForce = 0;
			if(startingSpeed < 0)
			{
				body.velocity = new Vector2(-(Mathf.Abs(body.velocity.x) + accelarationStep), 0);
			}
			else
			{
				body.velocity = new Vector2(body.velocity.x + accelarationStep, 0);
			}
		}
		else
		{
			breakingForce = 0;
		}
	}
}
