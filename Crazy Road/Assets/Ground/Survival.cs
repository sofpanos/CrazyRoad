using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MonoBehaviour {
	public GameObject Player;
	public GameObject Road;
	public GameObject StartSideWalk;
	public GameObject StartFiller;
	public LinkedList<GameObject> LiveObjects = new LinkedList<GameObject>();
	
	private Stack<Type> PreviousObjectsStack = new Stack<Type>();

	private const float RoadWidth = 2.56f;

	private enum Type
	{
		Road,
		Start,
		Filler
	}
	// Use this for initialization
	void Start () {
		foreach(Transform child in transform)
		{
			LiveObjects.AddFirst(child.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		GameObject First = LiveObjects.First.Value;
		GameObject Last = LiveObjects.Last.Value;
		if(Mathf.Abs(First.transform.position.y - Player.transform.position.y) < 2 * RoadWidth)
		{
			AddRoad();
		}
		if(Mathf.Abs(Player.transform.position.y - Last.transform.position.y) < 2 * RoadWidth)
		{
			AddPrevious();
		}
		else if(Mathf.Abs(Player.transform.position.y - Last.transform.position.y) > 3 * RoadWidth)
		{
			RemoveLast();
		}

	}

	private void AddRoad()
	{
		GameObject NewRoad = Instantiate(Road);
		Transform First = LiveObjects.First.Value.transform;
		NewRoad.transform.position = new Vector3(0, RoadWidth, 0) + First.position;
		NewRoad.transform.SetParent(transform);
		LiveObjects.AddFirst(NewRoad);
	}

	private void AddPrevious()
	{
		GameObject NewObject;
		Transform Last = LiveObjects.Last.Value.transform;
		if (PreviousObjectsStack.Count == 0)
		{
			return;
		}
		switch (PreviousObjectsStack.Pop())
		{
			case Type.Filler:
				NewObject = Instantiate(StartFiller);
				NewObject.tag = "Filler";
				NewObject.transform.position = Last.transform.position;
				break;
			case Type.Start:
				NewObject = Instantiate(StartSideWalk);
				NewObject.transform.position = Last.transform.position;
				NewObject.tag = "Start";
				break;
			case Type.Road:
				NewObject = Instantiate(Road);
				NewObject.tag = "Road";
				NewObject.transform.position = Last.position - new Vector3(0, RoadWidth, 0);
				break;
			default:
				NewObject = null;
				break;
		}
		if(NewObject == null)
		{
			return;
		}
		NewObject.transform.SetParent(transform);
		LiveObjects.AddLast(NewObject);
	}

	private void RemoveLast()
	{
		GameObject Last = LiveObjects.Last.Value;
		switch (Last.tag)
		{
			case "Filler":
				PreviousObjectsStack.Push(Type.Filler);
				break;
			case "Start":
				PreviousObjectsStack.Push(Type.Start);
				break;
			case "Road":
				PreviousObjectsStack.Push(Type.Road);
				break;
			default:
				break;
		}
		Destroy(Last);
		LiveObjects.RemoveLast();
	}
}
