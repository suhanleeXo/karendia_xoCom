using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBall : MonoBehaviour
{
    public Transform target;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        target=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(DestroyObject());
    }

    void FixedUpdate()
    {
        transform.position=Vector2.MoveTowards(transform.position,target.position,moveSpeed*Time.deltaTime);  
    }
    IEnumerator DestroyObject()
    {
        yield return YieldInstructionCache.WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
