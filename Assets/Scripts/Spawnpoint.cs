using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {

    GameObject inv;
    private GameObject pl;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        pl = GameObject.Find("Stella");
        pl.SendMessage("tp", gameObject.transform.position);
    }
	
	void Update () {
	    
	}

}
