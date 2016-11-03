using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElevatorScene : MonoBehaviour {

	public Image FadeImg;
	public float fadeSpeed = 1.5f;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("FadeToClear", 1.0f, 0.1f);
	}

	void FadeToClear()
	{
		FadeImg.color = Color.Lerp (FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
		if (FadeImg.color.a < 0.05f) {
			CancelInvoke ("FadeToClear");
			FadeImg.color = Color.clear;
		}
	}
}
