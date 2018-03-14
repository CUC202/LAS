using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Physics.gravity = new Vector3(-Physics.gravity.y, Physics.gravity.x, 0);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));
            Debug.Log(Physics.gravity.y);
            Debug.Log(Physics.gravity.x);
        }
    }
}
