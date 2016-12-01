using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour {

	public GameObject player;
	public Image FadeImg;
	public float fadeSpeed = 1.5f;

	void Start()
	{
		player = GameObject.FindWithTag ("Player");
	}

	// Use this for initialization
	void OnTriggerEnter2D (Collider2D col)
	{
		if (player.GetComponent<PlayerController>().loading == false) 
		{
			player.GetComponent<PlayerController>().loading = true;
			player.GetComponent<PlayerController> ().activeHint = true;
			InvokeRepeating ("FadeToBlack",0.0f, 0.02f);
			StartCoroutine ("LoadElevator");
		}
	}

	IEnumerator LoadElevator()
	{
		yield return new WaitForSeconds (5.0f);
		SceneManager.LoadSceneAsync ("Small Elevator Scene");
	}

	void FadeToBlack()
	{
		FadeImg.color = Color.Lerp (FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a == 1.0f) {
			Debug.Log (FadeImg.color.a);
			CancelInvoke ("FadeToBlack");
		}
	}
}
