using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ElevatorDoor : MonoBehaviour {

	public GameObject door;
	private GameObject player;

	void Start ()
	{
		player = GameObject.FindWithTag ("Player");
		door = GameObject.FindGameObjectWithTag ("ElevatorDoor");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (player.GetComponent<PlayerController> ().currentObjective == 5) {
			door.GetComponent<SpriteRenderer> ().enabled = false;
			door.GetComponent<BoxCollider2D> ().enabled = false;
		}
		else if(player.GetComponent<PlayerController> ().currentObjective == 8) {
			door.GetComponent<SpriteRenderer> ().enabled = false;
			door.GetComponent<BoxCollider2D> ().enabled = false;
		}
		else if(player.GetComponent<PlayerController> ().currentObjective == 9) {
			door.GetComponent<SpriteRenderer> ().enabled = false;
			door.GetComponent<BoxCollider2D> ().enabled = false;
		}
		else if(player.GetComponent<PlayerController> ().currentObjective == 10) {
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
