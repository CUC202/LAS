using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//长平台上的三个点光源，固定不动，定时闪烁（类似萤火虫）
//之后的两个悬挂点光源，会弧形摆动（以悬挂点为圆心旋转）
//之后会在editor里做详细的规划
public class Lightings : MonoBehaviour {
    public GameObject gameCtrl;

    public float size=1.0f;
    [Tooltip("1=点形  2=柱形  3=扇形 4=能量")]public int type;

    public bool is_Rotate;
    public bool is_Move;
    public bool is_Flash;
    public Vector2 startp;
    public Vector2 endp;
    public float speed;
    public float pauseTime;
    public Color lightColor;
    //flash
    private bool is_flashing = false;
    public float flashTime;
    public float flashScale;

    [Range(0,1)]public float alpha;
    Vector3 nowsc;
    // Use this for initialization

    int colortrans;
    void Awake()
    {
        //transform.position = new Vector3(startp.x, startp.y, 0);
        //transform.localScale = new Vector3(size, size, 1);
        nowsc = transform.localScale;
        lightColor = GetComponent<SpriteRenderer>().color;
    }
    void Start () {
     }
	
	// Update is called once per frame
	void Update () {
        Move();
    }
    void Move()
    {
        switch (type)
        {
            case 1:
                {
                    if (is_Move) Fly();
                    if (is_Rotate) Rot();
                    if (is_Flash && !is_flashing)
                        Flash();
                }
                break;
            case 2: break;
            case 3: break;
            case 4: break;
            default: break;
        }
    }
    void Fly()//点形平移
    {
        transform.DOLocalMove(new Vector3(endp.x, endp.y, 0), 2.0f, false);
        Invoke("Stay", 2.0f);
    }
    void Flyback()
    {
        transform.DOLocalMove(new Vector3(startp.x, startp.y, 0), 2.0f, false);
        Invoke("Stay", 2.0f);
    }
    /// <summary>
    /// ///////////////////////
    /// </summary>
    void Rot()//柱形旋转
    {

    }
    /// <summary>
    /// //////////////
    /// </summary>
    void Sca()//扇形扩大和缩小
    {

    }
    /// <summary>
    /// /////////////////
    /// </summary>
    /*void flash()//不动闪烁
    {
        
        this.transform.DOScale(nowsc * 2.0f, 1.0f);
        Invoke("flash2", 1.0f);
    }
    void flash2()
    {
        this.GetComponent<SpriteRenderer>().DOBlendableColor(new Color(1.0f, 1.0f, 1.0f, colortrans / 255), 1.0f);
        this.transform.DOScale(nowsc, 1.0f);
        Invoke("flash", 1.0f);
    }*/
    void Flash()
    {
        StartCoroutine(FlashLight());
    }
    IEnumerator FlashLight()
    {
        is_flashing = true;
        GetComponent<SpriteRenderer>().DOBlendableColor(new Color(1.0f, 1.0f, 1.0f, alpha), flashTime);
        transform.DOScale(nowsc * flashScale, flashTime);
        yield return StartCoroutine(Wait(flashTime));
        GetComponent<SpriteRenderer>().DOBlendableColor( lightColor, flashTime);
        transform.DOScale(nowsc, flashTime);
        yield return StartCoroutine(Wait(flashTime));
        is_flashing = false;
    }
    IEnumerator Wait(float duration)
    {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }
    /// <summary>
    /// /////////////////////////
    /// </summary>
    void Stay()
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameCtrl.GetComponent<GameCtrl>().dead = true;
    }

}
