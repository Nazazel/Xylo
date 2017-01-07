using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemPickupText : MonoBehaviour {

	public bool picked;
	public bool requireButtonPress;
	private bool waitForPress;
	public Text pickupText;
	public bool destroyWhenActivated;
	public AudioSource itemsound;

	// Use this for initialization
	void Start () {
		picked = false;
		itemsound = gameObject.GetComponent<AudioSource> ();
		pickupText = GameObject.Find("ManualPickup").GetComponent<Text> ();
	}

	void Update()
	{
		if (waitForPress && Input.GetKey (KeyCode.E) && !picked) {
			picked = true;
			StartCoroutine ("soundDestroy");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!picked) {
			pickupText.text = "Press 'E' to Pick Up " + gameObject.name;
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

	void OnTriggerExit2D(Collider2D other)
	{
		pickupText.text = "";
		if (other.name == "Stella")
		{
			waitForPress = false;
		}
	}

	public IEnumerator soundDestroy()
	{
		pickupText.text = "";
		itemsound.Play ();
		yield return new WaitUntil (() => !itemsound.isPlaying);
		DestroyImmediate (gameObject);
	}
}
