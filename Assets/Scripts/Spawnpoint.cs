using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        GameObject pl = GameObject.Find("Player");
        pl.SendMessage("tp", gameObject.transform.position);
    }
	
	void Update () {
	    
	}

}
