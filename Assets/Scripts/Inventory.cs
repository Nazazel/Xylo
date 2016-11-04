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
        Item.Inv = this;
        Item.Ready = true;

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

    public void passItem(GameObject go, int id)
    {
        id--;
        items[id] = new GameObject();
        items[id].name = "Inv" + go.name;
        items[id].transform.position = new Vector2(-1.1155f+0.74775f*id,10.8f);
        if (items[id].GetComponent<SpriteRenderer>() == null)
        {
            items[id].AddComponent<SpriteRenderer>();
            var sprite = items[id].GetComponent<SpriteRenderer>();
            sprite.sprite = go.GetComponent<SpriteRenderer>().sprite;
            sprite.sortingOrder = 1;
            sprite.sortingLayerName = "InvItems";
            sprite.enabled = false;
        }
    }
}
