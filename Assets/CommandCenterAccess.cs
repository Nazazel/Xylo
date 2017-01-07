using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class CommandCenterAccess : MonoBehaviour {

	public AudioSource door;
	public GameObject player;
	private Text pickupText;
	public bool atDoor;

	// Use this for initialization
	void Start () {
		door = gameObject.GetComponent<AudioSource> ();
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
			StartCoroutine ("comDoorOpen");
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

	public IEnumerator comDoorOpen ()
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
			StopCoroutine("comDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 1) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I need to get into the communications room to send out an SOS signal...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("comDoorOpen");
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
			StopCoroutine("comDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 3) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I should go into the communications room and send out an SOS signal before I do anything else...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("comDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			door.Play ();
			yield return new WaitUntil (() => !door.isPlaying);
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			SceneManager.LoadSceneAsync("Command Center");
			StopCoroutine("comDoorOpen");
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
			StopCoroutine("comDoorOpen");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 10) {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I should head back to the communications room...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			StopCoroutine("comDoorOpen");
		}
	}
}
