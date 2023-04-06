using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DailyRewardManager : MonoBehaviour
{
    public int rewardCount=0;
    public StartSceneManager manager;
    public string[] rewards={"Gold","Stamina","Karendia"};
    public bool hasRewarded;
    public DateTime nextRewardTime;
    public GameObject RewardUI;
    public List <GameObject>stampUI=new List<GameObject>();
    public void Start()
    {
        Load();
    }
    public void Load()
    {
        if(PlayerPrefs.HasKey("nextRewardtime")){
            string savedDateTimeString = PlayerPrefs.GetString("nextRewardtime");
            DateTime nextRewardTime = DateTime.ParseExact(savedDateTimeString, "yyyy-MM-dd HH:mm:ss", null);
        }
        else{
            nextRewardTime=DateTime.UtcNow.AddHours(24);
            string dateTimeString = nextRewardTime.ToString("yyyy-MM-dd HH:mm:ss");
            PlayerPrefs.SetString("nextRewardtime", dateTimeString);
            PlayerPrefs.Save();
        }
        rewardCount=PlayerPrefs.GetInt("rewardCount");
        if(PlayerPrefs.GetInt("hasRewarded")==1){
            hasRewarded=true;
        }
        else{
            hasRewarded=false;
        }
        
        Reward();
        
    }
    public void UpdateNextRewardTime()
    {
        nextRewardTime=DateTime.UtcNow.AddHours(24);
        string dateTimeString = nextRewardTime.ToString("yyyy-MM-dd HH:mm:ss");
        PlayerPrefs.SetString("nextRewardtime", dateTimeString);
        PlayerPrefs.Save();
    }

    // 보상을 받을 수 있는 상태인지 확인
    public bool CanReward()
    {
        return rewardCount < rewards.Length && !hasRewarded && DateTime.UtcNow >= nextRewardTime;
    }

    // 보상을 받기
    public void Reward()
    {
        if (CanReward())
        {
            RewardUI.SetActive(true);
            // 보상을 지급하고, 보상을 받았는지 여부와 보상 횟수를 갱신합니다.
            Debug.Log("보상 : " + rewards[rewardCount]);
            hasRewarded = true;
            rewardCount++;
            PlayerPrefs.SetInt("rewardCount", rewardCount);
            PlayerPrefs.SetInt("hasRewarded", 1);
            PlayerPrefs.Save();
            
            // 보상을 받았으므로, 보상을 받을 수 있는 시간 조건을 갱신합니다.
            UpdateNextRewardTime();
            for(int i=0;i<rewardCount+1;i++)
            {
                stampUI[i].SetActive(true);
            }
        }
        else
        {
            Debug.Log("보상을 받을 수 없습니다.");
            for(int i=0;i<rewardCount+1;i++)
            {
                stampUI[i].SetActive(true);
            }
        }
    }
    public void OnClick_RewardUI_Btn()
    {
        manager.ClickSound.Play();
        RewardUI.SetActive(true);
    }
    public void OnClick_RewardUI_Close()
    {
        manager.ClickSound.Play();
        RewardUI.SetActive(false);
    }
}