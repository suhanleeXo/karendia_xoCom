using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossManager : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    public float maxHealth;
    public float curHealth;
    public float damage;
    public GameObject item;
    public Animator anim;
    public Vector3 targetVec;
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();
        anim.SetBool("isAttack",true);
        target=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(AttackPattern());
    }

    void Update()
    {
        if(isActive==true){
            transform.position=Vector2.MoveTowards(transform.position,targetVec,moveSpeed*Time.deltaTime);
        }
    }

    IEnumerator AttackPattern()
    {
        yield return YieldInstructionCache.WaitForSeconds(3f);
        anim.SetBool("isAttack",false);
        targetVec=target.position;
        isActive=true;
        yield return YieldInstructionCache.WaitForSeconds(1f);
        isActive=false;
        anim.SetBool("isAttack",true);
        StartCoroutine(AttackPattern());
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="NormalAttack"){
            NormalAttack normalAttack=other.GetComponent<NormalAttack>();
            curHealth-=normalAttack.damage*Time.deltaTime;
            if(curHealth<=0){
                JoystickManager.killLog+=1;
                Instantiate(item,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                GameManager.instance.BossArea.SetActive(false);
                GameManager.instance.spawner.SetActive(true);
                GameManager.instance.Poolmanager.SetActive(true);
                GameManager.instance.isBossLive=false;
                Destroy(gameObject);          
            }
        }
    }
}
