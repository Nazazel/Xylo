using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeadCrewSight : MonoBehaviour {

	private GameObject triggerBox;
	public GameObject player;
	public bool manualStart;

	// Use this for initialization
	void Start () {
		triggerBox = this.gameObject;
		player = GameObject.FindWithTag ("Player");
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		StartCoroutine ("deathReaction");
	}
		

	public IEnumerator deathReaction ()
	{
		player.GetComponent<PlayerController> ().activeHint = true;
		player.GetComponent<PlayerController> ().canMove = false;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
		player.GetComponent<PlayerController> ().hintBox.SetActive (true);
		if (triggerBox.name == "Medic") {
			player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaFaceCover");
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: ...she's dead.";
			player.GetComponent<PlayerController> ().numDead += 1;
		} else if (triggerBox.name == "Engineer") {
			player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaFaceCover");
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Dear Lord...he's...dead.";
			player.GetComponent<PlayerController> ().numDead += 1;
		} else if (triggerBox.name == "Red Shirt") {
			player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaFaceCover");
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: H-he's dead.";
			player.GetComponent<PlayerController> ().numDead += 1;
		} else if (triggerBox.name == "Navigation") {
			player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaFaceCover");
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Jesus Christ, he's...";
			player.GetComponent<PlayerController> ().numDead += 1;
		} else if (triggerBox.name == "Captain") {
			player.GetComponent<PlayerController> ().playerAnimator.Play ("StellaFaceCover");
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: My God, no..t-the captain's dead.";
			player.GetComponent<PlayerController> ().numDead += 1;
		}
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		if (player.GetComponent<PlayerController> ().numDead == 5) {
			player.GetComponent<PlayerController> ().deadCrew = true;
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: What do I do? What do I do?! I'm alone!I need help...I need help!\n\tI...I need to send out a signal.\n\tI have to go to the communications room and send out an SOS signal! ";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
		}
		player.GetComponent<PlayerController> ().hintBox.SetActive (false);
		player.GetComponent<PlayerController> ().activeHint = false;
		player.GetComponent<PlayerController> ().canMove = true;
		player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
		triggerBox.GetComponent<BoxCollider2D>().enabled = false;
		StopCoroutine ("deathReaction");
	}
}
