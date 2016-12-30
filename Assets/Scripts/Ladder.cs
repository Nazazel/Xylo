using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

    private bool pClimb = false;
    private int ladderType;
    private float[] lBounds; //0 is lower bound, 1 is upper, 2 is the x position of the ladder
    private GameObject player;

	void Start () {
        player = GameObject.FindWithTag("Full Player");
        if (gameObject.CompareTag("LadderBot")) ladderType = 0;
        else if (gameObject.CompareTag("LadderTop")) ladderType = 1;
        else ladderType = -1;
        gameObject.name = ladderType+"";
        lBounds = new float[3];
        lBounds[2] = gameObject.transform.position.x;
        Invoke("getLBounds",0.5f);

    }
    private void getLBounds()
    {
        if (ladderType == 0)
        {
            lBounds[0] = gameObject.transform.position.y;
            lBounds[1] = transform.parent.FindChild("1").position.y+transform.parent.FindChild("1").localScale.y*transform.parent.FindChild("1").gameObject.GetComponent<BoxCollider2D>().size.y;
        }
        if (ladderType == 1)
        {
            lBounds[1] = gameObject.transform.position.y+gameObject.transform.localScale.y*gameObject.GetComponent<BoxCollider2D>().size.y;
            lBounds[0] = transform.parent.FindChild("0").position.y;
        }
    }

    // Update is called once per frame
    void Update () {
	    
	}

    void OnTriggerEnter2D(Collider2D o)
    {

        if (!pClimb)
        {
            o.SendMessage("canClimb", true);
            o.SendMessage("passLadderBounds", lBounds);
        }
    }

    void OnTriggerExit2D(Collider2D o)
    {

        if (!pClimb)
        {
            o.SendMessage("canClimb", false);
        }
    }
}
