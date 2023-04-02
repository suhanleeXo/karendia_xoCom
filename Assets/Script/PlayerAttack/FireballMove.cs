using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballMove : MonoBehaviour
{
    public Transform target;
    public Transform DirectionObject;
    public float moveSpeed=5f;
    public List<int> RandomNum=new List<int>();
    public float RandomX;
    public float RandomY;
    void Start()
    {
        target=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        while(true)
        {
            RandomNum.Add(Random.Range(0,3));
            RandomNum.Add(Random.Range(0,3));
            if(RandomNum[0]==0 & RandomNum[1]==0){
                RandomNum.Clear();
            }
            else{
                break;
            }
        }
        
        switch(RandomNum[0]){
            case 0 :
                RandomX=0f;
                break;
            case 1 :
                RandomX=20f;
                break;
            case 2 :
                RandomX=-20f;
                break;
        }
        switch(RandomNum[1]){
            case 0 :
                RandomY=0f;
                break;
            case 1 :
                RandomY=20f;
                break;
            case 2 :
                RandomY=-20f;
                break;
        }
        float x=target.transform.position.x+RandomX;
        float y=target.transform.position.y+RandomY;
        
        DirectionObject.transform.position=new Vector2(x*5f,y*5f);
        StartCoroutine("DestroyActive");
        Debug.Log(target.transform.position);
        Debug.Log(DirectionObject.transform.position);
    }

    void Update()
    {
        transform.position=Vector2.MoveTowards(transform.position,DirectionObject.transform.position,moveSpeed*Time.deltaTime);
    }

    IEnumerator DestroyActive()
    {
        yield return YieldInstructionCache.WaitForSeconds(5f); 
        Destroy(gameObject);
    }
}
