using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSkillFire : MonoBehaviour
{
    public GameObject A;  //A라는 GameObject변수 선언
    public JoystickManager manager;
    public GameObject FireRight;
    public GameObject FireLeft;
    Transform AT;
    void Start ()
    {
        AT=A.transform;
    }
    void Update()
    {
        if(manager.js.Horizontal>=0){
            FireRight.SetActive(true);
            FireLeft.SetActive(false);
        }
        else{
            FireLeft.SetActive(true);
            FireRight.SetActive(false);
        }
    }
    void LateUpdate () 
    {
        transform.position = new Vector3 (AT.position.x,AT.position.y,transform.position.z);
    }
}
