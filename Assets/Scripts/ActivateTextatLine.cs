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
	public AudioSource notesound;

    public bool isWall;
	public bool started;

    // Use this for initialization
    void Start ()
    {
		notesound = gameObject.GetComponent<AudioSource> ();
        textManager = FindObjectOfType<TextManager>();
		started = false;
        pickupText = GameObject.Find("ManualPickup").GetComponent<Text>();
		pickupText.text = "";
		player = GameObject.FindWithTag("Player");

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(waitForPress && Input.GetKeyDown(KeyCode.E))
        {
			notesound.Play ();
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
        if (isWall)
        {
            pickupText.text = "";
        }

        else
        {
            pickupText.text = "Press 'E' to Read";
        }
	

        if (other.name == "Stella")
        {
            if (requireButtonPress)
            {
                waitForPress = true;
                return;
            }

			if (gameObject.name == "Trigger" && !started) {
				started = true;
				player.GetComponent<PlayerController> ().activeHint = true;
				player.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;				
				StartCoroutine ("borkSequence");
			} else if (!started) {
				textManager.ReloadScript (theText);
				textManager.currentLine = startLine;
				textManager.endAtLine = endLine;
				textManager.EnableTextBox ();
			}

			if (destroyWhenActivated && !started)
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

	public IEnumerator borkSequence()
	{
		yield return new WaitForSeconds (0.5f);
		player.GetComponent<PlayerController> ().playerAnimator.Play ("SpaceDiscomfort");
		yield return new WaitForSeconds (2.0f);
		textManager.ReloadScript (theText);
		textManager.currentLine = startLine;
		textManager.endAtLine = endLine;
		textManager.EnableTextBox ();
	}
}
