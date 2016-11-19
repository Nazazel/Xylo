using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TempLevelTransition : MonoBehaviour {

    public int sceneDest;
	bool loading = false;
    public GameObject player;

	void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

	void OnTriggerEnter2D (Collider2D col)
	{
        
        if (loading == false)
        {
            loading = true;
            if (sceneDest != 0)
            {
                player.SendMessage("setLv1Coords");
                player.SendMessage("findFirstSpawnDelayed", sceneDest);
            }
            SceneManager.LoadSceneAsync(sceneDest);
            }

    }
}
