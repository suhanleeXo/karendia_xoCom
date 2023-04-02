using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_New : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    bool isLive=true;

    Rigidbody2D rigid;
    SpriteRenderer spriterenderer;

    
    void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();
        spriterenderer=GetComponent<SpriteRenderer>();
    }

    
    void FixedUpdate()
    {
        if(!isLive){
            return;
        }
        Vector2 dirVec=target.position-rigid.position;
        Vector2 nextVec=dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity=Vector2.zero;
    }

    void LateUpdate()
    {
        if(!isLive){
            return;
        }
        spriterenderer.flipX=target.position.x<rigid.position.x;
    }
}
