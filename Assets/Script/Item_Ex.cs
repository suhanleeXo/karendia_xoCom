using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Ex : MonoBehaviour
{
    public Transform target;
    public float moveSpeed=10f;
    public bool isTrigger;
    public bool isDestroy;
    public int itemExNum;
    
    void Start()
    {
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
    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="ItemFound"){
            if(isTrigger==false){
                isTrigger=true;
            }
        }
        else if(other.tag=="ItemDestroy" & isDestroy==false){
            isDestroy=true;
            GameManager.instance.curEx+=itemExNum;
            GameManager.instance.ItemSound.Play();
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        transform.position=new Vector3(0,0,0);
        isTrigger=false;
        isDestroy=false;
    }
    
}
