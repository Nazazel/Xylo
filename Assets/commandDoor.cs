using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class commandDoor : MonoBehaviour {

	public bool requireButtonPress;
	private bool waitForPress;

	public bool destroyWhenActivated;

	public Text pickupText;
	public GameObject player;

	// Use this for initialization
	void Start () {
		pickupText = GameObject.Find("ManualPickup").GetComponent<Text>();
		pickupText.text = "";
		player = GameObject.FindWithTag("Player");
	}

	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PlayerController> ().currentObjective == 3) {
			if (waitForPress && Input.GetKey (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false){
				if (player.GetComponent<PlayerController> ().hasSuit == false) {
					player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
				} 
				else {
					player.GetComponent<PlayerController> ().playerAnimator.Play ("SpaceStand");
				}
				StartCoroutine ("commandText");

			}
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			if (waitForPress && Input.GetKey (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false) {
				if (player.GetComponent<PlayerController> ().hasSuit == false) {
					player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
				} 
				else {
					player.GetComponent<PlayerController> ().playerAnimator.Play ("SpaceStand");
				}
				StartCoroutine ("commandText");

			}
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 5) {
			if (waitForPress && Input.GetKey (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false) {
				if (player.GetComponent<PlayerController> ().hasSuit == false) {
					player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
				} 
				else {
					player.GetComponent<PlayerController> ().playerAnimator.Play ("SpaceStand");
				}
				StartCoroutine ("commandText");

			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (player.GetComponent<PlayerController> ().currentObjective == 3) {
			pickupText.text = "Press 'E' to use";

			if (other.name == "Stella") {
				if (requireButtonPress) {
					waitForPress = true;
					return;
				}

				if (destroyWhenActivated) {
					Destroy (gameObject);
				}
			}
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			pickupText.text = "Press 'E' to use";

			if (other.name == "Stella") {
				if (requireButtonPress) {
					waitForPress = true;
					return;
				}

				if (destroyWhenActivated) {
					Destroy (gameObject);
				}
			}
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 5) {
			pickupText.text = "Press 'E' to use";

			if (other.name == "Stella") {
				if (requireButtonPress) {
					waitForPress = true;
					return;
				}

				if (destroyWhenActivated) {
					Destroy (gameObject);
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		pickupText.text = "";
		if (other.name == "Stella")
		{
			waitForPress = false;
		}
	}

	public IEnumerator commandText()
	{
		if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			player.GetComponent<PlayerController> ().activeHint = true;
			player.GetComponent<PlayerController> ().canMove = false;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "Stella: (...I should try to send my location out using the GPS Tracker in this room...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 5) {
			SceneManager.LoadSceneAsync("Level One Clean");
			StopCoroutine("commandText");
		}
	}
}
