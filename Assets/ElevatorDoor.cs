using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ElevatorDoor : MonoBehaviour {

	public GameObject door;

	void Start ()
	{
		door = GameObject.FindGameObjectWithTag ("ElevatorDoor");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (SceneManager.GetActiveScene ().name == "Level One") {
			door.GetComponent<SpriteRenderer> ().enabled = false;
			door.GetComponent<BoxCollider2D> ().enabled = false;
		}

	}

	void OnTriggerExit2D (Collider2D col)
	{
		door.GetComponent<SpriteRenderer> ().enabled = true;
		door.GetComponent<BoxCollider2D> ().enabled = true;
	}
}
