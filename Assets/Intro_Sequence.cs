using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Intro_Sequence : MonoBehaviour {
	public Text IntroText;

	// Use this for initialization
	void Start () {
		IntroText = this.gameObject.GetComponent<Text> ();
		IntroText.text = "The year is 2134; man has discovered hyper space travel. \nDue to extreme overpopulation of our own solar system, NASA has deployed several teams to search for other deep space regions to claim as our own. \nUpon investigation of nearby planetary systems, Stella Kern and her team of space explorers discover the human habitable planetary system, Xylo, not too far from their location. Thus, they embark on their journey. \nHowever, during their travel to Xylo, something goes horribly wrong. \nTheir spacecraft, the Ancora, crash lands on an unfamiliar planet. \nStella wakes up alone and confused. \nThere is no time to panic, she must find her crewmates and figure out a way to survive. ";
		StartCoroutine("tran_interact");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public  IEnumerator tran_interact(){
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		IntroText.text = "How to Play:\n\nUse the left and right arrow keys or the \"A\" and \"D\" keys to move Stella through the levels.\nUse the down arrow key or the \"S\" key to crouch.\nUse Spacebar to jump.\nPress the \"E\" key to interact with ladders, notes and key items.\nPress the Enter key to proceed through dialog.\nPress the \"H\" key for hints on objectives throughout the game.";
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
	}
}
