using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {

    private GameObject pl;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        pl = GameObject.FindWithTag("Player");
        pl.SendMessage("tp", gameObject.transform.position);
    }
}
