using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LPlayer : MonoBehaviour {
    [HideInInspector]
    public float hor;                           //水平输入
    [HideInInspector]
    public bool facingRight = false;            //角色朝向
    [HideInInspector]
    public Rigidbody2D rb;                      //刚体
    [HideInInspector]
    public bool grounded = false;               //落地检验
    [HideInInspector]
    public bool jump = false;                   //跳跃开关
    [HideInInspector]
    public float co;                            //合作输入开关
    [HideInInspector]
    public bool independent;                    //角色独立状态

    //合作相关变量
    public Transform sPlayer;                   //影的位置信息
    public float coForce;                       //合作技能刚体力
    public float coSpeed;                       //合作技能初速度

    //水平移动相关变量
    public float moveSpeed = 5f;                //移动速度限制值
    public float moveForce = 50f;               //移动刚体力

    //跳跃相关变量
    public float jumpTime = 0.3f;               //跳跃判断时间

    public float initialJump = 5f;              //跳跃初速度
    public float jumpForce = 20f;               //跳跃刚体力

    private bool jumpInitial = false;           //跳跃启动检验
    private float jumpTimer;                    //跳跃判断剩余时间
    private Transform groundCheck;              //落地检验物体

    // 初始化
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("LGroundCheck");
        independent = true;
    }

    // 每帧检测指令
    void Update () {
        // 获取水平输入
        hor = Input.GetAxis("LHorizontal");
        // 获取合作输入
        co = Input.GetAxis("SCooperate");
        // 落地检测
        GroundCheck();
        // 跳跃检测
        JumpCheck();
    }
    // 固定间隔执行
    private void FixedUpdate()
    {
        Combine();
        if (independent)
        {
            // 水平移动
            Run();
            // 跳跃
            Jump();
        }
        else
        {
            DisCombine();
        }
    }
    //水平移动
    public void Run()
    {
        //如果速度小于限制值
        if (hor * rb.velocity.x < moveSpeed)
        {
            //加速（对刚体施力）
            rb.AddForce(Vector2.right * hor * moveForce);
        }
        //如果速度大于限制值
        if (hor * rb.velocity.x > moveSpeed)
        {
            //限制水平方向的速度
            rb.velocity = new Vector2(hor * moveSpeed, rb.velocity.y);
        }

        //根据移动方向翻转角色朝向
        if (hor > 0 && !facingRight)
            Flip();
        else if (hor < 0 && facingRight)
            Flip();
    }
    //检查并初始化跳跃
    public void JumpCheck()
    {
        if (grounded && Input.GetButtonDown("LJump"))
        {
            //打开跳跃开关
            jump = true;
            //初始化跳跃判断计时器
            jumpTimer = jumpTime;
            //打开跳跃启动检验（接下来的跳跃是第一次跳跃）
            jumpInitial = true;
        }
    }
    //跳跃
    public void Jump()
    {
        if (jump)
        {
            //如果跳跃刚刚启动
            if (jumpInitial)
            {
                rb.velocity = new Vector2(rb.velocity.x, initialJump);
                if (Mathf.Abs(co) == 1.0f)
                {
                    rb.velocity += new Vector2(rb.position.x - sPlayer.position.x, rb.position.y - sPlayer.position.y) * coSpeed * co / Vector2.Distance(getPos(rb.position), getPos(sPlayer.position));
                }
                jumpInitial = false;
            }
            //在跳跃判定时间内，按住跳跃键会持续施加刚体力
            else if (Input.GetButton("LJump") && jumpTimer > 0)
            {
                jumpTimer -= Time.fixedDeltaTime;
                rb.AddForce(Vector2.up * jumpForce);
                if (Mathf.Abs(co) == 1.0f)
                {
                    rb.AddForce(new Vector2(rb.position.x - sPlayer.position.x, rb.position.y - sPlayer.position.y) * coForce * co / Vector2.Distance(getPos(rb.position), getPos(sPlayer.position)));
                }

            }
            //否则结束跳跃操作
            else
            {
                jump = false;
            }
        }
    }
    //检查落地
    public void GroundCheck()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Platform"));
    }
    //合体
    void Combine()
    {
        if (Mathf.Abs(Input.GetAxis("LCooperate")) == 1.0f && independent && sPlayer.GetComponent<SPlayer>().independent)
        {
            StartCoroutine(Change());
        }
    }
    //解除合体
    void DisCombine()
    {
        if (Mathf.Abs(co) == 1.0f && sPlayer.GetComponent<SPlayer>().independent)
        {
            StartCoroutine(Restore());
        }
    }
    IEnumerator Change()
    {
        independent = false;
        sPlayer.GetComponent<SPlayer>().independent = false;
        rb.simulated = false;
        GetComponent<CircleCollider2D>().enabled = false;
        transform.DOMoveX(sPlayer.position.x, 1.0f);
        transform.DOMoveY(sPlayer.position.y, 1.0f);
        transform.DOScale(0.1f, 2.0f);
        yield return StartCoroutine(Wait(2.0f));
        GetComponent<Renderer>().enabled = false;
        sPlayer.GetComponent<SPlayer>().independent = true;
    }
    IEnumerator Restore()
    {
        GetComponent<Renderer>().enabled = true;
        transform.position = sPlayer.position;
        transform.DOScale(0.2f, 2.0f);
        yield return StartCoroutine(Wait(2.0f));
        GetComponent<CircleCollider2D>().enabled = true;
        rb.simulated = true;
        independent = true;
    }
    IEnumerator Wait(float duration)
    {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }
    //转向
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    Vector2 getPos(Vector3 pos)
    {
        return new Vector2(pos.x, pos.y);
    }
}
