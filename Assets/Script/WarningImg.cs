using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WarningImg : MonoBehaviour
{
    float fadecount=1f;
    public Image warning;
    void Awake()
    {
        StartCoroutine(fadeinout());
        Color warning_color=this.GetComponent<Image>().color;
        warning.color=new Color(255,255,255,1f);
        Instantiate(GameManager.instance.BossArea,new Vector2(GameManager.instance.player.transform.position.x,GameManager.instance.player.transform.position.y),Quaternion.identity);
        Instantiate(GameManager.instance.BossSpot,new Vector2(GameManager.instance.player.transform.position.x,GameManager.instance.player.transform.position.y),Quaternion.identity);
    }

    // Update is called once per frame
    IEnumerator fadeinout()
    {
        while(fadecount>=0f)
        {
            fadecount-=0.05f;
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            warning.color=new Color(255,255,255,fadecount);
        }
        while(fadecount<1.0f)
        {
            fadecount+=0.05f;
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            warning.color=new Color(255,255,255,fadecount);
        }
        StartCoroutine(fadeinout());
    }
}
