using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Image fillImg;
	public Text oxygenText;
	public Text oxygenTextBG;
    float timeAmt = 45;
    float time;
	public bool started;
	public bool deathStarted = false;
    private GameObject player;

	// Use this for initialization
	void Start ()
    {
		started = true;
        fillImg = this.GetComponent<Image>();
        time = timeAmt;
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (started) {
			if ((time > 0) && player.GetComponent<PlayerController> ().hasSuit == false) {
				time -= Time.deltaTime;
				fillImg.fillAmount = time / timeAmt;
			} else if (time <= 0 && player.GetComponent<PlayerController> ().hasSuit == false && player.GetComponent<PlayerController> ().isAlive == true && deathStarted == false) {
				player.GetComponent<PlayerController> ().suffocated = true;
				deathStarted = true;
				StartCoroutine ("playerDeath");
			} else if (time <= 0 && player.GetComponent<PlayerController> ().hasSuit == false && player.GetComponent<PlayerController> ().isAlive == false && deathStarted == false) {
				deathStarted = true;
				StartCoroutine ("waitForRespawn");
			} else if (player.GetComponent<PlayerController> ().hasSuit == true) {
				fillImg.color = new Color (0, 0, 0, 0);
				oxygenText.color = new Color (0, 0, 0, 0);
				oxygenTextBG.color = new Color (0, 0, 0, 0);
			}
		}
	}

	IEnumerator playerDeath()
	{
		player.SendMessage ("die");
		yield return new WaitUntil (() =>player.GetComponent<PlayerController> ().isAlive == true);
		time = timeAmt;
		deathStarted = false;
		StopCoroutine ("playerDeath");

	}

	IEnumerator waitForRespawn()
	{
		yield return new WaitUntil (() => player.GetComponent<PlayerController> ().isAlive == true);
		time = timeAmt;
		deathStarted = false;
		StopCoroutine ("waitForRespawn");
	}
}
