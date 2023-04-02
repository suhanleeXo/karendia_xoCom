using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyManager : MonoBehaviour
{
    public Transform target;
    public float moveSpeed=1f;
    public float maxHealth;
    public float curHealth;

    public bool isDead;
    public float damage;
    public GameObject item;

    SpriteRenderer spriterenderer;
    int num;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        num=Random.Range(0, 10);
        target=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim=GetComponent<Animator>();
        spriterenderer=GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        transform.position=Vector2.MoveTowards(transform.position,target.position,moveSpeed*Time.deltaTime);
        if((transform.position.x-target.position.x)>=0){
            spriterenderer.flipX=true;
        }
        else{
            spriterenderer.flipX=false;
        }
    }
    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="NormalAttack"){
            NormalAttack normalAttack=other.GetComponent<NormalAttack>();
            curHealth-=normalAttack.damage;
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        if(curHealth<=0f & isDead==false){
            damage=0;
            isDead=true;
            StartCoroutine("FadeOut");
            JoystickManager.killLog+=1;
        }
    }
    IEnumerator FadeOut()
    {
        for(int i=10;i>=0;i--)
        {
            float f=i/10f;
            Color c=spriterenderer.material.color;
            c.a=f;
            spriterenderer.material.color=c;
            yield return YieldInstructionCache.WaitForSeconds(0.02f);
        }
        if(num>=5){
                Instantiate(item,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
            }
            gameObject.SetActive(false);
    }
}
