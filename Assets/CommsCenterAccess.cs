using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommsCenterAccess : MonoBehaviour {

	public GameObject player;
	private Text pickupText;
	public bool manualStart;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		pickupText = GameObject.Find ("ManualPickup").GetComponent<Text> ();
		pickupText.text = "";
	}

	void OnTriggerStay2D (Collider2D col)
	{
		pickupText.text = "Press 'E' to Open with Door";
		
	}

	void OnTriggerExit2D (Collider2D col)
	{
		pickupText.text = "";
	}

	public IEnumerator commsDoorOpen ()
	{
		if (Input.GetKeyDown (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().currentObjective == 0) {

		}
		else if (Input.GetKeyDown (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().currentObjective == 1) {

		}
		else if (Input.GetKeyDown (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().currentObjective == 2) {

		}
		else if (Input.GetKeyDown (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().currentObjective == 0) {

		}
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		player.GetComponent<PlayerController> ().hintBox.SetActive (true);
		player.GetComponent<PlayerController> ().hintText.text = "I've got the manual!";
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		player.GetComponent<PlayerController> ().hintText.text = "I need to find a Power Drill, Wrench, Hammer, Switchblade, Saw, Blow Torch, and Wire Cutters to repair the Communications Center!";
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		player.GetComponent<PlayerController> ().alarmIsStarted = true;
		player.GetComponent<PlayerController> ().hintText.text = "Oh no! Oxygen levels are dropping, I need to find a spacesuit!";
		yield return new WaitForSeconds (7.0f);
		player.GetComponent<PlayerController> ().hintBox.SetActive (false);
		GameObject.Find ("Timer").GetComponent<Timer> ().started = true;
		player.GetComponent<PlayerController> ().activeHint = false;
		player.GetComponent<PlayerController> ().canMove = true;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		//manual.SetActive (false);
		StopCoroutine ("alarmStartup");
	}
}
