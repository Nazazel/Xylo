using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Image fillImg;
    float timeAmt = 45;
    float time;
    private GameObject player;

	// Use this for initialization
	void Start ()
    {
        fillImg = this.GetComponent<Image>();
        time = timeAmt;
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
		if ((time > 0) && player.GetComponent<PlayerController> ().hasSuit == false) {
			time -= Time.deltaTime;
			fillImg.fillAmount = time / timeAmt;
		} else if (time <= 0 && player.GetComponent<PlayerController> ().hasSuit == false && player.GetComponent<PlayerController> ().isAlive == true) {
			player.GetComponent<PlayerController> ().suffocated = true;
			player.SendMessage ("die");
			time = timeAmt;
		}
		else if (time <= 0 && player.GetComponent<PlayerController> ().hasSuit == false && player.GetComponent<PlayerController> ().isAlive == false) {
			time = timeAmt;
		}
	}
}
