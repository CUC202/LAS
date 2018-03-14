using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPlayerCtrl : MonoBehaviour {
    [HideInInspector]
    public enum Status { down, right, up, left };//角色状态
    [HideInInspector]
    public Status status;              //角色当前状态

    Rigidbody2D rBody;                 //刚体
    [HideInInspector]
    public bool facingRight = false;   //角色朝向
    public GameObject lMark;           //转向标记
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
    /// <summary>
    /// //////////////////////////轴变量//////////////////////////////////////
    /// </summary>
    float hor;                         //水平输入
    float ver;                         //垂直输入
    public Vector2 Sgravity2D;         //重力
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
        status = Status.down;
        rBody = GetComponent<Rigidbody2D>();
        Sgravity2D = new Vector2(0, -29.43f);
        platSpeed = 0.0f;
        environSpeed = 0.0f;
    }

    // 每帧检测并更新状态
    void Update () {
        hor = Input.GetAxis("SHorizontal");
        //根据移动方向翻转角色朝向
        if (hor > 0 && !facingRight)
            Flip();
        else if (hor < 0 && facingRight)
            Flip();
        GroundCheck();
        JumpCheck();
        GChangeCheck();
    }

    //
    private void FixedUpdate()
    {
        rBody.AddForce(Sgravity2D);
        switch (status)
        {
            case Status.down:
                FixupGDown();
                break;
            case Status.up:
                FixupGUp();
                break;
            case Status.right:
                FixupGRight();
                break;
            case Status.left:
                FixupGLeft();
                break;
        }
    }

    //检测并转换重力状态
    void GChangeCheck()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switch (lMark.GetComponent<Lmark>().status)
            {
                case Lmark.Status.down:
                    Sgravity2D = new Vector2(0, -29.43f);
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    status = Status.down;
                    break;
                case Lmark.Status.right:
                    Sgravity2D = new Vector2(29.43f, 0);
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    status = Status.right;
                    break;
                case Lmark.Status.left:
                    Sgravity2D = new Vector2(-29.43f, 0);
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                    status = Status.left;
                    break;
                case Lmark.Status.up:
                    Sgravity2D = new Vector2(0, 29.43f);
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    status = Status.up;
                    break;
            }
        }
    }
    // 检测并初始化跳跃
    void JumpCheck()
    {
        if (grounded && Input.GetButtonDown("SJump"))
        {
            //确认跳跃状态
            jump = true;
            //确认跳跃动作（Jump）是初次启动
            jumpInitial = true;
            //初始化跳跃判断计时器
            jumpTimer = jumpTime;
        }//down影响手感有待验证
    }

    //Down和Up状态的移动
    public void MoveGHor(int direction)
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
    void JumpGHor(int direction)
    {
        //如果跳跃刚刚启动，赋予角色初速度
        if (jumpInitial)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed * direction);
            jumpInitial = false;
        }
        //启动后在跳跃判定时间内，按住跳跃键会持续施加刚体力
        else if (jumpTimer > 0 && Input.GetButton("SJump"))
        {
            jumpTimer -= Time.fixedDeltaTime;
            rBody.AddForce(Vector2.up * jumpForce * direction);
        }
        //判定结束后，结束跳跃操作
        else
        {
            jump = false;
        }
    }
    void FixupGDown()
    {
        //水平移动（direction==0无限制时，以下写法影响惯性）
        if (hor == 1)
            MoveGHor(1);
        else if (hor == -1)
            MoveGHor(-1);
        else
            MoveGHor(0);
        //跳跃
        if (jump)
            JumpGHor(1);
    }
    void FixupGUp()
    {
        //水平移动（direction==0无限制时，以下写法影响惯性）
        if (hor == 1)
            MoveGHor(-1);
        else if (hor == -1)
            MoveGHor(1);
        else
            MoveGHor(0);
        //跳跃
        if (jump)
            JumpGHor(-1);
    }

    //Right和Left状态的移动
    void MoveGVer(int direction)
    {
        //更新外界对角色竖直移动速度限制
        hSpeed = platSpeed + environSpeed;
        //如果当前速度没有达到角色当前位置输入方向上的速度限制，加速
        if (direction * rBody.velocity.y < moveSpeed + direction * hSpeed)
            rBody.AddForce(Vector2.up * direction * moveForce);
        //否则，限制移动速度
        if (direction * rBody.velocity.y > moveSpeed + direction * hSpeed || direction == 0)//direction == 0影响惯性
            rBody.velocity = new Vector2(rBody.velocity.x, direction * moveSpeed + hSpeed);
    }
    void JumpGVer(int direction)
    {
        //如果跳跃刚刚启动，赋予角色初速度
        if (jumpInitial)
        {
            rBody.velocity = new Vector2(jumpSpeed * direction, rBody.velocity.y);
            jumpInitial = false;
        }
        //启动后在跳跃判定时间内，按住跳跃键会持续施加刚体力
        else if (jumpTimer > 0 && Input.GetButton("SJump"))
        {
            jumpTimer -= Time.fixedDeltaTime;
            rBody.AddForce(Vector2.right * jumpForce * direction);
        }
        //判定结束后，结束跳跃操作
        else
        {
            jump = false;
        }
    }
    void FixupGRight()
    {
        //水平移动（direction==0无限制时，以下写法影响惯性）
        if (hor == 1)
            MoveGVer(1);
        else if (hor == -1)
            MoveGVer(-1);
        else
            MoveGVer(0);
        //跳跃
        if (jump)
            JumpGVer(-1);
    }
    void FixupGLeft()
    {
        //水平移动（direction==0无限制时，以下写法影响惯性）
        if (hor == 1)
            MoveGVer(-1);
        else if (hor == -1)
            MoveGVer(1);
        else
            MoveGVer(0);
        //跳跃
        if (jump)
            JumpGVer(1);
    }

    // 落地检验
    void GroundCheck()
    {
        grounded = Physics2D.OverlapPoint(groundCheck.position, isGround);
    }
    //转向
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
