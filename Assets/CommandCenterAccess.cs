using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CommandCenterAccess : MonoBehaviour {

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
			player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
			StartCoroutine ("commsDoorOpen");
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		pickupText.text = "Press 'E' to Open Door";
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
			player.GetComponent<PlayerController> ().hintText.text = "Stella: (...I've got to find my crew members and see if they're alright...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 1) {
			player.GetComponent<PlayerController> ().currentObjective = 2;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			SceneManager.LoadSceneAsync("Comms Center");
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 2) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "Stella: (...I need to search the rooms for two omnicards in order to get into the communications room...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("commsDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			player.GetComponent<PlayerController> ().currentObjective = 11;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			SceneManager.LoadSceneAsync("Comms Center");
			StopCoroutine("commsDoorOpen");

		}

	}
}
