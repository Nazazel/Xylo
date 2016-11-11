using UnityEngine;
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

	// Use this for initialization
	void Start ()
    {
        textManager = FindObjectOfType<TextManager>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(waitForPress && Input.GetKeyDown(KeyCode.J))
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
        if (other.name == "Stella")
        {
            if(requireButtonPress)
            {
                waitForPress = true;
                return;
            }

            textManager.ReloadScript(theText);
            textManager.currentLine = startLine;
            textManager.endAtLine = endLine;
            textManager.EnableTextBox();

            if(destroyWhenActivated)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.name == "Stella")
        {
            waitForPress = false;
        }
    }
}
