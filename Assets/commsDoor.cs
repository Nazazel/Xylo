using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class commsDoor : MonoBehaviour {

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
				StartCoroutine ("commsConsole");

			}
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			if (waitForPress && Input.GetKey (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false) {
				StartCoroutine ("commsConsole");

			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (player.GetComponent<PlayerController> ().currentObjective == 4) {
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
}
