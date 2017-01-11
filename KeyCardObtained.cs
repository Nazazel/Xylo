using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyCardObtained : MonoBehaviour {

	public bool picked;
	private GameObject keyCard;
	public GameObject player;
	public bool requireButtonPress;
	private bool waitForPress;
	public Text pickupText;
	private bool startKeyDialogue;

	// Use this for initialization
	void Start () {
		picked = false;
		keyCard = this.gameObject;
		player = GameObject.FindWithTag ("Player");
		pickupText = GameObject.Find ("ManualPickup").GetComponent<Text> ();
		pickupText.text = "";
		startKeyDialogue = false;
	}

	void Update()
	{
		if (player.GetComponent<PlayerController> ().activeTab == false && player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().currentObjective == 2 && waitForPress && Input.GetKey (KeyCode.E) && !picked) {
			picked = true;
			StartCoroutine ("keyPick");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!picked && player.GetComponent<PlayerController> ().currentObjective == 2) {
			pickupText.text = "Press 'E' to Pick Up Keycard";
		} else {
			pickupText.text = "";
		}

		if (other.name == "Stella") {
			if (requireButtonPress) {
				waitForPress = true;
				return;
			}

		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		pickupText.text = "";
		if (col.name == "Stella")
		{
			waitForPress = false;
		}
	}

	public IEnumerator keyPick ()
	{
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().activeTab = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<PlayerController>().playerAnimator.Play("StellaStand");
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		if (player.GetComponent<PlayerController> ().numKeys == 0) {
			player.GetComponent<PlayerController> ().numKeys += 1;
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: I've got an omnicard! One more to go!";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			pickupText.text = "";
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().activeTab = false;
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
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Great! Now that I have both omnicards I can get into the communications room!";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			pickupText.text = "";
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().activeTab = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
			startKeyDialogue = false;
			keyCard.SetActive (false);
			StopCoroutine ("keyPick");
		}
			
	}
}
