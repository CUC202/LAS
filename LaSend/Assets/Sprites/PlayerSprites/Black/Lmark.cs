using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lmark : MonoBehaviour {
    [HideInInspector]
    public enum Status { down, right, left, up };//角色状态
    [HideInInspector]
    public Status status;              //角色当前状态
    public GameObject lPlayer;
    float disX, disY;
    void Start () {
        status = Status.right;
	}
	
	// Update is called once per frame
	void Update () {
        disY = lPlayer.transform.position.y - transform.position.y;
        disX = lPlayer.transform.position.x - transform.position.x;
        if (disY - disX > 0)
        {
            if (disX + disY >= 0)
                status = Status.down;
            else
                status = Status.right;
        }
        else
        {
            if (disX + disY > 0)
                status = Status.left;
            else
                status = Status.up;
        }
        
	}
}
