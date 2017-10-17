using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using DynamicLight2D;

public class Darkness : MonoBehaviour {
    public GameObject gameCtrl;

    [HideInInspector]
    public PolygonCollider2D pc2;           //多边形碰撞盒
    [HideInInspector]
    public MeshFilter mf;                   //mesh

    public float scalesize=1.0f;
    public bool is_Rotate;
    public Transform RCenter;
    //[Range(0, 360)]public float startr;
    //[Range(0, 360)]public float endr;
    public float speed;
    public float pauseTime = 2.0f;
    public float rotateTime;
    float tim;
    bool is_roting;
    //动画
    // Use this for initialization
    void Start () {
        pc2 = GetComponent<PolygonCollider2D>();
        if (is_Rotate)
        {
            Invoke("Rot", pauseTime);
        }
    }
	
	// Update is called once per frame
	void Update () {
        mf = GetComponent<MeshFilter>();
        pc2.SetPath(0, Path(mf.mesh.vertices));//根据mesh的路点设定多边形碰撞盒

        transform.localScale *= scalesize;
        tim = Time.deltaTime;
        if(is_roting)
        {
            transform.RotateAround(RCenter.position, new Vector3(0, 0, 1), speed * Time.deltaTime);
        }
    }
    void Rot()
    {
        is_roting = true;
        speed = -speed;
        Invoke("Pauserot", rotateTime);
    }
    void Pauserot()
    {
        is_roting = false; ;
        Invoke("Rot", pauseTime);
    }
    //void rotback()
    //{
    //    is_roting = true;
    //    speed = -speed;
    //    Invoke("rot", 2.0f);
    //}
    //寻找路点
    Vector2[] Path(Vector3[] i3)
    {
        Vector2[] i2 = new Vector2[i3.Length];
        for (int i = 0; i < i3.Length; i++)
        {
            i2[i] = new Vector2(i3[i].x, i3[i].y);
        }
        return i2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameCtrl.GetComponent<GameCtrl>().dead = true;
    }
}
