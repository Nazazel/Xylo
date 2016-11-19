using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Timer2 : MonoBehaviour
{
    Image fillImg;
    public Text repairText;
    float timeAmt = 10;
    float time;
    public bool started;
    public bool isDone;
    // Use this for initialization
    void Start()
    {

        fillImg = this.GetComponent<Image>();
        time = timeAmt;
        started = false;
        repairText.enabled = false;
        fillImg.enabled = false;
        isDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (time > 0)
            {
                repairText.enabled = true;
                fillImg.enabled = true;
                time -= Time.deltaTime;
                fillImg.fillAmount = (timeAmt - time) / timeAmt;
                //Debug.Log((timeAmt - time) / timeAmt);


                if (((timeAmt - time) / timeAmt) > 1)
                {
                    fillImg.color = new Color(0, 0, 0, 0);
                    repairText.color = new Color(0, 0, 0, 0);
                    isDone = true;
                }
            }

        }
    }
}

