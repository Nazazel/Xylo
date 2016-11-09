using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    private bool claimed;
    public int id;
    private GameObject textO;
    public static Inventory Inv;
    public static bool Ready = false;
    private bool added = false;
    private bool abortAdd = false;
    private int attempts = 0;

	void Start () {
        claimed = false;
        textO = GameObject.Find("Pick Up Text");
	}
	
	void Update () {
        if (Ready)
        {
            if (attempts < 3 && !added && !abortAdd)
            {
                try { Inv.passItem(gameObject, id); added = true;}
                catch (System.Exception ex2)
                {
                    Debug.Log("Item with ID " + id + " not added. Trying again...");
                    attempts++;
                }
            }
            else if (!added && !abortAdd)
            { Debug.Log("Item with ID " + id + " could not be added."); abortAdd = true; }
        }
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
