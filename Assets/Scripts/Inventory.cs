using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    private bool[] haveItems;
    private Vector3[] iOffset;
    public GameObject[] items;
    private GameObject cam;
    int numItems;
    private Vector3 offset;

	void Start () {
        DontDestroyOnLoad(gameObject);
        cam = GameObject.Find("Main Camera");
        offset = gameObject.transform.position - cam.transform.position;
        numItems = 4;
        haveItems = new bool[numItems];
        items = new GameObject[numItems];
        iOffset = new Vector3[numItems];
        Item.Inv = this;
        Item.Ready = true;

    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    void LateUpdate()
    {
        gameObject.transform.position = cam.transform.position + offset;
        updateItemPos();
    }

    private void updateItemPos()
    {
        for(int i=0;i<numItems;i++)
        {
            if(haveItems[i])  items[i].transform.position = gameObject.transform.position + iOffset[i];
        }
    }

    public void claim(int id)
    {
        id--;
        items[id].transform.position = new Vector3(-1.1155f + 0.74775f * id + gameObject.transform.position.x, gameObject.transform.position.y, 0);
        iOffset[id] = items[id].transform.position - gameObject.transform.position;
        haveItems[id] = true;
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
        if (items[id].GetComponent<SpriteRenderer>() == null)
        {
            items[id].AddComponent<SpriteRenderer>();
            var sprite = items[id].GetComponent<SpriteRenderer>();
            sprite.sprite = go.GetComponent<SpriteRenderer>().sprite;
            sprite.sortingOrder = 1;
            sprite.sortingLayerName = "InvItems";
            sprite.enabled = false;
            DontDestroyOnLoad(items[id]);
        }
    }

    public void bindToCam()
    {
        cam = GameObject.Find("Main Camera");
    }
}
