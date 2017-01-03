using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CommsCenterAccess : MonoBehaviour {

	public GameObject player;
	private Text pickupText;
	public bool atDoor;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		atDoor = false;
		pickupText = GameObject.Find ("ManualPickup").GetComponent<Text> ();
		pickupText.text = "";
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().finishedJump == true && atDoor == true) {
			if (player.GetComponent<PlayerController> ().hasSuit == false) {
				player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
			} 
			else {
				player.GetComponent<PlayerController> ().playerAnimator.Play ("SpaceStand");
			}
			StartCoroutine ("commsDoorOpen");
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		pickupText.text = "Press 'E' to Enter";
		atDoor = true;
	}

	void OnTriggerExit2D (Collider2D col)
	{
		pickupText.text = "";
		atDoor = false;
	}

	public IEnumerator commsDoorOpen ()
	{
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

		if (player.GetComponent<PlayerController> ().currentObjective == 0) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I need to find my crew members and check if they're alright...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 1) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Damn it! It's locked!\n\tRight, now I remember...the door won't open under emergency lockdown unless I have two omnicards...\n\tI need to search the rooms.";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().commsCenterInit = true;
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 2) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I need to search the rooms for two omnicards in order to get into the communications room...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 3) {
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			SceneManager.LoadSceneAsync("Comms Center");
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I should try to send my location out using the GPS Tracker in the command center...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 5) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I need the repair manual located in the engineering wing in order to find the tools I need...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 10) {
			player.GetComponent<PlayerController> ().currentObjective = 11;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			SceneManager.LoadSceneAsync("Comms Center");
			StopCoroutine("commsDoorOpen");

		}

	}
}
