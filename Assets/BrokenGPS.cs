using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BrokenGPS : MonoBehaviour {

	private GameObject GPStracker;
	public GameObject player;
	private Text pickupText;
	public bool requireButtonPress;
	private bool waitForPress;
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

	void Update()
	{
		if (player.GetComponent<PlayerController> ().activeHint == false && player.GetComponent<PlayerController> ().finishedJump == true && startGPSDialogue == false && Input.GetKey (KeyCode.E)) {
			player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
			startGPSDialogue = true;
			StartCoroutine ("GPSPick");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		pickupText.text = "Press 'E' to use GPS Tracker";


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

	public IEnumerator GPSPick ()
	{
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

		if (player.GetComponent<PlayerController> ().currentObjective == 4 && GPSinteracted == false) {
			player.GetComponent<Animator> ().Play ("StellaFaceCover");
			yield return new WaitForSeconds (3.0f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: No...this can't be happening! The damn thing is broken...what am I going to do? I can't fix the ship...";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: I don't even know how ships work.\nI mean...the only option I have is to try to repair the communication terminal.But...what tools do I even need?";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Wasn't there some sort of repair manual that every ship is supposed to have?\nThink, Stella, think!";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<Animator> ().Play ("StellaStand");
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Yeah, yeah...I remember hearing that in a meeting when I was first commissioned. The manual is supposed to be located in the engineering wing of every ship owned by NASA.";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: I think I've even seen it in our engineering wing now that I think about it! Gotta go there and find that manual.";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().commandCenter = true;
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
			startGPSDialogue = false;
			GPSinteracted = true;
			StopCoroutine ("GPSPick");
		} 
		else {
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I need the repair manual located in the engineering wing in order to find the tools I need...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
			startGPSDialogue = false;
			StopCoroutine ("GPSPick");
		}

	}
}
