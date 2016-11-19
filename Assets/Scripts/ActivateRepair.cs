using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivateRepair : MonoBehaviour
{

    public bool requireButtonPress;
    private bool waitForPress;

    public bool destroyWhenActivated;

    public Text pickupText;
    public GameObject player;
    public GameObject repairBar;
    public bool isDown;


    // Use this for initialization
    void Start()
    {

        pickupText = GameObject.Find("ManualPickup").GetComponent<Text>();
        pickupText.text = "";
        player = GameObject.FindWithTag("Player");
        repairBar = GameObject.FindWithTag("RepairBar");

    }

    // Update is called once per frame
    void Update()
    {
        if (waitForPress && Input.GetKey(KeyCode.R) && !isDown)
        {
            repairBar.GetComponent<Timer2>().started = true;
            isDown = true;

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }

          
        }

        else
        {
            repairBar.GetComponent<Timer2>().started = false;
            isDown = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

            pickupText.text = "Hold 'R' to repair";

        if (other.name == "Stella")
        {
            if (requireButtonPress)
            {
                waitForPress = true;
                return;
            }


            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }




    }

    void OnTriggerExit2D(Collider2D other)
    {
        pickupText.text = "";
        if (other.name == "Stella")
        {
            waitForPress = false;
        }
    }
}
