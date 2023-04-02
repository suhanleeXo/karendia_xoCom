using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    float Sec;
    int Min;
    public static string FinalTime;
    [SerializeField]
    Text TimerText;

    void Update()
    {
        
        TimerStart();
    }
    void TimerStart()
    {
        Sec+=Time.deltaTime;
        TimerText.text=string.Format("{0:D2}:{1:D2}",Min,(int)Sec);
        FinalTime=string.Format("{0:D2}:{1:D2}",Min,(int)Sec);
        if((int)Sec>59)
        {
            Sec=0;
            Min++;
        }
    }
}
