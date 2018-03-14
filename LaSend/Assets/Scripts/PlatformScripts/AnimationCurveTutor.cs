using UnityEngine;
using System.Collections;

public class AnimationCurveTutor : MonoBehaviour
{
    public AnimationCurve move_anim;
    public AnimationCurve rot_anim;
    public bool is_move;
    public bool is_rotate;
    
    void update()
    {
        if(is_move)
        {
            transform.position = new Vector3(Time.time, move_anim.Evaluate(Time.time), 0);
        }
        if(is_rotate)
        {
            transform.localEulerAngles = new Vector3(0,0,rot_anim.Evaluate(Time.time));
        }
    }
}