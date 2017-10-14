using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//之后会在editor里做详细的规划
public class Lightings : MonoBehaviour {
    public float size;
    [Tooltip("0=点形  1=柱形  2=扇形 3=能量")]public int type;

    public bool is_Rotate;
    public bool is_Moving;
    public Vector2 startp;
    public Vector2 endp;
    public float speed;
    public float pauseTime;
    // Use this for initialization
    void Start () {
        transform.position = new Vector3(startp.x, startp.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
    }
    void move()
    {
        switch (type)
        {
            case 0: fly(); break;
            case 1: break;
            case 2: break;
            case 3: break;
            default: break;
        }
    }
    void fly()//点形平移
    {
        transform.DOLocalMove(new Vector3(endp.x, endp.y, 0), 2.0f, false);
        Invoke("stay", 2.0f);
    }
    void rot()//柱形旋转
    {

    }
    void sca()//扇形扩大和缩小
    {

    }
    void stay()
    {
        switch (type)
        {
            case 0: Invoke("flyback", pauseTime); break;
            case 1: break;
            case 2: break;
            case 3: break;
            default: break;
        }
       
    }
    void flyback()
    {
        transform.DOLocalMove(new Vector3(startp.x, startp.y, 0), 2.0f, false);
        Invoke("stay", 2.0f);
    }
}
