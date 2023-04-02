using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    public PlayerManager manager;
    
    void Awake()
    {
        coll=GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area")){
            return;
        }
        Vector3 playerPos=GameManager.instance.player.transform.position;
        Vector3 myPos=transform.position;
        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;
        
        switch(transform.tag){
            case "Ground":
                // 타일맵 하나의 크기는 20 -> 두 칸 건너뛰어야 하니 40
                if (diffX > diffY) {
                    transform.Translate(dirX * 80, 0, 0);
                } 
                else if (diffX < diffY) {
                    transform.Translate(0, dirY * 80, 0);
                } 
                else {
                    transform.Translate(dirX * 80, dirY * 80, 0);
                }
                break;
            case "NormalEnemy" :
                break;
        }
    }
}
