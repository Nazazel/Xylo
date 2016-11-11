using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TempLevelTransition : MonoBehaviour {

	bool loading = false;
	public Image FadeImg;
	public float fadeSpeed = 1.5f;

	// Use this for initialization
	void OnTriggerEnter2D (Collider2D col)
	{
		if (loading == false) 
		{
			loading = true;
            //InvokeRepeating ("FadeToBlack",0.0f, 0.02f);
            //StartCoroutine ("LoadElevator");
            SceneManager.LoadSceneAsync("Engineering Wing");
		}
	}

	IEnumerator LoadElevator()
	{
		yield return new WaitForSeconds (5.0f);
		//SceneManager.LoadSceneAsync ("Elevator");
	}

	void FadeToBlack()
	{
		FadeImg.color = Color.Lerp (FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a == 1.0f) {
			CancelInvoke ("FadeToBlack");
		}
	}
}
