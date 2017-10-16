using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down : MonoBehaviour {
    public GameObject gameCtrl;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameCtrl.GetComponent<GameCtrl>().dead = true;
    }
}
