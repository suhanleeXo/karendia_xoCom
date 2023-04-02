using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; //..프리펩들을 보관할 변수
    public List<GameObject> [] pools; // .. 풀 담당을 하는 리스트들
    
    void Awake()
    {
        pools=new List<GameObject>[prefabs.Length];
        
        for(int i=0;i<pools.Length;i++){
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        //선택한 풀의 놀고 있는(비활성화 된) 게임 오브젝트 접근
        foreach (GameObject item in pools[index]){ 
            if(!item.activeSelf){ //발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        //못 찾았으면?
        if(!select){
            select = Instantiate(prefabs[index], transform);// 새롭게 생성하고 select변수에 할당
            pools[index].Add(select);
        }
        return select;
    }
}
