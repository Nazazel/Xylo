using UnityEngine;
using System.Collections;

public class Spacesuit : MonoBehaviour {

    private bool claimed = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.gameObject.CompareTag("Player") && !claimed)
        {
            o.gameObject.SendMessage("canPickUp", true);
            o.gameObject.SendMessage("setRef", gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D o)
    {
        if (o.gameObject.CompareTag("Player") && !claimed)
        {
            o.gameObject.SendMessage("canPickUp", false);
        }
    }


    public void claim()
    {
        claimed = true;
        gameObject.SetActive(false);
    }
}
