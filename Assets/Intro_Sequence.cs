using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro_Sequence : MonoBehaviour {
	public Text IntroText;
	public Image FadeImg;
	public Image controlsImage;
	public float fadeSpeed = 3.0f;

	// Use this for initialization
	void Start () {
		controlsImage.color = new Color (0, 0, 0, 0);
		IntroText = this.gameObject.GetComponent<Text> ();
		IntroText.text = "The year is 2134. Man has discovered hyperspace travel. \nDue to extreme overpopulation of our own solar system, NASA has deployed several teams to search for other deep space regions to claim as our own. \nUpon investigation of nearby planetary systems, Stella Kern and her team of space explorers discover the human habitable planetary system, Xylo, not too far from their location. Thus, they embark on their journey. \nHowever, during their travel to Xylo, something goes horribly wrong. \nTheir spacecraft, the Ancora, crash lands on an unfamiliar planet. \nStella wakes up alone and confused. \nThere is no time to panic, she must find her crewmates and figure out a way to survive. ";
		InvokeRepeating ("FadeToClear", 0f, 0.1f);
		StartCoroutine("tran_interact");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public  IEnumerator tran_interact(){
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		IntroText.text = "";
		controlsImage.color = new Color (1, 1, 1, 1);
		yield return new WaitUntil (() => Input.GetKeyDown (KeyCode.Return));
		yield return new WaitForSeconds (0.2f);
		if (IsInvoking ()) {
			CancelInvoke ("FadeToClear");
		}
		InvokeRepeating ("FadeToBlack", 0f, 0.1f);
		yield return new WaitForSeconds (4.5f);
		SceneManager.LoadSceneAsync ("Level One");
	}

	void FadeToClear()
	{
		FadeImg.color = Color.Lerp (FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a < 0.05f) {
			CancelInvoke ("FadeToClear");
			FadeImg.color = Color.clear;
		}
	}

	void FadeToBlack()
	{
		FadeImg.color = Color.Lerp (FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a == 1.0f) {
			CancelInvoke ("FadeToBlack");
		}
	}
}
