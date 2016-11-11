using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    private bool[] haveItems;
    public GameObject[] items;
    private GameObject cam;
    int numItems;

	void Start () {
        cam = GameObject.Find("2DCamera");
        gameObject.transform.parent = cam.transform;
        numItems = 8;
        haveItems = new bool[numItems];
        items = new GameObject[numItems];
        Item.Inv = this;
        Item.Ready = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void claim(int id)
    {
        id--;
        items[id].transform.position = new Vector3(-1.1155f + 0.747f * (id-2) + gameObject.transform.position.x, gameObject.transform.position.y, 0);
        haveItems[id] = true;
        items[id].transform.parent = gameObject.transform;
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
}
