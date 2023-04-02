using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSkillRight : MonoBehaviour
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
        rigid.velocity=new Vector2(WindSpeed,rigid.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag=="DestroyWall"){
            Destroy(gameObject);
        }
    }
}
