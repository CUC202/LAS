using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//长平台上的三个点光源，固定不动，定时闪烁（类似萤火虫）
//之后的两个悬挂点光源，会弧形摆动（以悬挂点为圆心旋转）
//之后会在editor里做详细的规划
public class Lightings : MonoBehaviour {
    public float size=1.0f;
    [Tooltip("1=点形  2=柱形  3=扇形 4=能量")]public int type;

    public bool is_Rotate;
    public bool is_Moving;
    public bool is_Flashing;
    public Vector2 startp;
    public Vector2 endp;
    public float speed;
    public float pauseTime;
    [Range(0,255)]public int 透明度=100;
    Vector3 nowsc;
    // Use this for initialization

    int colortrans;
    void Awake()
    {
        colortrans = 255 - 透明度;
        //transform.position = new Vector3(startp.x, startp.y, 0);
        this.transform.localScale = new Vector3(size, size, 1);
        nowsc = this.transform.localScale*size;
    }
    void Start () {
        move();
     }
	
	// Update is called once per frame
	void Update () {
        //move();
    }
    void move()
    {
        switch (type)
        {
            case 1:
                {
                    if (is_Moving) fly();
                    if (is_Rotate) rot();
                    if (is_Flashing) this.GetComponent<SpriteRenderer>().DOBlendableColor(new Color(1.0f, 1.0f, 1.0f, colortrans / 255), 1.0f);flash();
                }
                break;
            case 2: break;
            case 3: break;
            case 4: break;
            default: break;
        }
    }
    void fly()//点形平移
    {
        transform.DOLocalMove(new Vector3(endp.x, endp.y, 0), 2.0f, false);
        Invoke("stay", 2.0f);
    }
    void flyback()
    {
        transform.DOLocalMove(new Vector3(startp.x, startp.y, 0), 2.0f, false);
        Invoke("stay", 2.0f);
    }
    /// <summary>
    /// ///////////////////////
    /// </summary>
    void rot()//柱形旋转
    {

    }
    /// <summary>
    /// //////////////
    /// </summary>
    void sca()//扇形扩大和缩小
    {

    }
    /// <summary>
    /// /////////////////
    /// </summary>
    void flash()//不动闪烁
    {
        
        this.transform.DOScale(nowsc * 2.0f, 1.0f);
        Invoke("flash2", 1.0f);
    }
    void flash2()
    {
        //this.GetComponent<SpriteRenderer>().DOBlendableColor(new Color(1.0f, 1.0f, 1.0f, colortrans / 255), 1.0f);
        this.transform.DOScale(nowsc, 1.0f);
        Invoke("flash", 1.0f);
    }
    /// <summary>
    /// /////////////////////////
    /// </summary>
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
    
}
