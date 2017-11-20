using UnityEngine;

public class MovePlatform : MonoBehaviour {
    public float moveSpeed;                //平台移动速度
    public float stopRange;                //平台停止检测范围
    public Transform[] tPoints;            //平台路径点

    Rigidbody2D rBody;                     //平台刚体
    int status;                            //当前目标路径点（0表示返回起点）
    float dis;                             //与当前目标的距离

	// Use this for initialization
	void Start () {
        status = 0;
        rBody = GetComponent<Rigidbody2D>();
        dis = Vector2.Distance(transform.position, tPoints[status].position);
	}
	
	// Update is called once per frame
	void Update () {
        //检测是否到达目标点
        dis = Vector2.Distance(transform.position, tPoints[status].position);
        if(dis < stopRange)//如果是，更新目标点
        {
            status++;
            if (status == tPoints.Length)
                status = 0;
        }
	}

    private void FixedUpdate()
    {
        rBody.velocity = new Vector2(tPoints[status].position.x - transform.position.x, tPoints[status].position.y - transform.position.y).normalized * moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //修正平台上的玩家角色的移动速度(因为平台移动方向会变，不能在enter时更新）
        if (collision.transform.tag == "LPlayer")
        {
            collision.gameObject.GetComponent<LPlayer>().platSpeed = rBody.velocity.x;
            collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x, rBody.velocity.y);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //修正离开平台的玩家角色的移动速度限制
        if (collision.transform.tag == "LPlayer")
        {
            collision.gameObject.GetComponent<LPlayer>().platSpeed = 0;
        }
    }
}
