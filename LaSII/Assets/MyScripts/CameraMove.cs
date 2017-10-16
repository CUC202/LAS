using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//双人（未合体）模式时的相机移动效果
public class CameraMove : MonoBehaviour
{
    Vector2 campos;
    public GameObject player1;
    public GameObject player2;
    Vector2 p1;
    Vector2 p2;
    float x1, y1;
    float p1w, p1h, p2w, p2h;
    private Plane[] pan;
    // Use this for initialization
    void Start()
    {
        p1w = p1h = p2w = p2h = 0.0f;
    }
    void Update()
    {
        if (player1.GetComponent<SpriteRenderer>() != null && player2.GetComponent<SpriteRenderer>() != null)
        {
            p1w = player1.transform.localScale.x * player1.GetComponent<SpriteRenderer>().sprite.bounds.size.x;//获取player1的宽
            p1h = player1.transform.localScale.y * player1.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            p2w = player2.transform.localScale.x * player2.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            p2h = player2.transform.localScale.y * player2.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        }
        else if (player1.GetComponent<SpriteRenderer>() != null && player2.GetComponent<SpriteRenderer>() == null)
        {
            p1w = p1h = 0;
            p2w = player2.transform.localScale.x * player2.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            p2h = player2.transform.localScale.y * player2.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        }
        else if (player2.GetComponent<SpriteRenderer>() != null && player1.GetComponent<SpriteRenderer>() == null)
        {
            p2w = p2h = 0;
            p1w = player1.transform.localScale.x * player1.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            p1h = player1.transform.localScale.y * player1.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        }
        else
        {
            p1w = p1h = p2w = p2h = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float width;
        float height;
        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        //SetBasicValues ();

        p1 = player1.transform.position;
        p2 = player2.transform.position;
        transform.DOMoveX((p1.x + p2.x) / 2, 0.2f);
        transform.DOMoveY((p1.y + p2.y) / 2, 0.2f);
        x1 = Mathf.Abs(p1.x - p2.x);
        y1 = Mathf.Abs(p1.y - p2.y);

        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(-Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        width = rightBorder - leftBorder;
        height = topBorder - downBorder;


        if ((x1 >= (width - 4 * p1w)) && Camera.main.fieldOfView <= 90)
        {
            Camera.main.fieldOfView += 25 * Time.deltaTime;
            if (Camera.main.fieldOfView >= 88)
            {
                //启用相机边界碰撞
            }
        }
        if (x1 < width / 2 && y1 < height && Camera.main.fieldOfView >= 35)
        {
            Camera.main.fieldOfView -= 25 * Time.deltaTime;
        }
        float a = x1 + 4 * p1w - width;

    }
}
