using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {
    public Vector3 lPlayer_Pos;
    public PlayerData()
    {
        lPlayer_Pos = new Vector3(0, 0, 0);
    }
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void PlayerPos(Vector3 pos)
    {
        lPlayer_Pos = pos;
    }
}
