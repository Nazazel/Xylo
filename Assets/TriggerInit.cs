using UnityEngine;
using System.Collections;

public class TriggerInit : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
	
	void Update () {
	    
	}

    void OnTriggerStay2D(Collider2D o)
    {
        if (o.gameObject.CompareTag("Player"))
        {
            o.gameObject.SendMessage("updateCheck", gameObject.transform.position);
        }
    }

}
