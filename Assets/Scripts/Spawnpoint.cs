using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {

    private GameObject pl;

	// Use this for initialization
	void Start () {
		Debug.Log (gameObject.name);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
		Debug.Log (gameObject.name);
        pl = GameObject.FindWithTag("Player");
        pl.SendMessage("tp", gameObject.transform.position);
    }
}
