using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSkillUp : MonoBehaviour
{
    Rigidbody2D rigid;
    public float WindSpeed;
    void Start()
    {
        rigid=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid.velocity=new Vector2(rigid.velocity.x,WindSpeed);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="DestroyWall"){
            Destroy(gameObject);
        }
    }
}
