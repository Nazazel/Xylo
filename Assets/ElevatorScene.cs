using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ElevatorScene : MonoBehaviour {

	public AudioSource ding;
	public AudioSource ambience;
	public Image FadeImg;
	public float fadeSpeed = 1.5f;
	public GameObject player;
	public bool transitionStarted = false;

	// Use this for initialization
	void Start () {
		ambience = GameObject.Find ("Ground").GetComponent<AudioSource> ();
		ding = gameObject.GetComponent<AudioSource> ();
		player = GameObject.FindWithTag ("Player");
		transitionStarted = false;
	}

	void Update()
	{
		if (!transitionStarted && FadeImg.color.a == 0.0f) {
			StartCoroutine ("transitionToLevel");
			transitionStarted = true;
		}
	}

	void FadeToBlack()
	{
		FadeImg.color = Color.Lerp (FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a == 1.0f) {
			CancelInvoke ("FadeToBlack");
			player.GetComponent<PlayerController> ().introDone = true;
		}
	}

	public IEnumerator transitionToLevel()
	{
		yield return new WaitForSeconds (4.0f);
		if (player.GetComponent<PlayerController> ().currentObjective == 5) {
			player.GetComponent<PlayerController> ().introDone = false;
			InvokeRepeating ("FadeToBlack", 1.0f, 0.1f);
			yield return new WaitForSeconds (8.0f);
			ambience.Stop ();
			yield return new WaitForSeconds (1.0f);
			ding.Play ();
			yield return new WaitUntil (() => !ding.isPlaying);
			SceneManager.LoadSceneAsync("Engineering Wing");
		} 
		else if (player.GetComponent<PlayerController> ().currentObjective == 8) {
			player.GetComponent<PlayerController> ().introDone = false;
			InvokeRepeating ("FadeToBlack", 1.0f, 0.1f);
			yield return new WaitForSeconds (8.0f);
			ambience.Stop ();
			yield return new WaitForSeconds (1.0f);
			ding.Play ();
			yield return new WaitUntil (() => !ding.isPlaying);
			SceneManager.LoadSceneAsync("Medical Ward");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 9) {
			player.GetComponent<PlayerController> ().introDone = false;
			InvokeRepeating ("FadeToBlack", 1.0f, 0.1f);
			yield return new WaitForSeconds (8.0f);
			ambience.Stop ();
			yield return new WaitForSeconds (1.0f);
			ding.Play ();
			yield return new WaitUntil (() => !ding.isPlaying);
			SceneManager.LoadSceneAsync("Engine Room");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 10) {
			player.GetComponent<PlayerController> ().introDone = false;
			InvokeRepeating ("FadeToBlack", 1.0f, 0.1f);
			yield return new WaitForSeconds (8.0f);
			ambience.Stop ();
			yield return new WaitForSeconds (1.0f);
			ding.Play ();
			yield return new WaitUntil (() => !ding.isPlaying);
			SceneManager.LoadSceneAsync("Level One Clean");
		}
	}


}
