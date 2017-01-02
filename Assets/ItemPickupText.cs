using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemPickupText : MonoBehaviour {

	public bool requireButtonPress;
	private bool waitForPress;
	public Text pickupText;
	public bool destroyWhenActivated;

	// Use this for initialization
	void Start () {
		pickupText = GameObject.Find("ManualPickup").GetComponent<Text> ();
	}

	void Update()
	{
		if (waitForPress && Input.GetKey (KeyCode.E)) {
			pickupText.text = "";
			DestroyImmediate (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		pickupText.text = "Press 'E' to Pick Up " + gameObject.name;

		if (other.name == "Stella") {
			if (requireButtonPress) {
				waitForPress = true;
				return;
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
}
