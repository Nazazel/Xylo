using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeadCrewSight : MonoBehaviour {

	private GameObject triggerBox;
	public GameObject player;
	public bool manualStart;

	// Use this for initialization
	void Start () {
		triggerBox = this.gameObject;
		player = GameObject.FindWithTag ("Player");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		StartCoroutine ("deathReaction");
	}
		

	public IEnumerator deathReaction ()
	{
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		player.GetComponent<PlayerController> ().hintBox.SetActive (true);
		if (triggerBox.name == "Medic") {
			player.GetComponent<PlayerController> ().hintText.text = "";
			player.GetComponent<PlayerController> ().numDead += 1;
		}
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		if (player.GetComponent<PlayerController> ().numDead == 5) {
			player.GetComponent<PlayerController> ().deadCrew = true;
		}
		player.GetComponent<PlayerController> ().hintBox.SetActive (false);
		player.GetComponent<PlayerController> ().activeHint = false;
		player.GetComponent<PlayerController> ().canMove = true;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		triggerBox.GetComponent<BoxCollider2D>().enabled = false;
		StopCoroutine ("deathReaction");
	}
}
