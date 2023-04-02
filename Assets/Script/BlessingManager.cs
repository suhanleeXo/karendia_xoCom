using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using PlayFab;
using PlayFab.ClientModels;
public class BlessingManager : MonoBehaviour
{
    //오디오 소스
    public AudioSource Blessing_BGM;
    public AudioSource ClickSound;
    public AudioSource ShootingSound;

    public GameObject AlertUI;
    public GameObject StarlightFolder;
    public GameObject Blessing_Info;
    public GameObject Gatcha_Exception_Folder; //가챠 후 아이템 확인 시 제외될 오브젝트 모음
    public GameObject Gatcha_On_Folder; //가챠 시 보여줄 오브젝트 모음
    public Animator anim;
    bool isInfoTouch;
    public Text KarendiaText;
    public GameObject Effect;
    public Animator EffectAnim;
    public int HowNum=100; //가챠 횟수

    public GameObject LoadingPanel;
    SceneMoveManager scene_move_manager;

    //가챠 On
    public List<string>ItemName=new List<string>();
    public List<Sprite>ItemImage=new List<Sprite>();
    public int TouchNum=0;
    public GameObject Gatcha_List_Scrollview;
    // Start is called before the first frame update
    void Start()
    {
        GetInventory();
        scene_move_manager=GameObject.Find("SceneMoveManager").GetComponent<SceneMoveManager>();
        anim=StarlightFolder.GetComponent<Animator>();
        anim=EffectAnim.GetComponent<Animator>();
        anim.SetInteger("isBlessing_01",3);
    }

    public void GetInventory()
    {
        LoadingPanel.SetActive(true);
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),(result)=>
        {
            KarendiaText.text=(result.VirtualCurrency["KR"]).ToString();
            for(int i=0;i<result.Inventory.Count;i++)
            {
                var Inven = result.Inventory[i];
                Debug.Log(Inven);
            }
        },
        (error)=>Debug.Log("인벤토리 불러오기 실패"));
        LoadingPanel.SetActive(false);
    }
    
    public void SubtractKarendia(int karendia)
    {
        LoadingPanel.SetActive(true);
        var request = new SubtractUserVirtualCurrencyRequest(){VirtualCurrency="KR",Amount=karendia}; 
        PlayFabClientAPI.SubtractUserVirtualCurrency(request,(result)=> KarendiaText.text=(int.Parse(KarendiaText.text)+karendia).ToString(), (error)=>Debug.Log("카렌디아 빼기 실패"));
        LoadingPanel.SetActive(false);
    }

    public void OnClick_Karendia_Gatcha(int num) //1회 뽑기, 10회 뽑기 선택
    {
        ClickSound.Play(); 
        AlertUI.SetActive(true);
        HowNum=num;
    }
    public void OnClick_Karentdia_Gatcha_OK() //가챠 시작
    {
        Gatcha_Exception_Folder.SetActive(false);
        SubtractKarendia(HowNum);
        AlertUI.SetActive(false);
        Blessing_BGM.Stop();
        ShootingSound.Play();
        StartCoroutine("Blessing_01");
        if(HowNum==1){ //1회 가챠 시작
            anim.SetInteger("isBlessing_01",1); //파라미터 1로 변경
        }
        else{ //10회 가챠 시작
            anim.SetInteger("isBlessing_01",2); //파라미터 2로 변경
        }
    }

    IEnumerator Blessing_01() //가챠 애니메이션 시간
    {
        yield return YieldInstructionCache.WaitForSeconds(3.9f); //3.5초 대기
        anim.SetInteger("isBlessing_01",3); //애니메이션idle로 변경
        Gatcha_On_Folder.SetActive(true);
        Blessing_BGM.Play();
        Gatcha_Show();
    }
    public void Gatcha_Show()
    {
        if(TouchNum==HowNum){
            TouchNum=0;
            Gatcha_On_Folder.SetActive(false);
            Gatcha_List_Scrollview.SetActive(true);
        }
        else{
            ClickSound.Play();
            TouchNum++;
            Effect.SetActive(true);
            StartCoroutine(Gatcha_Effect());
        }   
    }
    public void OnClick_Skip_Btn()
    {
        ClickSound.Play();
        TouchNum=0;
        Gatcha_On_Folder.SetActive(false);
        Gatcha_List_Scrollview.SetActive(true);
    }
    IEnumerator Gatcha_Effect()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.9f);
        //EffectAnim.SetBool("isTouch",false);
        Effect.SetActive(false);
    }

    public void OnClick_Gatcha_Back_Btn()
    {
        ClickSound.Play();
        Gatcha_List_Scrollview.SetActive(false);
        Gatcha_Exception_Folder.SetActive(true);
    }

    public void OnClick_AlertUI_False()
    {
        ClickSound.Play();
        AlertUI.SetActive(false);
    }
    
    public void OnClick_Blessing_Info()
    {
        ClickSound.Play();
        if(isInfoTouch==false){
            isInfoTouch=true;
            Blessing_Info.SetActive(true);
        }
        else{
            isInfoTouch=false;
            Blessing_Info.SetActive(false);
        }
    }
    public void OnClick_BackBtn()
    {
        ClickSound.Play();
        StartCoroutine("SceneChange");
    }
    IEnumerator SceneChange()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        scene_move_manager.SceneNum=1;
        SceneManager.LoadScene("LoadingScene");
    }
}
