using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFolderManager : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;

    public GameObject Enemy_Prefab;
    public GameObject item_Prefab;
    public GameObject Magnet;
    public bool isEnemyDead;
    public bool isGemDestroy;
    public int lifeCycle=0; //적 오브젝트 생명주기
    public int itemNum;    

    // Start is called before the first frame update
    void Start()
    {
        //target=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target=GameManager.instance.player.GetComponent<Transform>();
        itemNum=Random.Range(0,100); //1%확률
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Enemy_Prefab.activeSelf==true){ //적 live
            transform.position=Vector2.MoveTowards(transform.position,target.position,moveSpeed*Time.deltaTime);
        }
        
        if(Enemy_Prefab.activeSelf==false & lifeCycle==0 & itemNum<99){ //경험치 젬 드랍
            lifeCycle=1;
            item_Prefab.SetActive(true);
        }
        if(Enemy_Prefab.activeSelf==false & lifeCycle==0 & itemNum==99){ //자석 드랍
            lifeCycle=1;
            Magnet.SetActive(true);
        }
        if(Enemy_Prefab.activeSelf==false & item_Prefab.activeSelf==false & Magnet.activeSelf==false){ //경험치 획득 시
            lifeCycle=0;
            gameObject.SetActive(false);
            
        }
    }
    void OnEnable()
    {
        lifeCycle=0;
        itemNum=Random.Range(0,100); //1%확률
        Enemy_Prefab.SetActive(true);
        item_Prefab.SetActive(false);
        Magnet.SetActive(false);
        item_Prefab.transform.position=transform.position;
        Enemy_Prefab.transform.position=transform.position;
    }
}
