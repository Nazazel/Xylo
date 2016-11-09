using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
	
	void Update () {
	    
	}

    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.gameObject.CompareTag("Player"))
        {
            o.gameObject.SendMessage("updateCheck", gameObject.transform.position);
        }
    }

}
