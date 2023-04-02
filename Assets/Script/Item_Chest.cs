using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Item_Chest : MonoBehaviour
{
    public Transform target;
    public float moveSpeed=10f;
    public bool isTrigger;
    public GameObject chest_UI;
    
    void Start()
    {
        isTrigger=false;
        target=GameManager.instance.player.GetComponent<Transform>();
        chest_UI=GameManager.instance.Chest_UI;
    }
    void Update()
    {
        if(isTrigger==true){
            transform.position=Vector2.MoveTowards(transform.position,target.position,moveSpeed*Time.deltaTime);
            StartCoroutine("DestroyItem");
        }
    }
    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="ItemFound"){
            if(isTrigger==false){
                isTrigger=true;
                GameManager.instance.AudioSourceFolder.SetActive(false);
                GameManager.instance.ItemSound.Play();
                GameManager.instance.playermanager.BGM.Stop();
                GameManager.instance.ChestBGM.Play();
                StartCoroutine("DestroyItem");
            }
        }
    }

    IEnumerator DestroyItem()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.2f); 
        Time.timeScale=0f;
        chest_UI.SetActive(true);
        isTrigger=false;
        Destroy(gameObject);
    }
}
