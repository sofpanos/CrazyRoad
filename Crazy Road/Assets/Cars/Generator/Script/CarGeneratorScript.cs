using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarGeneratorScript : MonoBehaviour {
	//Car speed constraints
	
	private readonly float[] Ambulance = { 1f, -1f };
	private readonly float[] Audi = { 2.5f, -.5f };
	private readonly float[] BlackViper = { 4f, .5f };
	private readonly float[] Car = { 1f, -2f };
	private readonly float[] MiniTruck = { 0.5f, -3f };
	private readonly float[] MiniVan = { .5f, -2.5f };
	private readonly float[] PolicCar = { 1f, 0f };
	private readonly float[] Taxi = { 1f, -1.5f };
	private readonly float[] Truck = { 0f, -3f };


	public const float MaxSpeed = 10f;
	public const float MinSpeed = 5f;


	public List<GameObject> CarList = new List<GameObject>();
	public static TimeSpan CarInterval = new TimeSpan(0, 0, 0, 2, 200);
	private DateTime LastGenerationTime;
	private bool start = true;
	
	
	// Update is called once per frame
	void Update () {
		if (!UIScript.GameOver && !UIScript.Won && !UIScript.Entry && !UIScript.Paused)
		{
			if (start)
			{
				GenerateCar();
				start = false;
			}
			else if (DateTime.Now - LastGenerationTime > CarInterval)
			{
				GenerateCar();
			}
		}
	}

	private void GenerateCar()
	{
		
		int carIndex = UnityEngine.Random.Range(0, CarList.Count);
		GameObject car = Instantiate(CarList[carIndex]);
		if((carIndex == 0 || carIndex == 1) && (new System.Random()).NextDouble() < 0.15f)
		{
			car.GetComponent<AudioSource>().Play();
			car.GetComponent<Animator>().SetBool("siren", true);
		}
		if (transform.position.x > 0)
		{
			car.transform.position = transform.position;
			car.GetComponent<VehicleScript>().Flipped = true;
			car.transform.Rotate(Vector3.forward, 180f);
			GiveVelocity(-1, car);
			LastGenerationTime = DateTime.Now;
		}
		else
		{
			car.transform.position = transform.position;
			GiveVelocity(1, car);
			LastGenerationTime = DateTime.Now;
		}
	}

	private void GiveVelocity(int direction, GameObject car)
	{
		Rigidbody2D body = car.GetComponent<Rigidbody2D>();
		float carSpeed = 0;
		switch (car.tag)
		{
			case "Ambulance":
				carSpeed = UnityEngine.Random.Range(Ambulance[0] + MinSpeed, Ambulance[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
			case "Audi":
				carSpeed = UnityEngine.Random.Range(Audi[0] + MinSpeed, Audi[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
			case "BlackViper":
				carSpeed = UnityEngine.Random.Range(BlackViper[0] + MinSpeed, BlackViper[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
			case "Car":
				carSpeed = UnityEngine.Random.Range(Car[0] + MinSpeed, Car[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
			case "MiniTruck":
				carSpeed = UnityEngine.Random.Range(MiniTruck[0] + MinSpeed, MiniTruck[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
			case "MiniVan":
				carSpeed = UnityEngine.Random.Range(MiniVan[0] + MinSpeed, MiniTruck[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
			case "PoliceCar":
				carSpeed = UnityEngine.Random.Range(PolicCar[0] + MinSpeed, PolicCar[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
			case "Truck":
				carSpeed = UnityEngine.Random.Range(Truck[0] + MinSpeed, Truck[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
			case "Taxi":
				carSpeed = UnityEngine.Random.Range(Taxi[0] + MinSpeed, Taxi[1] + MaxSpeed);
				body.velocity = direction * new Vector2(carSpeed, 0);
				break;
		}
	}
}
