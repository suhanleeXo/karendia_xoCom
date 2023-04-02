using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    public GameObject Enemy_A_Folder;
    public GameObject Enemy_B_Folder;
    public GameObject Enemy_C_Folder;
    int num;
    int enemyNum;
    
    public GameObject[] EnemyA_Folder;
    public GameObject[] EnemyB_Folder;
    public GameObject[] EnemyC_Folder;


    GameObject[] targetPool;
    // Start is called before the first frame update
    void Start()
    {
        num=Random.Range(0, 10);
        enemyNum=Random.Range(0,3);
    }
    void Awake()
    {
        EnemyA_Folder=new GameObject[20];
        EnemyB_Folder=new GameObject[20];
        EnemyC_Folder=new GameObject[20];
    }

    public void Generate()
    {
        for(int i=0;i<EnemyA_Folder.Length;i++){
            EnemyA_Folder[i] = Instantiate(Enemy_A_Folder); 
            EnemyA_Folder[i].SetActive(false);
        }
        
        for(int i=0;i<EnemyB_Folder.Length;i++){
            EnemyB_Folder[i] = Instantiate(Enemy_B_Folder);
            EnemyB_Folder[i].SetActive(false);
        }
        
        for(int i=0;i<EnemyC_Folder.Length;i++){
            EnemyC_Folder[i] = Instantiate(Enemy_C_Folder);
            EnemyC_Folder[i].SetActive(false);
        }
    }
    public IEnumerator enemyGenerate(int DelayTime)
    {
        yield return YieldInstructionCache.WaitForSeconds(DelayTime);
        if(num<=3){
            switch (enemyNum){
                case 0 :
                    Instantiate(Enemy_A_Folder,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                    break;
                case 1 :
                    Instantiate(Enemy_B_Folder,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                    break;
                case 2 :
                    Instantiate(Enemy_C_Folder,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                    break;
            }           
        }
        StartCoroutine(enemyGenerate(DelayTime));
    }

    public GameObject MakeObj(string type)
    {
        switch(type){
            case "Enemy_A_Folder" :
                targetPool=EnemyA_Folder;
                break;
            case "Enemy_B_Folder" :
                targetPool=EnemyB_Folder;
                break;
            case "Enemy_C_Folder" :
                targetPool=EnemyC_Folder;
                break;
        }
        for(int i=0;i<targetPool.Length;i++){
            if(!targetPool[i].activeSelf){
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }
}
