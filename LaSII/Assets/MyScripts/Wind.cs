using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour {
    public float windForce;
    public Transform direction;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Rigidbody2D>().AddForce(getPos(direction.position - transform.position) / Vector2.Distance(getPos(direction.position), getPos(transform.position)) * windForce);
    }

    Vector2 getPos(Vector3 pos)
    {
        return new Vector2(pos.x, pos.y);
    }
}
