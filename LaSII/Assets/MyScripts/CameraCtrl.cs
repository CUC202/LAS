using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {
    public Transform LPlayer;
    public Transform SPlayer;

    private Camera cam;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3((LPlayer.position + SPlayer.position).x / 2, (LPlayer.position + SPlayer.position).y / 2, -10);
	}
    
}
