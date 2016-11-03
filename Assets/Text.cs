using UnityEngine;
using System.Collections;

public class PromptText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void toggleVis(bool vis)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = vis;
    }
}
