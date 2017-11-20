using UnityEngine;

public class LPlayer : MonoBehaviour
{
    [HideInInspector]
    public enum Status {normal, combining, combined, shoot, bullet, unctrl};//角色状态分类：通常、合体中、合体、射击、子弹、不可控
    [HideInInspector]
    public Status status;              //角色当前状态

    Rigidbody2D rBody;                 //刚体
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
    /// <summary>
    /// /////////////////////////合体//////////////////////////////////
    /// </summary>
    public float combineSpeed;         //合体速度限制
    public float combineForce;         //合体力
    public float combineDis;           //抓取队友的范围

    bool combineInitial;               //合体初次启动确认
    /// <summary>
    /// /////////////////////////射击//////////////////////////////////
    /// </summary>
    public GameObject aim;             //射击方向标记
    /// <summary>
    /// /////////////////////////子弹//////////////////////////////////
    /// </summary>
    [HideInInspector]
    public Vector2 shootAim;           //射击方向
    [HideInInspector]
    public bool bulletInitial;         //子弹初次启动确认

    public float bulletSpeed;          //子弹状态运动初速度





    // 初始化
    void Start () {
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
            case Status.combining:
                CombiningCtrl();
                break;
            case Status.combined:
                CombinedCtrl();
                break;
            case Status.shoot:
                ShootCtrl();
                break;
            case Status.bullet:
                break;
            case Status.unctrl:
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
            case Status.combining:
                CombiningFixedCtrl();
                break;
            case Status.combined:
                break;
            case Status.shoot:
                break;
            case Status.bullet:
                BulletFixedCtrl();
                break;
            case Status.unctrl:
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
    // 检测并初始化跳跃
    void JumpCheck()
    {
        if (grounded && Input.GetButton("LJump"))
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
    // 合体(使角色进入combined状态)
    public void Combine()
    {
        //切换状态
        status = Status.combined;
        //关闭刚体等
    }
    // 检测并初始化合体
    void CombineCheck()
    {
        if (Input.GetAxis("LCombine") == 1 && sPlayer.GetComponent<SPlayer>().status == SPlayer.Status.normal)//附身队友(消耗能量加在这里）
        {
            status = Status.combining;
            combineInitial = true;
        }
        if (Input.GetAxis("LCombine") == -1 && sPlayer.GetComponent<SPlayer>().status == SPlayer.Status.normal && Vector2.Distance(transform.position, sPlayer.transform.position) < combineDis)//抓取队友
        {
            sPlayer.GetComponent<SPlayer>().Combine();
        }
    }
    // 检测并初始化射击
    void ShootCheck()
    {
        if(Input.GetButton("LShoot") && sPlayer.GetComponent<SPlayer>().status == SPlayer.Status.combined)
        {
            status = Status.shoot;
            Time.timeScale = 0.0f;
            aim.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    // Normal状态
    void NormalCtrl()
    {
        hor = Input.GetAxis("LHorizontal");
        GroundCheck();
        JumpCheck();
        CombineCheck();
        ShootCheck();
    }
    void NormalFixedCtrl()
    {
        //水平移动（direction==0无限制时，以下写法影响惯性）
        if(hor == 1)
            Move(1);
        else if(hor == -1)
            Move(-1);
        else
            Move(0);
        //跳跃
        if(jump)
            Jump();
    }
    // Combining状态
    void CombiningCtrl()
    {
        if (combineInitial)//合体启动时，调整动画等
        {
            combineInitial = false;
        }
        if (Vector2.Distance(rBody.position, sPlayer.transform.position) < 0.1f)//到达目标位置（距离足够小）后进入combined状态
        {
            Combine();
        }
    }
    void CombiningFixedCtrl()
    {
        //跟随目标移动
        rBody.velocity = Vector2.Lerp(rBody.velocity, new Vector2(sPlayer.transform.position.x - transform.position.x,
                                                                  sPlayer.transform.position.y - transform.position.y).normalized * combineSpeed, 1.0f);
    }
    // Shoot状态
    void ShootCtrl()
    {
        hor = Input.GetAxis("LHorizontal");
        ver = Input.GetAxis("LVertical");
        Vector3 direction = new Vector3(hor, ver).normalized;
        float angle = Vector3.Angle(direction, Vector3.right);
        
        float dot = Vector3.Dot(direction, Vector3.up);
        if (dot < 0)
            angle = - angle;
        float atan = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        aim.transform.rotation = Quaternion.Slerp(aim.transform.rotation, Quaternion.Euler(0, 0, atan), 1.0f);
        //Debug.Log(angle);
        //aim.transform.Rotate(0, 0, angle);

        if (Input.GetButtonUp("LShoot"))
        {
            sPlayer.GetComponent<SPlayer>().shootAim = direction;
            sPlayer.GetComponent<SPlayer>().status = SPlayer.Status.bullet;
            sPlayer.GetComponent<SPlayer>().bulletInitial = true;
            aim.GetComponent<SpriteRenderer>().enabled = false;
            Time.timeScale = 1.0f;
            status = Status.normal;
        }
    }
    // Combined状态
    void CombinedCtrl()
    {
        transform.position = sPlayer.transform.position;
    }
    // Bullet状态
    void BulletCtrl()
    {
        if (Input.GetButton("LJump"))
        {
            //确认跳跃状态
            jump = true;
            //确认跳跃动作（Jump）是初次启动
            jumpInitial = true;
            //初始化跳跃判断计时器
            jumpTimer = jumpTime;

            status = Status.normal;
        }
    }
    void BulletFixedCtrl()
    {
        if (bulletInitial)
        {
            rBody.velocity = shootAim * bulletSpeed;
            bulletInitial = false;
        }
        if (jump)
            Jump();
    }
    // 
}