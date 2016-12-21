using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ElevatorScene : MonoBehaviour {

	public Image FadeImg;
	public float fadeSpeed = 1.5f;
	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
		InvokeRepeating ("FadeToClear", 1.0f, 0.1f);
		StartCoroutine ("transitionToLevel");
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

	public IEnumerator transitionToLevel()
	{
		yield return new WaitForSeconds (10.0f);
		if (player.GetComponent<PlayerController> ().currentObjective == 5) {
			InvokeRepeating ("FadeToBlack", 1.0f, 0.1f);
			yield return new WaitForSeconds (3.0f);
			SceneManager.LoadSceneAsync("Engineering Wing");
		} 
		else if (player.GetComponent<PlayerController> ().currentObjective == 8) {
			InvokeRepeating ("FadeToBlack", 1.0f, 0.1f);
			yield return new WaitForSeconds (3.0f);
			SceneManager.LoadSceneAsync("Medical Ward");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 9) {
			InvokeRepeating ("FadeToBlack", 1.0f, 0.1f);
			yield return new WaitForSeconds (3.0f);
			SceneManager.LoadSceneAsync("Engine Room");
		}
		else if (player.GetComponent<PlayerController> ().currentObjective == 10) {
			InvokeRepeating ("FadeToBlack", 1.0f, 0.1f);
			yield return new WaitForSeconds (3.0f);
			SceneManager.LoadSceneAsync("Level One Clean");
		}
	}

}
