﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatSwitch : MonoBehaviour {
    public GameObject plat;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "LPlayer")
        {
            plat.GetComponent<PlatLCtrl>().Mov = true;
        }
    }
    
    
}
