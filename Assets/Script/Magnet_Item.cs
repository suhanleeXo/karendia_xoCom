using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet_Item : MonoBehaviour
{
    public Transform target;
    public float moveSpeed=10f;
    public bool isTrigger;
    public bool isDestroy;
    void Start()
    {
        Debug.Log("자석 생성");
        isTrigger=false;
        target=GameManager.instance.player.GetComponent<Transform>();
    }
    void Update()
    {
        if(isTrigger==true){
            transform.position=Vector2.MoveTowards(transform.position,target.position,moveSpeed*Time.deltaTime);
            //StartCoroutine("DestroyItem");
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="ItemFound"){
            if(isTrigger==false){
                isTrigger=true;
            }
        }
        else if(other.tag=="ItemDestroy" & isDestroy==false){
            isDestroy=true;
            GameManager.instance.ItemFound.transform.localScale = new Vector3(100, 100, 1);
            StartCoroutine("DestroyItem");
        }
    }

    IEnumerator DestroyItem()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.2f); 
        GameManager.instance.ItemFound.transform.localScale = new Vector3(GameManager.instance.skillCount[8]+1, GameManager.instance.skillCount[8]+1, 1);
        isTrigger=false;
        isDestroy=false;
        gameObject.SetActive(false);
    }
}
