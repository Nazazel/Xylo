using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    private bool claimed;
    public int id;
    private GameObject textO;

	void Start () {
        claimed = false;
        textO = GameObject.Find("Pick Up Text");
	}
	
	void Update () {
	    
	}
    void OnTriggerEnter2D(Collider2D o)
    {
        if (o.gameObject.CompareTag("Player") && !claimed)
        {
            o.gameObject.SendMessage("canPickUp", true);
            o.gameObject.SendMessage("setItemID", id);
            o.gameObject.SendMessage("setRef", gameObject);
            textO.SendMessage("toggleVis", true);
            
        }
    }
    void OnTriggerExit2D(Collider2D o)
    {
        if (o.gameObject.CompareTag("Player") && !claimed)
        {
            o.gameObject.SendMessage("canPickUp", false);
            textO.SendMessage("toggleVis", false);
        }
    }

    public void claim()
    {
        claimed = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        textO.SendMessage("toggleVis", false);
    }
}
