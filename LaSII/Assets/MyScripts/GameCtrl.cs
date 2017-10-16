using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCtrl : MonoBehaviour {
    public bool dead;

	// Use this for initialization
	void Start () {
        dead = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            SceneManager.LoadScene("Scene1.1");
        }
	}
}
