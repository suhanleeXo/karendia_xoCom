using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using GooglePlayGames;

using PlayFab;
using PlayFab.ClientModels;


public class StartSceneManager : MonoBehaviour
{
    public AudioClip[] MainSound_Folder;
    public AudioClip[] HalfDragon_Interaction_Folder;
    public AudioSource BGM;
    public AudioSource HalfDragon_Naration;
    public AudioSource ClickSound;
    public int Global_CharacterNum;
    public Image BG;
    public Image portrait;
    public GameObject AlbumFolderUI;
    public GameObject Book;
    public List<Sprite>BgList=new List<Sprite>();
    public List<Sprite>portraitList=new List<Sprite>();
    public Image BlackPanel;
    public Text nametext;
    public Text ProfileNameText;
    public GameObject Setting_UI;
    public GameObject Sound_UI;
    public GameObject Profile_UI;
    public GameObject GameInfo_UI;
    public GameObject Market_UI;
    public int BGM_Num;
    public AudioSource Na01;
    int Naration_Num=0;

    public Text GoldText;
    public Text StaminaText;
    public Text LevelText;
    public Text KarendiaText;
    
    public List<GameObject> Character_Blind=new List<GameObject>();

    //조작안내
    public List <Sprite> GameInfoImageFolder=new List<Sprite>();
    public Image GameInfoImage;
    public Text GameInfoText;
    public List <string> GameInfoTextFolder=new List<string>();
    int gameInfoImageNum=0;
    //상점
    public Sprite Normal_Img; //선택안할 때 이미지
    public Sprite Change_Img; //선택 시 이미지
    public List <Image> Market_Option_Img=new List<Image>();
    public List <GameObject> Market_ScrollView=new List<GameObject>();
    public List <RectTransform> ScrollView=new List<RectTransform>();
    
    //알림 팝업 통일
    public GameObject AlertUI;
    public Text AlertInfoText;
    
    SceneMoveManager scene_move_manager;
    
    public GameObject LoadingPanel;
    public int staminaBuyNum;

    int MoveUINum;//팝업 yes버튼 터치 시 이동[1 : 상점 , ]

    public float curEx;
    public float maxEx;
    public Slider PlayerExbar;

    public void Awake()
    {
        LoadingPanel.SetActive(true);
        scene_move_manager=GameObject.Find("SceneMoveManager").GetComponent<SceneMoveManager>();
        Load();
        //GetInventory();
    }
    public void Load()
    {
        if(PlayerPrefs.HasKey("BGM_Num")){
            BGM_Num=PlayerPrefs.GetInt("BGM_Num");
        }
        else{
            BGM_Num=0;
        }
        BGM.clip=MainSound_Folder[BGM_Num];
        BGM.Play();
        LevelText.text=scene_move_manager.level.ToString();
        nametext.text=scene_move_manager.displayname;
        curEx=scene_move_manager.curEx;
        maxEx=scene_move_manager.maxEx;
        PlayerExbar.value = curEx / maxEx;
        //KarendiaText.text=scene_move_manager.karendia.ToString();
        Debug.Log("플레이어 데이터 불러오기 성공");
        OnGetVirtualCurrency();
    }

    public void OnGetVirtualCurrency() //가상화폐 불러오기
    {
        var request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, GetInventorySuccess, GetInventoryFailure);
    }

    public void GetInventorySuccess(GetUserInventoryResult result)
    {
        Debug.Log("가상화폐 불러오기 성공");
        GoldText.text=result.VirtualCurrency["GD"].ToString();
        StaminaText.text=result.VirtualCurrency["ST"].ToString();
        KarendiaText.text=result.VirtualCurrency["KR"].ToString();
        scene_move_manager.karendia=result.VirtualCurrency["KR"];
        LoadingPanel.SetActive(false);
    }

    public void GetInventoryFailure(PlayFabError error)
    {
        Debug.Log("가상화폐 불러오기 실패");
    }

    public void AddMoney(int gold)
    {
        LoadingPanel.SetActive(true);
        var request = new AddUserVirtualCurrencyRequest(){VirtualCurrency="GD",Amount=gold};
        PlayFabClientAPI.AddUserVirtualCurrency(request,(result)=> GoldText.text=(int.Parse(GoldText.text)+gold).ToString(),(error)=>Debug.Log("돈 추가 실패"));
        LoadingPanel.SetActive(false);
    }

    public void SubtractMoney(int gold)
    {
        LoadingPanel.SetActive(true);
        var request = new SubtractUserVirtualCurrencyRequest(){VirtualCurrency="GD",Amount=gold}; 
        PlayFabClientAPI.SubtractUserVirtualCurrency(request,(result)=> GoldText.text=(int.Parse(GoldText.text)-gold).ToString(), (error)=>Debug.Log("돈 빼기 실패"));
        LoadingPanel.SetActive(false);
    }

    public void AddStamina(int stamina)
    {
        LoadingPanel.SetActive(true);
        var request = new AddUserVirtualCurrencyRequest(){VirtualCurrency="ST",Amount=stamina};
        PlayFabClientAPI.AddUserVirtualCurrency(request,(result)=> StaminaText.text=(int.Parse(StaminaText.text)+stamina).ToString(),(error)=>Debug.Log("스테미나 추가 실패"));
        LoadingPanel.SetActive(false);
    }

    public void SubtractStamina(int stamina)
    {
        LoadingPanel.SetActive(true);
        var request = new SubtractUserVirtualCurrencyRequest(){VirtualCurrency="ST",Amount=stamina}; 
        PlayFabClientAPI.SubtractUserVirtualCurrency(request,(result)=> NextScene(), (error)=>Debug.Log("스테미나 빼기 실패"));
        LoadingPanel.SetActive(false);
    }
    
    public void PurchaseItem(int itemPrice)
    {
        LoadingPanel.SetActive(true);
        var request = new PurchaseItemRequest() {CatalogVersion = "Item", ItemId = "Stamina",VirtualCurrency="GD",Price = itemPrice};
        PlayFabClientAPI.PurchaseItem(request,(result)=>Debug.Log("구매성공"),(error)=>Debug.Log("구매 실패"));
        LoadingPanel.SetActive(false);
    }

    public void ConsumeItem(int ConsumeNum, string itemId)
    {
        LoadingPanel.SetActive(true);
        var request = new ConsumeItemRequest {ConsumeCount=ConsumeNum, ItemInstanceId=itemId};
        PlayFabClientAPI.ConsumeItem(request,(result)=>Debug.Log("사용 성공"),(error)=>Debug.Log("사용 실패"));
        LoadingPanel.SetActive(false);
    }

    public void GetInventory()
    {
        LoadingPanel.SetActive(true);
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),(result)=>
        {
            GoldText.text=(result.VirtualCurrency["GD"]).ToString();
            StaminaText.text=(result.VirtualCurrency["ST"]).ToString();
            for(int i=0;i<result.Inventory.Count;i++)
            {
                var Inven = result.Inventory[i];
                Debug.Log(Inven);
            }
        },
        (error)=>Debug.Log("인벤토리 불러오기 실패"));
        LoadingPanel.SetActive(false);
    }
    
    public void OnClick_BuyItem(int addStaminaNum)
    {
        ClickSound.Play();
        MoveUINum=3;
        staminaBuyNum=addStaminaNum;
        AlertInfoText.text="해당 아이템을 구매하시겠습니까?";
        AlertUI.SetActive(true);
    }

    public void OnClick_AlertNo()
    {
        ClickSound.Play();
        AlertUI.SetActive(false);
    }

    public void OnClick_AlertYes()
    {
        AlertUI.SetActive(false);
        switch(MoveUINum){
            case 1 :
                ClickSound.Play();
                Market_Option_Img[2].sprite=Change_Img;
                Market_ScrollView[2].SetActive(true);
                Market_UI.SetActive(true);
                break;
            case 2 :
                OnClick_Blessing_UI();
                break;
            case 3 : //아이템(스테미나)구매 시
                ClickSound.Play();
                AddStamina(staminaBuyNum);
                break;
        }
    }

    public void OnClick_HalfDragonPlay()
    {
        ClickSound.Play();
        if(int.Parse(StaminaText.text)<20)
        {
            MoveUINum=1;
            AlertInfoText.text="스테미나가 부족합니다.\n상점으로 이동하시겠습니까?";
            AlertUI.SetActive(true);
        }
        else{
            BGM.Stop();
            switch(Global_CharacterNum){
                case 0 :
                    ClickSound.Play();
                    StaminaText.text=(int.Parse(StaminaText.text)-20).ToString();
                    scene_move_manager.SceneNum=2;
                    Na01.Play();
                    SubtractStamina(20);
                    break;
            }
        }
    }
    public void NextScene()
    {
        StartCoroutine(moveLoadingScene());
    }
    IEnumerator moveLoadingScene()
    {
        yield return YieldInstructionCache.WaitForSeconds(2.5f);
        SceneManager.LoadScene("LoadingScene");
    }
    public void OnClick_SwitchCharacter(int CharacterNum)
    {
        if(Global_CharacterNum!=CharacterNum){
            ClickSound.Play();
            StartCoroutine(fadein_out(CharacterNum));
        }
    }
    
    public void OnClick_CharacterBlindUI()
    {
        ClickSound.Play();
        MoveUINum=2;
        AlertInfoText.text="캐릭터가 없습니다.\n카렌디아를 소환해 캐릭터를 뽑으시겠습니까?";
        AlertUI.SetActive(true);
    }
    public void OnClick_Album()
    {
        ClickSound.Play();
        AlbumFolderUI.SetActive(true);
        switch(Global_CharacterNum){
            case 0 :
                
                break;
        }
    }
    public void OnClick_AlbumBack()
    {
        ClickSound.Play();
        AlbumFolderUI.SetActive(false);
    }
    IEnumerator fadein_out(int CharacterNum)
    {
        float fadecount=0;
        while(fadecount<1.0f)
        {
            fadecount+=0.05f;
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            BlackPanel.color=new Color(0,0,0,fadecount);
        }
        
        switch(CharacterNum){
            case 0 :
                Global_CharacterNum=0;
                portrait.sprite=portraitList[0];
                BG.sprite=BgList[0];
                break;
            case 1 :
                Global_CharacterNum=1;
                portrait.sprite=portraitList[1];
                BG.sprite=BgList[1];
                break;
            case 2 :
                Global_CharacterNum=2;
                BG.sprite=BgList[2];
                break;
            case 3 :
                Global_CharacterNum=3;
                BG.sprite=BgList[3];
                break;
        }
        while(fadecount>=0f)
        {
            fadecount-=0.05f;
            yield return YieldInstructionCache.WaitForSeconds(0.01f);
            BlackPanel.color=new Color(0,0,0,fadecount);
        }
    }

    public void OnClick_Setting_UI()
    {
        ClickSound.Play();
        Setting_UI.SetActive(true);
    }

    public void OnClick_Back_Setting_UI()
    {
        ClickSound.Play();
        Setting_UI.SetActive(false);
    }

    public void OnClick_Sound_UI()
    {
        ClickSound.Play();
        Sound_UI.SetActive(true);
    }
    public void OnClick_GameInfo_UI()
    {
        ClickSound.Play();
        GameInfo_UI.SetActive(true);
    }
    public void OnClick_Back_GameInfo_UI()
    {
        ClickSound.Play();
        gameInfoImageNum=0;
        GameInfoImage.sprite=GameInfoImageFolder[gameInfoImageNum];
        GameInfoText.text=GameInfoTextFolder[gameInfoImageNum];
        GameInfo_UI.SetActive(false);
    }
    public void OnClick_ChangeSound(int num)
    {
        ClickSound.Play();
        switch(num){
            case 0 :
                BGM.clip=MainSound_Folder[0];
                BGM.Play();
                PlayerPrefs.SetInt("BGM_Num",0);
                PlayerPrefs.Save();
                break;
            case 1 :
                BGM.clip=MainSound_Folder[1];
                BGM.Play();
                PlayerPrefs.SetInt("BGM_Num",1);
                PlayerPrefs.Save();
                break;
            case 2 :
                BGM.clip=MainSound_Folder[2];
                BGM.Play();
                PlayerPrefs.SetInt("BGM_Num",2);
                PlayerPrefs.Save();
                break;
            case 3 :
                BGM.clip=MainSound_Folder[3];
                BGM.Play();
                PlayerPrefs.SetInt("BGM_Num",3);
                PlayerPrefs.Save();
                break;
        }
    }
    
    public void OnClick_Back_Sound_UI()
    {
        ClickSound.Play();
        Sound_UI.SetActive(false);
    }

    public void OnClick_Profile_UI()
    {
        ClickSound.Play();
        Profile_UI.SetActive(true);
    }
    public void OnClick_Back_Profile_UI()
    {
        ClickSound.Play();
        Profile_UI.SetActive(false);
    }

    public void OnClick_Blessing_UI()
    {
        ClickSound.Play();
        BGM.Stop();
        scene_move_manager.SceneNum=3;
        SceneManager.LoadScene("LoadingScene");
    }
    
    public void OnClick_HalfDragon()
    {
        if(Naration_Num==4){
            HalfDragon_Naration.clip=HalfDragon_Interaction_Folder[Naration_Num];
            HalfDragon_Naration.Play();
            Naration_Num=0;
        }
        else{
            HalfDragon_Naration.clip=HalfDragon_Interaction_Folder[Naration_Num];
            HalfDragon_Naration.Play();
            Naration_Num++;
        }
    }

    public void OnClick_Market()
    {
        ClickSound.Play();
        Market_UI.SetActive(true);
        Market_Option_Img[0].sprite=Change_Img;
        Market_ScrollView[0].SetActive(true);
    }
    public void OnClick_Back_Market()
    {
        ClickSound.Play();
        Market_UI.SetActive(false);
        for(int i=0;i<Market_ScrollView.Count;i++){
            ScrollView[i].anchoredPosition=new Vector3(0,0,0);
            Market_ScrollView[i].SetActive(false);
            Market_Option_Img[i].sprite=Normal_Img;
        }
    }
    public void OnClick_Change_Market_List(int num)
    {
        ClickSound.Play();
        for(int i=0;i<Market_ScrollView.Count;i++){
            ScrollView[i].anchoredPosition=new Vector3(0,0,0);
            if(i!=num){
                Market_ScrollView[i].SetActive(false);
                Market_Option_Img[i].sprite=Normal_Img;
            }
            else{  
                Market_ScrollView[num].SetActive(true);
                Market_Option_Img[num].sprite=Change_Img;
            }
        } 
    }

    public void OnClick_RightBtn()
    {
        ClickSound.Play();
        if(gameInfoImageNum==10){
            gameInfoImageNum=0;
        }
        else{
            gameInfoImageNum++;
        }
        GameInfoImage.sprite=GameInfoImageFolder[gameInfoImageNum];
        GameInfoText.text=GameInfoTextFolder[gameInfoImageNum];
    }
    public void OnClick_LeftBtn()
    {
        ClickSound.Play();
        if(gameInfoImageNum==0){
            gameInfoImageNum=10;
        }
        else{
            gameInfoImageNum--;
        }
        GameInfoImage.sprite=GameInfoImageFolder[gameInfoImageNum];
        GameInfoText.text=GameInfoTextFolder[gameInfoImageNum];
    }
    
}
