using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy_ScriptManager : MonoBehaviour
{
    public Transform target;
    public float maxHealth;
    public float curHealth;
    public float damage;
    SpriteRenderer spriterenderer;
    
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        spriterenderer=GetComponent<SpriteRenderer>();
        target=GameManager.instance.player.GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        if((transform.position.x-target.position.x)>=0){
            anim.SetBool("isLeft",true);
        }
        else{
            anim.SetBool("isLeft",false);
        }
    }
    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="NormalAttack"){
            NormalAttack normalAttack=other.GetComponent<NormalAttack>();
            curHealth-=normalAttack.damage*Time.deltaTime;
            if(curHealth<=0){
                JoystickManager.killLog+=1;
                gameObject.SetActive(false);            
            }
        }
    }
}
