using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    

    float Timer;
    public float sec=1f;
    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer>sec){
            Timer=0;
            Spawn();
        }     
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(Random.Range(0,3));
        enemy.transform.position=spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}


