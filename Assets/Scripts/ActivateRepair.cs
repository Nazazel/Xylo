using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivateRepair : MonoBehaviour
{
    public bool requireButtonPress;
    private bool waitForPress;

    public bool destroyWhenActivated;
	public Animator terminalAnimator;

    public Text pickupText;
    public GameObject player;
    public GameObject repairBar;
    public bool isDown;
	public bool finish;


    // Use this for initialization
    void Start()
    {
		terminalAnimator = gameObject.GetComponent<Animator> ();
        pickupText = GameObject.Find("ManualPickup").GetComponent<Text>();
        pickupText.text = "";
        player = GameObject.FindWithTag("Player");
		repairBar = GameObject.FindWithTag ("RepairBar");

    }

    // Update is called once per frame
    void Update()
    {
		if (player.GetComponent<PlayerController> ().currentObjective == 3) {
			if (waitForPress && Input.GetKey (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false) {
				StartCoroutine ("commsConsole");

			}
		} else if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			if (waitForPress && Input.GetKey (KeyCode.E) && player.GetComponent<PlayerController> ().activeHint == false) {
				StartCoroutine ("commsConsole");

			}
		} else if (player.GetComponent<PlayerController> ().currentObjective == 11 && repairBar.GetComponent<Timer2> ().isDone == false) {
			if (waitForPress && Input.GetKey (KeyCode.R)) {
				repairBar.GetComponent<Timer2> ().started = true;
				isDown = true;
				player.GetComponent<Animator> ().Play ("SpaceRepair");
				player.GetComponent<PlayerController> ().canMove = false;
				player.GetComponent<PlayerController> ().activeHint = true;
				player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
				player.GetComponent<PlayerController> ().glow.sortingOrder = 48;


				if (destroyWhenActivated) {
					Destroy (gameObject);
				}

          
			} else {
				repairBar.GetComponent<Timer2> ().started = false;
				isDown = false;
				player.GetComponent<PlayerController> ().canMove = true;
				player.GetComponent<PlayerController> ().activeHint = false;
				player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
				player.GetComponent<PlayerController> ().glow.sortingOrder = 60;
			}
		} 
		else if (repairBar.GetComponent<Timer2> ().isDone == true && finish == false) {
			pickupText.text = "";
			terminalAnimator.Play ("Fixed");
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
		else if (player.GetComponent<PlayerController> ().currentObjective == 11) {
			pickupText.text = "Hold 'R' to repair";

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
		else if (repairBar.GetComponent<Timer2> ().isDone == true) {
			pickupText.text = "";

			Debug.Log("Done!");
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

	public IEnumerator commsConsole()
	{
		if (player.GetComponent<PlayerController> ().currentObjective == 3) {
			player.GetComponent<PlayerController> ().accessCommsCenter = true;
			player.GetComponent<PlayerController> ().activeHint = true;
			player.GetComponent<PlayerController> ().canMove = false;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
			player.GetComponent<Animator> ().Play ("StellaFaceCover");
			yield return new WaitForSeconds (3.0f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: No, no, no! The communication terminal is busted...";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<Animator> ().Play ("StellaStand");
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: Maybe...maybe I can use the GPS tracker in the command center to at least send out my location.\n\tI need to go into the command center and find that GPS tracker. I pray it didn't get damaged in the crash.";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 4) {
			player.GetComponent<PlayerController> ().activeHint = true;
			player.GetComponent<PlayerController> ().canMove = false;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
			player.GetComponent<PlayerController> ().hintBox.SetActive (true);
			player.GetComponent<PlayerController> ().hintText.text = "<color=fuchsia>Stella</color>: (...I should try to send my location out using the GPS Tracker in the command center...)";
			yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
			yield return new WaitForSeconds (0.2f);
			player.GetComponent<PlayerController> ().hintBox.SetActive (false);
			player.GetComponent<PlayerController> ().activeHint = false;
			player.GetComponent<PlayerController> ().canMove = true;
			player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
		}

	}
}
