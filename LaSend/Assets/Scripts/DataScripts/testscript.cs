using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour {
    public GameObject gameManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "LPlayer")
        {
            gameManager.GetComponent<GameManager>().pd.PlayerPos(collision.transform.position);
            gameManager.GetComponent<GameManager>().Save();
        }
    }
}
