using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyCardObtained : MonoBehaviour {

	private GameObject keyCard;
	public GameObject player;
	private Text pickupText;
	private bool startKeyDialogue;

	// Use this for initialization
	void Start () {
		keyCard = this.gameObject;
		player = GameObject.FindWithTag ("Player");
		pickupText = GameObject.Find ("ManualPickup").GetComponent<Text> ();
		pickupText.text = "";
		startKeyDialogue = false;
	}

	void OnTriggerStay2D (Collider2D col)
	{
		if (player.GetComponent<PlayerController> ().currentObjective == 2 && startKeyDialogue == false) {
			pickupText.text = "Press 'E' to Pick Up Keycard";

			if (Input.GetKeyDown (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().finishedJump == true) {
				player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
				startKeyDialogue = true;
				StartCoroutine ("keyPick");
			}
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		pickupText.text = "";
	}

	public IEnumerator keyPick ()
	{
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		if (player.GetComponent<PlayerController> ().numKeys == 0) {
			player.GetComponent<PlayerController> ().numKeys += 1;
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "Stella: I've got an omnicard! One more to go!";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			pickupText.text = "";
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			startKeyDialogue = false;
			keyCard.SetActive (false);
			StopCoroutine ("keyPick");
		}
		else if (player.GetComponent<PlayerController> ().numKeys == 1) {
			player.GetComponent<PlayerController> ().numKeys += 1;
			player.GetComponent<PlayerController> ().keyCards = true;
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "Stella: Great! Now that I have both omnicards I can get into the communications room!";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			pickupText.text = "";
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			startKeyDialogue = false;
			keyCard.SetActive (false);
			StopCoroutine ("keyPick");
		}
			
	}
}
