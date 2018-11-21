using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarGeneratorScript : MonoBehaviour {
	public List<GameObject> CarList = new List<GameObject>();
	private TimeSpan CarInterval = new TimeSpan(0, 0, 2);
	private DateTime LastGenerationTime;
	public static float MaxSpeed = 9;
	public static float MinSpeed = 5;
	// Use this for initialization
	void Start () {
		GenerateCar();
	}
	
	// Update is called once per frame
	void Update () {
		if(DateTime.Now - LastGenerationTime > CarInterval)
		{
			GenerateCar();
		}
	}

	private void GenerateCar()
	{
		int carIndex = UnityEngine.Random.Range(0, CarList.Count);
		GameObject car = Instantiate(CarList[carIndex]);
		if(carIndex == 0 || carIndex == 1)
		{
			if(UnityEngine.Random.value < 0.5f)
			{
				car.GetComponent<Animator>().SetBool("siren", true);
			}
		}
		float StartSpeed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
		if (transform.position.x > 0)
		{
			car.transform.position = transform.position;
			car.GetComponent<SpriteRenderer>().flipY = true;
			car.GetComponent<VehicleScript>().VehicleInFrontCheck.localPosition = new Vector3(0, -6);
			car.GetComponent<Rigidbody2D>().velocity = new Vector2(-StartSpeed, 0);
			LastGenerationTime = DateTime.Now;
		}
		else
		{
			car.transform.position = transform.position;
			car.GetComponent<Rigidbody2D>().velocity = new Vector2(StartSpeed, 0);
			LastGenerationTime = DateTime.Now;
		}
	}
}
