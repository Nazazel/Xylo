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
	    if (time > 0)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt;
        }
        else
        {
            player.SendMessage("die");
        }
	}
}
