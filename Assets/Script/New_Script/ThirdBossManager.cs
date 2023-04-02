using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossManager : MonoBehaviour
{
    public int BossType;
    public Transform target;
    public float maxHealth;
    public float curHealth;
    public float damage;
    public GameObject item;
    public GameObject DarkAttackSkillFolder;
    public GameObject DarkBall;
    // Start is called before the first frame update
    void Start()
    {
        target=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(AttackPattern());
    }

    IEnumerator AttackPattern()
    {
        
        yield return YieldInstructionCache.WaitForSeconds(3f);
        DarkAttackSkillFolder.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(2.02f);
        DarkAttackSkillFolder.SetActive(false);
        Instantiate(DarkBall,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        Instantiate(DarkBall,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        Instantiate(DarkBall,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
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
