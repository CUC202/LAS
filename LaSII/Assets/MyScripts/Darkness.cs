using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
public class Darkness : MonoBehaviour {
    public float scalesize=1.0f;
    public bool is_Rotate;
    public Vector3 RCenter;
    //[Range(0, 360)]public float startr;
    //[Range(0, 360)]public float endr;
    public float speed;
    public float pauseTime=2.0f;
    float tim;
    bool is_roting;
    //动画
    // Use this for initialization
    void Start () {
        if (is_Rotate)
        {
            Invoke("rot", pauseTime);
        }
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.localScale *= scalesize;
        tim = Time.deltaTime;
        if(is_roting)
        {
            transform.RotateAround(RCenter, new Vector3(0, 0, 1), speed * Time.deltaTime);
        }
    }
    void rot()
    {
        is_roting = true;
        speed = -speed;
        Invoke("pauserot", 2.0f);
    }
    void pauserot()
    {
        is_roting = false; ;
        Invoke("rot", pauseTime);
    }
    //void rotback()
    //{
    //    is_roting = true;
    //    speed = -speed;
    //    Invoke("rot", 2.0f);
    //}
}
