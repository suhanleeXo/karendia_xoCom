using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject Poolmanager;
    public PoolManager pool; //풀 매니저
    public EnemyCreate objectManager;
    public GameObject spawner;
    public Spawner spawnScript;
    public PlayerManager playermanager; //PlayerManager
    public JoystickManager joystickmanager; //JoystickManager
    public GameObject player; //플레이어 오브젝트
    public GameObject StoryPanel; //터치하면 스토리 진행되는 패널
    public GameObject Portrait; //하프드래곤 초상화 오브젝트 선언
    public GameObject TalkPanel; //대화패널 오브젝트
    public GameObject MenuUI; //메뉴 UI
    public GameObject OtherMenuUI; //
    public GameObject Title; //게임 시작 타이틀
    public GameObject Timer;
    
    public Text TalkText; //스토리 대사 텍스트
    public Text Menu_Info_Text;
    public AudioSource ChestBGM;
    public AudioSource ClickSound; //터치 사운드
    public GameObject AudioSourceFolder;
    public AudioSource NormalAttack_Sound; //기본공격 사운드
    public AudioSource levelUpSound; //레벨업 사운드
    public AudioSource ItemSound; // 아이템 획득 사운드
    public AudioSource WindSkillSound; //날개짓 사운드
    public AudioSource ThunderSkillSound; //드래곤의 포효 사운드
    public AudioSource DragonBreathSound; //용의 숨결 사운드
    public AudioSource WarningSound; //보스출현사운드
    public AudioSource ItemGetSound;
    public float curEx=0;
    public float maxEx=20;
    public Slider Exbar;
    public int StoryNum; //스토리 진행 숫자
    public int Level=1;
    public int FireballRotationNum=60;
    public float ThunderDelayTime;
    public float DragonBreathDelayTime=8f;
    public Text LevelText;
    public GameObject LevelUpUI;
    public Sprite[] skillset=new Sprite[10]; //스킬 이미지 세트
    public int windSkillTime=4; //바람스킬 시간 값
    
    
    //지금까지 사용한 데이터 리스트
    public GameObject[] Skill_Panel=new GameObject[9]; //스킬 클릭 금지 패널 세트
    public List<int> skillCount=new List<int>{0,0,0,0,0,0,0,0,0}; //스킬 레벨 리스트
    public Sprite[] skillLevel=new Sprite[5]; //스킬레벨 이미지 세트 
    public List<Image> SkillLevel_UI=new List<Image>(); //스킬레벨 표시 UI;
    
    //스킬 오브젝트
    public NormalAttack magicholeAttack; //드래곤의 영역 스크립트
    public NormalAttack normalattack_script; //기본 공격 스크립트
    public NormalAttack DragonBreath_script; //용의 숨결 스크립트
    public List<NormalAttack> fireball_script=new List<NormalAttack>(); //화염구 스크립트 리스트
    public List<NormalAttack> Windskill_script=new List<NormalAttack>(); //드래곤폭풍 공격력 스크립트 리스트
    public List<NormalAttack> Thunder_script=new List<NormalAttack>(); //드래곤 포효 공격력 스크립트 리스트
    
    public GameObject NormalAttack; //기본공격 오브젝트
    public GameObject DragonBreath; //용의 숨결 오브젝트
    public GameObject MagicHole; //드래곤의 영역 오브젝트
    public GameObject FireBallFolder; //화염구 오브젝트 폴더
    public List<GameObject> Fireball=new List<GameObject>(); //화염구 오브젝트 리스트
    public List<GameObject> WindAttack=new List<GameObject>();
    public GameObject ThunderFolder; //드래곤포효 스킬폴더
    
    public List<GameObject> Thunder=new List<GameObject>(); //드래곤 포효 스킬 요소 리스트
    
    public GameObject ItemFound; //인력 오브젝트
    public GameObject ItemDestroy;
    List<int> SList=new List<int>();

    //타이머
    int Sec;
    int Min;
    public static string FinalTime;
    public Text TimerText;

    //보스
    public GameObject WarningImg;
    
    public List<GameObject> Boss=new List<GameObject>();
    
    public GameObject BossArea;
    public GameObject BossSpot;
    int BossNum=0;

    SceneMoveManager scene_move_manager;

    //레벨업 스킬
    public List<Sprite> Level_UI=new List<Sprite>();//스킬 레벨 이미지(5)
    public List<string> skillInfo=new List<string>();//스킬 정보(50)

    public List<Image> Level_UI_Img=new List<Image>();
    public List<Text> Skill_Info_Text=new List<Text>();

    //보물상자
    public GameObject Chest_UI;
    public GameObject Chest;
    public Animator ChestAnim;
    public GameObject ItemImg_UI;
    public GameObject Open_Btn;
    public GameObject Ok_Btn;
    public Image itemImg;
    public Text itemText;
    public List<Sprite>itemChangeImg=new List<Sprite>();
    public List<string>itemChangeText=new List<String>();

    public bool isBossLive;
    void Awake()
    {
        try{
            scene_move_manager=GameObject.Find("SceneMoveManager").GetComponent<SceneMoveManager>();
        }
        catch(NullReferenceException ie)
        {
            Debug.Log("DontDestroyOnLoad Is Null");
        }
        instance=this;
    }
    void Start() 
    {
        LevelUp_System();
        ThunderDelayTime=8f; //드래곤의 포효 스킬 쿨타임
        StartCoroutine("StoryActive"); //시작하면 StoryActive 실행
        for(int i=0;i<skillCount.Count-1;i++)
        {
            Skill_Info_Text[i].text=skillInfo[i*5];
        }
        ChestAnim=Chest.GetComponent<Animator>();
    }

    void LevelUp_System()
    {
        //랜덤값 3개 뽑기
        
        while(true)
        {
            int k=UnityEngine.Random.Range(0, 9);
            if(SList.Count==0){
                SList.Add(k);
                continue;
            }
            else{
                if(SList.Contains(k)){
                    continue;
                }
                else{
                   SList.Add(k); 
                   if(SList.Count==3){
                    break;
                   }
                }
            }
        }
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(false);
        }
        Debug.Log(SList[0] + " " + SList[1] + " " + SList[2]);
    }
    
    void Update()
    {
        Exbar.value = curEx / maxEx;
        FireBallFolder.transform.Rotate(new Vector3 (0, 0, FireballRotationNum) * Time.deltaTime);
        if(Exbar.value>=1)
        {
            maxEx+=maxEx*0.5f;
            curEx=0;
            StartCoroutine("LevelUp");
        }
    }
    
    public void StoryStart()
    {
        switch (StoryNum){
            case 0 :
                TalkPanel.SetActive(true);
                TalkText.text="스토리 대사 1";
                break;
            case 1 :
                TalkText.text="스토리 대사 2";
                break;
            case 2 :
                TalkText.text="스토리 대사 3";
                break;
            case 3 :
                TalkText.text="스토리 대사 4";
                break;
            case 4 :
                Destroy(StoryPanel); // 스토리 패널 오브젝트 파괴
                TalkPanel.SetActive(false); //대화 패널 비활성화
                Portrait.SetActive(false); //하프드래곤 초상화 비활성화
                Timer.SetActive(true); //타이머 활성화
                StartCoroutine(TimerFunc()); //타이머 시작
                StartCoroutine(Normal_Attack(2f));
                StartCoroutine(playermanager.FinalSkillTime());
                spawner.SetActive(true);
                break;
        }
        StoryNum++;
    }

    IEnumerator TimerFunc()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        if(isBossLive==false)
        {
            Sec+=1;
            if((int)Sec>59)
            {
                Sec=0;
                Min++;
            }
            TimerText.text=string.Format("{0:D2}:{1:D2}",Min,Sec);
        }
        if((Min==0 & Sec==55) | (Min==1 & Sec==55) | (Min==2 & Sec==55)){
            WarningImg.SetActive(true);
            WarningSound.Play();
            spawner.SetActive(false);
            Poolmanager.SetActive(false);
            BossArea.transform.position=player.transform.position;
            BossSpot.transform.position=player.transform.position;
            BossArea.SetActive(true);
            BossSpot.SetActive(true);
        }
        if(((Min==1 & Sec==0) | (Min==2 & Sec==0) | (Min==3 & Sec==0)) & isBossLive==false){
            isBossLive=true;
            WarningImg.SetActive(false);
            BossSpawn(Min);
        }
        StartCoroutine(TimerFunc());
    }
    public void BossSpawn(int Min) //5분,10분,15분 보스 스폰 함수
    {
        BossSpot.SetActive(false);
        switch(Min){
            case 1 :
                Instantiate(Boss[0],new Vector2(BossSpot.transform.position.x,BossSpot.transform.position.y),Quaternion.identity);
                break;
            case 2 :
                Instantiate(Boss[1],new Vector2(BossSpot.transform.position.x,BossSpot.transform.position.y),Quaternion.identity);
                break;
            case 3 :
                Instantiate(Boss[2],new Vector2(BossSpot.transform.position.x,BossSpot.transform.position.y),Quaternion.identity);
                break;
        }
    }
    IEnumerator LevelUp() //레벨업 
    {
        yield return YieldInstructionCache.WaitForSeconds(0.3f);
        AudioSourceFolder.SetActive(false);
        Time.timeScale=0f;
        LevelUpUI.SetActive(true);
        levelUpSound.Play();
        Level++;
        LevelText.text="LV."+Level;
        if(spawnScript.sec>0.15f){
            spawnScript.sec-=0.1f;
        } 
    }
    IEnumerator StoryActive() 
    {
        yield return YieldInstructionCache.WaitForSeconds(2f); //Title의 Animation효과가 끝날 때 까지 시간 지연
        Portrait.SetActive(true); //하프드래곤 초상화 활성화
        StoryPanel.SetActive(true); //터치하면 스토리 진행되는 패널 활성화
        Destroy(Title);
    }
    public void OnClick_MenuUI()
    {
        ClickSound.Play();
        MenuUI.SetActive(true);
        StartCoroutine(TimeStop(0.6f));
    }
    IEnumerator TimeStop(float time)
    {
        yield return YieldInstructionCache.WaitForSeconds(time);
        OtherMenuUI.SetActive(true);
        Menu_Info_Text.text="레벨 : "+LevelText.text+"\n\n플레이 시간 : "+TimerText.text+"\n\n처치 수 : "+playermanager.KillText.text;
        AudioSourceFolder.SetActive(false);
        Time.timeScale=0;
    }
    IEnumerator Normal_Attack(float delayTime) //기본 공격 딜레이 함수
    {
        yield return YieldInstructionCache.WaitForSeconds(delayTime);
        NormalAttack.SetActive(true);
        NormalAttack_Sound.Play();
        yield return YieldInstructionCache.WaitForSeconds(delayTime);
        NormalAttack.SetActive(false);
        NormalAttack_Sound.Stop();
        StartCoroutine(Normal_Attack(delayTime));
    }
    
    
    
    
    public void OnClick_ReturnGameUI()
    {
        AudioSourceFolder.SetActive(true);
        MenuUI.SetActive(false);
        OtherMenuUI.SetActive(false);
        Time.timeScale=1;
    }
    public void OnClick_GameOver()
    {
        for(int i=0;i<Windskill_script.Count;i++) //날개짓 데미지 초기화
        {
            Windskill_script[i].damage+=4f;
        }
        for(int i=0;i<WindAttack.Count;i++) //날개짓 오브젝트 크기 초기화
        {
            WindAttack[i].transform.localScale = new Vector3(1, 1, 1);
        }
        DragonBreath.transform.localScale = new Vector3(1, 1, 1); //용의 숨결 오브젝트 크기 초기화
        DragonBreath_script.damage=8f; //용의 숨결 데미지 초기화
        JoystickManager.killLog=0;
        ClickSound.Play();
        Time.timeScale=1f;
        scene_move_manager.SceneNum=1;
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnClick_DragonStorm() //드래곤폭풍 카드 클릭 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        switch(skillCount[3]){
            case 0 :
                StartCoroutine("WindSkillTime");
                skillCount[3]++;
                break;
            case 1 :
                skillCount[3]++;
                break;
            case 2 :
                skillCount[3]++;
                break;
            case 3 :
                skillCount[3]++;
                break;
            case 4 :
                skillCount[3]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[3].text=skillInfo[15+skillCount[3]];
        Level_UI_Img[3].sprite=Level_UI[skillCount[3]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }

    IEnumerator WindSkillTime() //날개짓 딜레이 함수
    {
        yield return YieldInstructionCache.WaitForSeconds(windSkillTime);
        WindSkillSound.Play();
        switch(skillCount[3]){
            case 1 :
                Instantiate(WindAttack[0],new Vector2(player.transform.position.x+5,player.transform.position.y),Quaternion.identity);
                break;
            case 2 :
                Instantiate(WindAttack[0],new Vector2(player.transform.position.x+5,player.transform.position.y),Quaternion.identity);
                Instantiate(WindAttack[1],new Vector2(player.transform.position.x-5,player.transform.position.y),Quaternion.identity);
                break;
            case 3 :
                Instantiate(WindAttack[0],new Vector2(player.transform.position.x+5,player.transform.position.y),Quaternion.identity);
                Instantiate(WindAttack[1],new Vector2(player.transform.position.x-5,player.transform.position.y),Quaternion.identity);
                Instantiate(WindAttack[2],new Vector2(player.transform.position.x,player.transform.position.y+5),Quaternion.identity);
                break;
            case 4 :
                Instantiate(WindAttack[0],new Vector2(player.transform.position.x+5,player.transform.position.y),Quaternion.identity);
                Instantiate(WindAttack[1],new Vector2(player.transform.position.x-5,player.transform.position.y),Quaternion.identity);
                Instantiate(WindAttack[2],new Vector2(player.transform.position.x,player.transform.position.y+5),Quaternion.identity);
                Instantiate(WindAttack[3],new Vector2(player.transform.position.x,player.transform.position.y-5),Quaternion.identity);
                break;
            case 5 :
                Instantiate(WindAttack[0],new Vector2(player.transform.position.x+5,player.transform.position.y),Quaternion.identity);
                Instantiate(WindAttack[1],new Vector2(player.transform.position.x-5,player.transform.position.y),Quaternion.identity);
                Instantiate(WindAttack[2],new Vector2(player.transform.position.x,player.transform.position.y+5),Quaternion.identity);
                Instantiate(WindAttack[3],new Vector2(player.transform.position.x,player.transform.position.y-5),Quaternion.identity);
                windSkillTime=2; //날개짓 스킬 쿨 2초로 변경
                break;
        }
        StartCoroutine("WindSkillTime");
    }

    public void OnClick_DragonBreath() //용의 숨결 카드 클릭 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        switch(skillCount[0]){
            case 0 :
                StartCoroutine("DragonBreathTime");
                skillCount[0]++;
                break;
            case 1 :
                skillCount[0]++;
                break;
            case 2 :
                skillCount[0]++;
                break;
            case 3 :
                skillCount[0]++;
                break;
            case 4 :
                skillCount[0]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[0].text=skillInfo[skillCount[0]];
        Level_UI_Img[0].sprite=Level_UI[skillCount[0]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }

    IEnumerator DragonBreathTime() //용의 숨결 딜레이 함수
    {
        for(int i=0;i<skillCount[0];i++)
        {
            yield return YieldInstructionCache.WaitForSeconds(1f);
            Instantiate(DragonBreath,new Vector2(player.transform.position.x,player.transform.position.y),Quaternion.identity);
            DragonBreathSound.Play();
        }
        yield return YieldInstructionCache.WaitForSeconds(DragonBreathDelayTime);
        StartCoroutine("DragonBreathTime");
    }


    public void OnClick_FireBall() //화염구 소환 카드 선택 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        switch(skillCount[4]){
            case 0 : 
                Fireball[0].SetActive(true);
                StartCoroutine("Fireball_Time"); //화염구 딜레이 코루틴 함수
                skillCount[4]++;
                break; 
            case 1 :
                Fireball[1].SetActive(true);
                skillCount[4]++;
                break;
            case 2 :
                Fireball[2].SetActive(true);
                skillCount[4]++;
                break;
            case 3 :
                Fireball[3].SetActive(true);
                skillCount[4]++;
                break;
            case 4 :
                FireballRotationNum=240;
                skillCount[4]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[4].text=skillInfo[20+skillCount[4]];
        Level_UI_Img[4].sprite=Level_UI[skillCount[4]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }

    IEnumerator Fireball_Time() //화염구 딜레이 함수
    {
        FireBallFolder.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(5);
        FireBallFolder.SetActive(false);
        yield return YieldInstructionCache.WaitForSeconds(4);
        StartCoroutine("Fireball_Time");
    }

    public void OnClick_DragonArea() //드래곤의 영역 카드 클릭 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        switch(skillCount[1]){
            case 0 :
                MagicHole.SetActive(true);
                skillCount[1]++;
                break;
            case 1 :
                MagicHole.transform.localScale = new Vector3(2, 2, 1); //드래곤 영역 크기 두배 증가
                skillCount[1]++;
                break;
            case 2 :
                magicholeAttack.damage+=0.2f; //드래곤 영역 데미지 20% 
                skillCount[1]++;
                break;
            case 3 :
                MagicHole.transform.localScale = new Vector3(3, 3, 1); //드래곤 영역 크기 세배 증가
                skillCount[1]++;
                break;
            case 4 :
                magicholeAttack.damage+=0.2f; //드래곤 영역 데미지 20% 증가
                skillCount[1]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[1].text=skillInfo[5+skillCount[1]];
        Level_UI_Img[1].sprite=Level_UI[skillCount[1]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }

    public void OnClick_Recovery() //리커버리 카드 클릭 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        switch(skillCount[6]){
            case 0 :
                skillCount[6]++;
                StartCoroutine("Recovery");
                break;
            case 1 : 
                skillCount[6]++;
                break;
            case 2 :
                skillCount[6]++;
                break;
            case 3 :
                skillCount[6]++;
                break;
            case 4 :
                skillCount[6]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[6].text=skillInfo[30+skillCount[6]];
        Level_UI_Img[6].sprite=Level_UI[skillCount[6]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }
    IEnumerator Recovery() //리커버리 스킬 코루틴 함수
    {
        yield return YieldInstructionCache.WaitForSeconds(10f);
        float recoveryHealth=playermanager.maxHealth*0.01f*skillCount[6];
        if(playermanager.curHealth + recoveryHealth<=playermanager.maxHealth){
            playermanager.curHealth+=recoveryHealth;
        }
        StartCoroutine("Recovery");
    }
    public void OnClick_WingUpgrade() //날개강화 카드 클릭 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        switch(skillCount[5]){
            case 0 :
                joystickmanager.speed+=joystickmanager.speed*0.2f;
                skillCount[5]++;
                break;
            case 1 : 
                joystickmanager.speed+=joystickmanager.speed*0.2f;
                skillCount[5]++;
                break;
            case 2 :
                joystickmanager.speed+=joystickmanager.speed*0.2f;
                skillCount[5]++;
                break;
            case 3 :
                joystickmanager.speed+=joystickmanager.speed*0.2f;
                skillCount[5]++;
                break;
            case 4 :
                joystickmanager.speed+=joystickmanager.speed*0.2f;
                skillCount[5]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[5].text=skillInfo[25+skillCount[5]];
        Level_UI_Img[5].sprite=Level_UI[skillCount[5]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }

    public void OnClick_DragonThunder() //드래곤 포효 카드 클릭 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        switch(skillCount[2]){
            case 0 : 
                Thunder[0].SetActive(true);
                StartCoroutine("Thunder_Time");
                skillCount[2]++;
                break;
            case 1 : 
                Thunder[1].SetActive(true);
                skillCount[2]++;
                break;
            case 2 :
                Thunder[2].SetActive(true);
                skillCount[2]++;
                break;
            case 3 :
                Thunder[3].SetActive(true);
                skillCount[2]++;
                break;
            case 4 :
                ThunderDelayTime=2f;
                skillCount[2]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[2].text=skillInfo[10+skillCount[2]];
        Level_UI_Img[2].sprite=Level_UI[skillCount[2]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }
    
    IEnumerator Thunder_Time() //드래곤포효 스킬 코루틴 함수
    {
        ThunderFolder.SetActive(true);
        ThunderSkillSound.Play();
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        ThunderFolder.SetActive(false);
        yield return YieldInstructionCache.WaitForSeconds(ThunderDelayTime);
        StartCoroutine("Thunder_Time");
    }
    
    public void OnClick_DragonFate() //용의 숙명 카드 클릭 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        normalattack_script.damage*=2; //일반공격 데미지 2배 증가
        magicholeAttack.damage*=2; //드래곤의 영역 데미지 2배 증가
        DragonBreath_script.damage*=2; //용의 숨결 데미지 2배 증가
        for(int i=0;i<fireball_script.Count;i++){ //화염구 데미지 2배 증가
                fireball_script[i].damage*=2; 
        }
        for(int i=0;i<Windskill_script.Count;i++){ //드래곤폭풍 데미지 2배 증가
            Windskill_script[i].damage*=2; 
        }
        for(int i=0;i<Thunder_script.Count;i++){ //드래곤포효 데미지 2배 증가
            Thunder_script[i].damage*=2; 
        }
        switch(skillCount[7]){
            case 0 :
                skillCount[7]++;
                break;
            case 1 : 
                skillCount[7]++;
                break;
            case 2 :
                skillCount[7]++;
                break;
            case 3 :
                skillCount[7]++;
                break;
            case 4 :
                skillCount[7]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[7].text=skillInfo[35+skillCount[7]];
        Level_UI_Img[7].sprite=Level_UI[skillCount[7]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }

    public void OnClick_Magnetic() //인력 카드 클릭 시
    {
        AudioSourceFolder.SetActive(true);
        ClickSound.Play();
        for(int i=0;i<SList.Count;i++){
            Skill_Panel[SList[i]].SetActive(true);
        }
        SList.Clear();
        switch(skillCount[8]){
            case 0 :
                ItemFound.transform.localScale = new Vector3(2, 2, 1);
                skillCount[8]++;
                break;
            case 1 : 
                ItemFound.transform.localScale = new Vector3(3, 3, 1);
                skillCount[8]++;
                break;
            case 2 :
                ItemFound.transform.localScale = new Vector3(4, 4, 1);
                skillCount[8]++;
                break;
            case 3 :
                ItemFound.transform.localScale = new Vector3(5, 5, 1);
                skillCount[8]++;
                break;
            case 4 :
                ItemFound.transform.localScale = new Vector3(6, 6, 1);
                skillCount[8]++;
                break;
            case 5 :
                Debug.Log("만렙");
                break;
        }
        Skill_Info_Text[8].text=skillInfo[40+skillCount[8]];
        Level_UI_Img[8].sprite=Level_UI[skillCount[8]];
        Time.timeScale=1f;
        LevelUpUI.SetActive(false);
        LevelUp_System();
    }
    public void OnClick_ChestOpen_Btn()
    {
        Open_Btn.SetActive(false);
        Chest.SetActive(true);
        ChestAnim.SetBool("isOpen",true);
        StartCoroutine(ChestOpen_Event());
        ItemGetSound.Play();
        int randNum=UnityEngine.Random.Range(0, itemChangeImg.Count);
        itemImg.sprite=itemChangeImg[randNum];
        itemText.text=itemChangeText[randNum];
    }
    IEnumerator ChestOpen_Event()
    {
        yield return new WaitForSecondsRealtime(2.6f);
        ChestAnim.SetBool("isOpen",false);
        Chest.SetActive(false);
        ItemImg_UI.SetActive(true);
        Ok_Btn.SetActive(true);
    }

    public void OnClick_OKBtn()
    {
        ChestAnim.SetBool("isOpen",false);
        ChestBGM.Stop();
        playermanager.BGM.Play();
        AudioSourceFolder.SetActive(true);
        ItemImg_UI.SetActive(false);
        Chest.SetActive(true);
        Open_Btn.SetActive(true);
        Ok_Btn.SetActive(false);
        Chest_UI.SetActive(false);
        Time.timeScale=1f;
    }
    
}
