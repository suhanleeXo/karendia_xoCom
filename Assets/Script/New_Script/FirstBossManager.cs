using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossManager : MonoBehaviour
{
    public Transform target;
    public float moveSpeed=1f;
    public float maxHealth;
    public float curHealth;
    public bool isEnter;
    public bool isDead;
    public float damage;
    public GameObject item;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        isEnter=false;
        target=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim=GetComponent<Animator>();
        StartCoroutine(BossLogic());
    }
    void Update()
    {
        transform.position=Vector2.MoveTowards(transform.position,target.position,moveSpeed*Time.deltaTime);
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
            curHealth-=normalAttack.damage;
            if(curHealth<=0){
                JoystickManager.killLog+=1;
                Instantiate(item,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                Destroy(gameObject);            
            }
        }
    }
    IEnumerator BossLogic()
    {
        moveSpeed=1f;
        anim.SetBool("isAttack",false);
        yield return YieldInstructionCache.WaitForSeconds(4f);
        moveSpeed=0f;
        anim.SetBool("isAttack",true);
        yield return YieldInstructionCache.WaitForSeconds(2f);
        StartCoroutine(BossLogic());
    }
}
