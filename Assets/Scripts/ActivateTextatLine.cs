using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivateTextatLine : MonoBehaviour
{
    public TextAsset theText;
    public int startLine;
    public int endLine;

    public TextManager textManager;

    public bool requireButtonPress;
    private bool waitForPress;

    public bool destroyWhenActivated;

    public Text pickupText;
    public GameObject player;

    // Use this for initialization
    void Start ()
    {
        textManager = FindObjectOfType<TextManager>();

        pickupText = GameObject.Find("ManualPickup").GetComponent<Text>();
		pickupText.text = "";
		player = GameObject.FindWithTag("Player");

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(waitForPress && Input.GetKeyDown(KeyCode.E))
        {
            textManager.ReloadScript(theText);
            textManager.currentLine = startLine;
            textManager.endAtLine = endLine;
            textManager.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {

		pickupText.text = "Press 'E' to pick up";

        if (other.name == "Stella")
        {
            if (requireButtonPress)
            {
                waitForPress = true;
                return;
            }

            textManager.ReloadScript(theText);
            textManager.currentLine = startLine;
            textManager.endAtLine = endLine;
            textManager.EnableTextBox();

            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }

        

        
    }

    void OnTriggerExit2D(Collider2D other)
    {
		pickupText.text = "";
        if(other.name == "Stella")
        {
            waitForPress = false;
        }
    }
}
