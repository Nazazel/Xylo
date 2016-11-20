using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {

    private GameObject pl;

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        pl = GameObject.FindWithTag("Player");
        if ((gameObject.scene.name.Equals("Level One") || gameObject.scene.buildIndex == 0) && pl.GetComponent<PlayerController>().firstSpawnInLV1 == true)
        {
            pl.SendMessage("tp", gameObject.transform.position);
            pl.GetComponent<PlayerController>().firstSpawnInLV1 = false;
        }
        else if(gameObject.scene.buildIndex != 5 && gameObject.scene.buildIndex != 0)
        {
            pl.SendMessage("tp", gameObject.transform.position);
        }
        else if(gameObject.scene.buildIndex == 5 || gameObject.scene.buildIndex == 0)
        {
            pl.SendMessage("tpL1");
        }
    }
}
