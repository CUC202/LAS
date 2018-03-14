using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {
    [HideInInspector]
    public enum Status {white, black };//角色状态
    [HideInInspector]
    public Status status;              //角色当前状态

    Rigidbody2D rBody;                 //刚体
    [HideInInspector]
    public bool facingRight = false;            //角色朝向
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
        status = Status.white;
        rBody = GetComponent<Rigidbody2D>();
        platSpeed = 0.0f;
        environSpeed = 0.0f;
    }

    // 每帧检测并更新状态
    void Update () {
        hor = Input.GetAxis("Horizontal");
        //根据移动方向翻转角色朝向
        if (hor > 0 && !facingRight)
            Flip();
        else if (hor < 0 && facingRight)
            Flip();
        GroundCheck();
        JumpCheck();
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            Physics2D.gravity = new Vector2(-Physics2D.gravity.y, Physics2D.gravity.x);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z + 90));
            Debug.Log(Physics2D.gravity.y);
            Debug.Log(Physics2D.gravity.x);
        }
        Debug.Log(rBody.velocity.x);*/

    }
    // 固定间隔根据角色状态更新物理系统
    private void FixedUpdate()
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
    // 检测并初始化跳跃
    void JumpCheck()
    {
        if (grounded && Input.GetButton("Jump"))
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
        else if (jumpTimer > 0 && Input.GetButton("Jump"))
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
    //转向
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
