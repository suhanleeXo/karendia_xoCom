using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JoystickManager : MonoBehaviour
{
    public bl_Joystick js;
    public float speed;
    Animator anim;
    public Text KillText;
    public static int killLog=0;
    Vector3 dir;
    void Start()
    {
        anim=GetComponent<Animator>();
    }

    void Update()
    {
        KillText.text=""+killLog;
        //Vector3 dir = new Vector3(js.Horizontal, js.Vertical,0);
        dir = new Vector3(js.Horizontal, js.Vertical,0);
        if(js.Horizontal>0){
            anim.SetBool("isRun",true);
            transform.localScale=new Vector3(-1f,1f,1f);
        }
        else if(js.Horizontal<0){
            anim.SetBool("isRun",true);
            transform.localScale=new Vector3(1f,1f,1f);
        }
        else{
            anim.SetBool("isRun",false);
        }
        dir.Normalize();
        transform.position+=dir*speed*Time.deltaTime;
    }
}

