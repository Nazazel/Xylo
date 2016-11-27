using UnityEngine;
using System.Collections;

public class Lev1Spawn : MonoBehaviour
{

    private GameObject pl;
    public int spawnId;
    private bool active = false;
    private int obj;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        obj = pl.GetComponent<PlayerController>().currentObjective;
        pl = GameObject.FindWithTag("Player");
        if (spawnId == 1 && (obj == 4 ))
            active = true;
        else if (spawnId == 2 && (obj == 5 ))
            active = true;
        else if (spawnId == 3 && (obj >= 8 && obj <= 10))
            active = true;
        if(active)
            pl.SendMessage("tp", gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
