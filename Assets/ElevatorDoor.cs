using UnityEngine;
using System.Collections;

public class ElevatorDoor : MonoBehaviour {

	GameObject door;

	void Start ()
	{
		door = GameObject.FindGameObjectWithTag ("ElevatorDoor");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		door.transform.position = new Vector2(door.transform.position.x, 1.84f);
	}

	void OnTriggerExit2D (Collider2D col)
	{
		door.transform.position = new Vector2(door.transform.position.x, 4.84f);
	}
}
