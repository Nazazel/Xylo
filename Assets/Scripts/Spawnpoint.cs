using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {

    GameObject inv;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        GameObject pl = GameObject.Find("Player");
        pl.SendMessage("tp", gameObject.transform.position);
        inv = GameObject.Find("Inventory Slots");
        inv.SendMessage("bindToCam");
    }
	
	void Update () {
	    
	}

}
