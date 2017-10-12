using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LP_Skill : MonoBehaviour
{
    float lit_sc;
    public float biggest = 3.0f;//光的最大scale
    public float litspeed = 2.0f;
    public float newLightScale = 1.0f;//留下的光的初始scale

    Vector3 lpscale;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lpscale = this.GetComponentInParent<Transform>().localScale;

        LightUp();

        if (Input.GetKeyDown(KeyCode.Q))//测试用，input setting里有LPS2
        {
            Debug.Log("S2");
            SetLight();
        }
    }
    void LightUp()
    {
        this.transform.localScale = new Vector3(lit_sc, lit_sc, lit_sc);
        if (Input.GetKey(KeyCode.Z) && lit_sc > 0f && lit_sc < biggest)//测试用，input setting里有LPS1
        {
            //同时损耗能量
            lit_sc += Time.deltaTime * litspeed;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            lit_sc = 0.1f;//这里稍微有点矛盾但目前没有出Bug
            this.transform.DOScale(new Vector3(0.1f, 0.1f, 1f), 0.2f);
        }
    }
    void SetLight()
    {
        Vector2 lightpos = this.transform.position;
        GameObject newl = Instantiate(this.gameObject, lightpos, Quaternion.identity);
        newl.GetComponent<LP_Skill>().enabled = false;
        newl.AddComponent<Rigidbody2D>();
        //newl.transform.localScale = new Vector2(newLightScale, newLightScale);

        //Vector3 finalsc;
        //finalsc.x = newLightScale * this.transform.localScale.x;
        //finalsc.y = newLightScale * this.transform.localScale.y;

       // newl.transform.DOScale(new Vector3(this.transform.localScale.x * 1.5f, this.transform.localScale.y * 1.5f, 1), 1.5f);
        Destroy(newl, 2.0f);

    }
    void OnColliderEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Light":
                {

                    break;
                }
        }
    }
}
