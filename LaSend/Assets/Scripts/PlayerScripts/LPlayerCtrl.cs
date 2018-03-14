using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPlayerCtrl : MonoBehaviour {
    [HideInInspector]
    public enum Status { normal, light};//角色状态
    [HideInInspector]
    public Status status;              //角色当前状态

    Rigidbody2D rBody;                 //刚体
    [HideInInspector]
    public bool facingRight = false;   //角色朝向
    public GameObject sPlayer;         //
    /// <summary>
    /// /////////////////////////水平移动变量//////////////////////////////////
    /// </summary>
    public float moveSpeed;            //角色对自身水平移动速度限制
    public float moveForce;            //角色自身水平移动力

    float hSpeed;                      //外界对角色水平移动速度限制
    [HideInInspector]
    public float platSpeed;            //平台属性提供的速度限制变量（例如传送带）
    [HideInInspector]
    public float environSpeed;         //环境属性提供的速度限制变量（例如风）

    public float lightMoveSpeed;       //发光状态移动速度限制
    public float lightMoveForce;       //发光状态移动力
    /// <summary>
    /// //////////////////////////轴变量//////////////////////////////////////
    /// </summary>
    float hor;                         //水平输入
    float ver;                         //垂直输入
    /// <summary>
    /// /////////////////////////跳跃变量//////////////////////////////////
    /// </summary>
    public float jumpSpeed;            //跳跃初速度
    public float jumpForce;            //跳跃力
    public float jumpTime;             //跳跃判断时间
    public Transform groundCheck;      //落地检验物体的空间状态
    public LayerMask isGround;         //地面层

    bool jump;                         //跳跃状态确认
    bool grounded;                     //落地检验
    bool jumpInitial;                  //跳跃初次启动确认
    float jumpTimer;                   //跳跃判断剩余时间

    // 初始化
    void Start()
    {
        status = Status.normal;
        rBody = GetComponent<Rigidbody2D>();
        platSpeed = 0.0f;
        environSpeed = 0.0f;
    }

    // 每帧检测并更新状态
    void Update () {
        switch (status)
        {
            case Status.normal:
                NormalCtrl();
                break;
            case Status.light:
                LightCtrl();
                break;
        }
	}

    // 固定间隔根据角色状态更新物理系统
    private void FixedUpdate()
    {
        switch (status)
        {
            case Status.normal:
                NormalFixedCtrl();
                break;
            case Status.light:
                LightFixedCtrl();
                break;
        }
    }

    // 角色水平移动，单独写为了方便做过场（如果有的话）
    public void Move(int direction)
    {
        //更新外界对角色水平移动速度限制
        hSpeed = platSpeed + environSpeed;
        //如果当前速度没有达到角色当前位置输入方向上的速度限制，加速
        if (direction * rBody.velocity.x < moveSpeed + direction * hSpeed)
            rBody.AddForce(Vector2.right * direction * moveForce);
        //否则，限制移动速度
        if (direction * rBody.velocity.x > moveSpeed + direction * hSpeed || direction == 0)//direction == 0影响惯性
            rBody.velocity = new Vector2(direction * moveSpeed + hSpeed, rBody.velocity.y);
    }
    //转向
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // 检测并初始化跳跃
    void JumpCheck()
    {
        if (grounded && Input.GetButtonDown("LJump"))
        {
            //确认跳跃状态
            jump = true;
            //确认跳跃动作（Jump）是初次启动
            jumpInitial = true;
            //初始化跳跃判断计时器
            jumpTimer = jumpTime;
        }//down影响手感有待验证
    }

    // 跳跃
    void Jump()
    {
        //如果跳跃刚刚启动，赋予角色初速度
        if (jumpInitial)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
            jumpInitial = false;
        }
        //启动后在跳跃判定时间内，按住跳跃键会持续施加刚体力
        else if (jumpTimer > 0 && Input.GetButton("LJump"))
        {
            jumpTimer -= Time.fixedDeltaTime;
            rBody.AddForce(Vector2.up * jumpForce);
        }
        //判定结束后，结束跳跃操作
        else
        {
            jump = false;
        }
    }

    // 落地检验
    void GroundCheck()
    {
        grounded = Physics2D.OverlapPoint(groundCheck.position, isGround);
    }

    // 检测并初始化发光状态
    void LightCheck()
    {
        if (Input.GetButtonDown("Light"))
        {
            //切换状态
            status = Status.light;
            //去除重力
            rBody.gravityScale = 0;
        }
    }

    //Normal状态
    void NormalCtrl()
    {
        hor = Input.GetAxis("LHorizontal");
        //根据移动方向翻转角色朝向
        if (hor > 0 && !facingRight)
            Flip();
        else if (hor < 0 && facingRight)
            Flip();
        GroundCheck();
        JumpCheck();
        LightCheck();
    }
    void NormalFixedCtrl()
    {
        //水平移动（direction==0无限制时，以下写法影响惯性）
        if (hor == 1)
            Move(1);
        else if (hor == -1)
            Move(-1);
        else
            Move(0);
        //跳跃
        if (jump)
            Jump();
    }

    //结束发光状态
    void LightEnd()
    {
        if (Input.GetButtonDown("Light"))
        {
            status = Status.normal;
            rBody.gravityScale = 3;
        }
    }

    //发光状态移动
    void LightMove(float directionX, float directionY)
    {
        //如果当前速度没有达到角色当前位置水平输入方向上的速度限制，加速
        if (directionX * rBody.velocity.x < lightMoveSpeed)
            rBody.AddForce(Vector2.right * directionX * lightMoveForce);
        //否则，限制移动速度
        if (directionX * rBody.velocity.x > lightMoveSpeed)
            rBody.velocity = new Vector2(directionX * lightMoveSpeed, rBody.velocity.y);
        //如果当前速度没有达到角色当前位置竖直输入方向上的速度限制，加速
        if (directionY * rBody.velocity.y < lightMoveSpeed)
            rBody.AddForce(Vector2.up * directionY * lightMoveForce);
        //否则，限制移动速度
        if (directionY * rBody.velocity.y > lightMoveSpeed)
            rBody.velocity = new Vector2(rBody.velocity.x, directionY * lightMoveSpeed);
    }

    //Light状态
    void LightCtrl()
    {
        hor = Input.GetAxis("LHorizontal");
        ver = Input.GetAxis("LVertical");
        LightEnd();
    }
    void LightFixedCtrl()
    {
        LightMove(hor, ver);
    }
}
