using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Combine : MonoBehaviour//还有很多要修补
{
    //public GameObject player1;
    //public GameObject player2;
    //bool isground;
    //Vector2 lpos;
    //Vector2 dpos;
    //// Use this for initialization
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    lpos = player1.transform.position;
    //    dpos = player2.transform.position;
    //    if (Input.GetButton("LightCombine"))
    //    {//由光发起的合并，一起到光的位置 如果是做成对抗向的话就由暗发起（苟命）
    //        Invoke("setInvisibile2", 2.1f);
    //        //player2.GetComponent<PolygonCollider2D>().enabled = false;//具体碰撞盒后根据sprite来
    //        player2.GetComponent<CircleCollider2D>().enabled = false;
    //        player2.GetComponent<Rigidbody2D>().Sleep();
    //        player2.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    //        player2.transform.DOMoveX(lpos.x, 1.0f);
    //        player2.transform.DOMoveY(lpos.y, 1.0f);
    //        player2.transform.DOScale(0.1f, 2.0f);
    //        GetComponent<CameraMove>().enabled = false;

    //    }
    //    if (Input.GetButton("DarkCombine"))
    //    {//由暗发起的合并，一起到暗的位置
    //        Invoke("setInvisibile1", 2.1f);
    //        //player1.GetComponent<PolygonCollider2D>().enabled = false;//
    //        player1.GetComponent<CircleCollider2D>().enabled = false;
    //        //player1.GetComponent<Rigidbody2D> ().enabled = false;
    //        player1.transform.DOMoveX(dpos.x, 1.0f);
    //        player1.transform.DOMoveY(dpos.y, 1.0f);
    //        player1.transform.DOScale(0.1f, 2.0f);
    //        GetComponent<CameraMove>().enabled = false;
    //    }
    //}

    //bool CanCombine()//检测有无其他技能释放
    //{
    //    return false;
    //}
    //void setInvisibile1()
    //{
    //    player1.GetComponent<Renderer>().enabled = false;
    //    player1.GetComponent<CircleCollider2D>().enabled = false;
    //}
    //void setInvisibile2()
    //{
    //    player2.GetComponent<Renderer>().enabled = false;
    //    player2.GetComponent<CircleCollider2D>().enabled = false;//
    //}
    //void init(GameObject player)//还原
    //{
    //    player.transform.DOScale(1.0f, 2.0f);
    //}
}
