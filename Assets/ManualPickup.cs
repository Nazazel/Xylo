using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManualPickup : MonoBehaviour {

	private GameObject manual;
	public GameObject player;
	private Text pickupText;
	public bool manualStart;

	// Use this for initialization
	void Start () {
		manual = this.gameObject;
		player = GameObject.FindWithTag ("Player");
		manualStart = false;
		pickupText = GameObject.Find ("ManualPickup").GetComponent<Text> ();
		pickupText.text = "";
	}

	void OnTriggerStay2D (Collider2D col)
	{
		if (manualStart == false) {
			pickupText.text = "Press 'E' to Pick Up Manual";
		} 
		else {
			pickupText.text = "";
		}

		if (Input.GetKeyDown (KeyCode.E) && manualStart == false && player.GetComponent<PlayerController> ().activeHint == false) {
			player.GetComponent<PlayerController> ().manual = true;
			if (manualStart == false) {
				StartCoroutine("alarmStartup");
				manualStart = true;
			}
		}
	}

	void OnTriggerExit2D (Collider2D col)
	{
		pickupText.text = "";
	}

	public IEnumerator alarmStartup ()
	{
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaStand");
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		player.GetComponent<PlayerController> ().hintBox.SetActive (true);
		player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Yes! This is it! This is the manual!\nMust've been displaced in the crash. It's a little...bloody but everything inside is legible...";
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Okay, so according to the book, I need eight tools to repair the communication terminal.\nI need to find a power drill, wrench, switchblade, hammer, saw, wire cutter, a screwdriver and a blowtorch.";
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: ...That's a lot of stuff. Guess I'd better start searching.";
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		player.GetComponent<PlayerController> ().alarmIsStarted = true;
		player.GetComponent<Animator> ().Play ("StellaDiscomfort");
		player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Wait, what!? What the hell is going on? Oh God, it's the oxygen levels...they're failing! \nNeed to find a spacesuit before the 02 levels completely drop!";
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		player.GetComponent<PlayerController> ().hintBox.SetActive (false);
		GameObject.Find ("Timer").GetComponent<Timer> ().started = true;
		player.GetComponent<PlayerController> ().activeHint = false;
		player.GetComponent<PlayerController> ().canMove = true;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		manual.SetActive (false);
		StopCoroutine ("alarmStartup");
	}
}
