using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TempLevelTransition : MonoBehaviour {

    public int sceneDest;
	bool loading = false;
    private float doorCool = 0f;

	void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (doorCool < 2f)
            doorCool += Time.deltaTime;
    }

	void OnTriggerStay2D (Collider2D col)
	{
        if (Input.GetKey(KeyCode.F) && doorCool >= 2f)
        {
            if (loading == false)
            {
                loading = true;
                if (sceneDest != 0 && sceneDest != 5)
                {
                    col.gameObject.SendMessage("setLv1Coords");
                }
                SceneManager.LoadSceneAsync(sceneDest);
            }
        }
    }
}
