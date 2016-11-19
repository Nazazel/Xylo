using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BrokenGPS : MonoBehaviour {

	private GameObject GPStracker;
	public GameObject player;
	private Text pickupText;
	private bool startGPSDialogue;
	private bool GPSinteracted;

	// Use this for initialization
	void Start () {
		GPStracker = this.gameObject;
		player = GameObject.FindWithTag ("Player");
		pickupText = GameObject.Find ("ManualPickup").GetComponent<Text> ();
		pickupText.text = "";
		startGPSDialogue = false;
		GPSinteracted = false;
	}

	void OnTriggerStay2D (Collider2D col)
	{
		pickupText.text = "Press 'E' to use GPS Tracker";

		if (Input.GetKeyDown (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().finishedJump == true && startGPSDialogue == false) {
				player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
				startGPSDialogue = true;
				StartCoroutine ("GPSPick");
			}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		pickupText.text = "";
	}

	public IEnumerator GPSPick ()
	{
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

		if (player.GetComponent<PlayerController> ().currentObjective == 4 && GPSinteracted == false) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "Stella: I've got an omnicard! One more to go!";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().commandCenter = true;
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
			GPSinteracted = true;
			StopCoroutine ("GPSPick");
		} 
		else if (GPSinteracted == true) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "Stella: (...I need the repair manual located in the engineering wing in order to find the tools I need...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine ("GPSPick");
		}

	}
}
