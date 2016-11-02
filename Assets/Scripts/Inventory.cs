using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    private bool[] haveItems;
    public GameObject[] items;
    int numItems;

	void Start () {
        numItems = 4;
        haveItems = new bool[numItems];
        items = new GameObject[numItems];

        //Temporarily hard-coded items, may change later
        items[0] = GameObject.Find("InvBall");
        items[2] = GameObject.Find("InvScribble");

        for (int i = 0; i < numItems; i++)
        {
            haveItems[i] = false;
            if(items[i] != null)
                items[i].GetComponent<SpriteRenderer>().enabled = false;

        }

    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void claim(int id)
    {
        Debug.Log(items[id-1]);
        haveItems[id - 1] = true;
        updateView();
    }

    private void updateView()
    {
        for (int i = 0; i < numItems; i++)
        {
            if (haveItems[i])
            {
                items[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
