using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    public Scanner scanner;
    public GameManager manager;
    public GameObject gameoverUI;
    public GameObject FinalSkillFire;
    public Camera MainCamera;
    public float maxHealth;
    public float curHealth;
    public Slider hpbar;
    public AudioSource GameOverSound;
    public AudioSource BGM;
    public AudioSource PlayerChangeSound;
    public Text GameOver_InfoText;
    public float curFinalSkill=0;
    public float maxFinalSkill=120;
    public Slider FinalSkillbar;
    public int ChangeNum=0;
    Animator anim;
    
    //조이스틱
    public bl_Joystick js;
    public float speed;
    //Animator anim;
    public Text KillText;
    public static int killLog=0;
    public Vector3 dir;

    float lastZoomSpeed;
    void Start()
    {
        anim=GetComponent<Animator>();
        MainCamera=GetComponentInChildren<Camera>();
        scanner=GetComponent<Scanner>();
    }
    
    void Update()
    {
        hpbar.value = curHealth / maxHealth;
        FinalSkillbar.value=curFinalSkill / maxFinalSkill;
        if(ChangeNum==2){
            ZoomOut();
        }
        else if(ChangeNum==1) {
            ZoomIn();
        }

        dir = new Vector3(js.Horizontal, js.Vertical,0);
        if(js.Horizontal>0){
            anim.SetBool("isRun",true);
            transform.localScale=new Vector3(-1f,1f,1f);
        }
        else if(js.Horizontal<0){
            anim.SetBool("isRun",true);
            transform.localScale=new Vector3(1f,1f,1f);
        }
        else{
            anim.SetBool("isRun",false);
        }
        dir.Normalize();
        transform.position+=dir*speed*Time.deltaTime;
    }
    
    void OnTriggerStay2D(Collider2D other){
        if(other.tag=="NormalEnemy"){
            Enemy_ScriptManager normalenemyAttack=other.GetComponent<Enemy_ScriptManager>();
            curHealth-=normalenemyAttack.damage*Time.deltaTime;
            StartCoroutine(OnDamage());
        }
        else if(other.tag=="ThirdBoss"){
            ThirdBossManager thirdboss_attack=other.GetComponent<ThirdBossManager>();
            curHealth-=thirdboss_attack.damage*Time.deltaTime;
            StartCoroutine(OnDamage());
        }
        else if(other.tag=="SecondBoss"){
            SecondBossManager secondboss_attack=other.GetComponent<SecondBossManager>();
            curHealth-=secondboss_attack.damage*Time.deltaTime;
            StartCoroutine(OnDamage());
        }
        else if(other.tag=="ThirdBossSkill"){
            curHealth-=10*Time.deltaTime;
            StartCoroutine(OnDamage());
        }
    }
    
    
    IEnumerator OnDamage()
    {
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        if(curHealth<=0f){
            GameOver_InfoText.text="플레이 시간 : "+GameManager.instance.TimerText.text+"\n\n처치 수 : "+KillText.text+"\n\n획득 카렌디아 : 0\n\n획득 골드 : 100골드";
            StartCoroutine("gameover");
        }
    }
    IEnumerator gameover()
    {
        manager.AudioSourceFolder.SetActive(false);
        gameoverUI.SetActive(true);
        BGM.Stop();
        GameOverSound.Play();
        yield return YieldInstructionCache.WaitForSeconds(0.6f); 
        Time.timeScale=0f;
    }
    public IEnumerator FinalSkillTime()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        if(curFinalSkill>=maxFinalSkill){
            yield return null;
        }
        else{
            curFinalSkill++;
            StartCoroutine("FinalSkillTime");
        } 
    }
    public void OnClick_FinalSkill() //궁극기 버튼 누르기
    {
        if(curFinalSkill>=maxFinalSkill){
            ChangeNum=2;
            PlayerChangeSound.Play();
            anim.SetBool("isUpgrade",true);
            manager.NormalAttack.transform.localScale = new Vector3(20, 20, 1);
            manager.FireBallFolder.transform.localScale=new Vector3(2,2,1);
            transform.localScale = new Vector3(3, 3, 1);
            FinalSkillFire.SetActive(true);
            StartCoroutine("FinalSkillActive");
        }
    }

    IEnumerator FinalSkillActive() //궁극기 사용 중
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        if(curFinalSkill<=0){
            curFinalSkill=0;
            transform.localScale = new Vector3(1, 1, 1); 
            manager.NormalAttack.transform.localScale = new Vector3(8, 8, 1);
            manager.FireBallFolder.transform.localScale=new Vector3(1,1,1);
            FinalSkillFire.SetActive(false);
            anim.SetBool("isUpgrade",false);
            ChangeNum=1;
            StartCoroutine("FinalSkillTime");
        }
        else{
            curFinalSkill=curFinalSkill-20;
            curHealth=maxHealth;
            StartCoroutine("FinalSkillActive");
        } 
    }

    void ZoomOut()
    {
        float smoothZoomSize = Mathf.SmoothDamp(MainCamera.orthographicSize, 15, ref lastZoomSpeed, 0.2f);                                        
        MainCamera.orthographicSize = smoothZoomSize;
        if(smoothZoomSize>=15f){
            ChangeNum=0;
        }
    }
    void ZoomIn()
    {
        float smoothZoomSize = Mathf.SmoothDamp(MainCamera.orthographicSize, 12, ref lastZoomSpeed, 0.2f);                                        
        MainCamera.orthographicSize = smoothZoomSize;
        if(smoothZoomSize<=12.1f){
            ChangeNum=0;
        }
    }
}
