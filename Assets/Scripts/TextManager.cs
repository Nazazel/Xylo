using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TextManager : MonoBehaviour
{
    public GameObject textBox;
    public Text theText;


    public TextAsset textFile;
    public string[] textLines;


    public int currentLine;
    public int endAtLine;


    //to stop player when in dialogue
	public GameObject player;

    public bool isActive;

    public bool stopPlayerMovement;


    // Use this for initialization
    void Start()
    {
		player = GameObject.FindWithTag("Player");

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }


        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }

        if(isActive)
        {
            EnableTextBox();
        }

        else
        {
            DisableTextBox();
        }
    }


    void Update()
    {

        if(!isActive)
        {
            return;
        }

        theText.text = textLines[currentLine];


        if (Input.GetKeyDown(KeyCode.Return))
        {
            currentLine += 1;
        }


        if (currentLine > endAtLine)
        {
            DisableTextBox();

        }
    }

    public void EnableTextBox()
    {
		player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
		player.GetComponent<PlayerController> ().activeHint = true;
        textBox.SetActive(true);
        isActive = true;
    }

    public void DisableTextBox()
    {
		player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeRotation;
		player.GetComponent<PlayerController> ().finishedJump = true;
		player.GetComponent<PlayerController> ().activeHint = false;
        textBox.SetActive(false);
        isActive = false;
    }

    public void ReloadScript(TextAsset theText)
    {
        if(theText != null)
        {
            textLines = new string[1];
            textLines = (theText.text.Split('\n'));
        }
    }

}
