using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GooglePlayGames;
using PlayFab;
using PlayFab.ClientModels;

public class StartManager : MonoBehaviour
{
    public AudioSource ClickSound;
    public GameObject TouchUI;
    SceneMoveManager scene_move_manager;
    public GameObject DevLogin;
    public Text InputFieldText;
    public GameObject LoadingPanel;
    public Text LogText;
    public GameObject Register_UI;
    public Text NickNameText;
    public bool isGoogleLoginOk;
    public string userid;
    public string username;
    public string displayName;
    public string playFabId;
    
    // GetUserData API로 받아온 결과를 저장할 변수
    Dictionary<string, UserDataRecord> userDataDictionary;

    // Dictionary<string, string> 형식으로 변환한 결과를 저장할 변수
    Dictionary<string, string> userDataStringDictionary = new Dictionary<string, string>();

    void Start()
    {
        StartCoroutine("TouchActive");
        scene_move_manager = GameObject.Find("SceneMoveManager").GetComponent<SceneMoveManager>();
        PlayGamesPlatform.DebugLogEnabled = true; //디버그용 변수
        PlayGamesPlatform.Activate();//구글 관련 서비스 활성화
    }

    IEnumerator TouchActive()
    {
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        TouchUI.SetActive(true);
    }

    public void GoogleLogin() //구글 로그인인지 아닌지
    {
        Debug.Log("구글로그인인지 아닌지 확인");
        ClickSound.Play();
        TouchUI.SetActive(false);
        LoadingPanel.SetActive(true);
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    isGoogleLoginOk = true;
                    Debug.Log("구글 로그인 성공");
                    LogText.text="구글 로그인 성공";
                    PlayFabIdCheck(); //id있는지 체크
                }
                else
                {
                    Debug.Log("구글 로그인 실패");
                    LogText.text="구글 로그인 실패";
                    DevLogin.SetActive(true);
                    LoadingPanel.SetActive(false);
                }
            });
        }
    }

    public void PlayFabIdCheck() //playfab에 id 있는지 확인
    {
        Debug.Log("PlayFab에 id가 있는지 확인");
        if (isGoogleLoginOk == true)
        {
            GoogleLoginCheck();
        }
        else if(isGoogleLoginOk==false)
        {
            DevLoginCheck();
        }
    }
    public void GoogleLoginCheck()
    {
        var request = new LoginWithEmailAddressRequest { Email=Social.localUser.id + "@rand.com", Password=Social.localUser.id};
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { GoogleLogin_Success(); }, (error) => { LoginFailed(); });
    }

    public void DevLoginCheck()
    {
        userid=InputFieldText.text;
        var request = new LoginWithEmailAddressRequest { Email=userid + "@rand.com", Password="suhan1234"};
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { DevLogin_Success(); }, (error) => { LoginFailed(); });
    }

    public void LoginFailed()
    {
        Debug.Log("회원가입창으로 이동");
        Register_UI.SetActive(true);
        LoadingPanel.SetActive(false);
    }

    public void GoogleLogin_Success() //id가 있다면 displayname 받기
    {
        scene_move_manager.userid=Social.localUser.id;
        if(scene_move_manager.displayname=="")
        {
            Debug.Log("디스플레이 네임 가져오기");
            var request = new GetAccountInfoRequest
            {
                Email = Social.localUser.id + "@rand.com",
            };
            PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
        }
        else
        {
            LoadingPanel.SetActive(false);
            scene_move_manager.userid=userid;
            scene_move_manager.SceneNum=1;
            SceneManager.LoadScene("LoadingScene");
            //StartCoroutine(NextScene());
        }
        
    }

    public void DevLogin_Success() //id가 있다면 displayname 받기
    {
        if(scene_move_manager.displayname=="")
        {
            Debug.Log("디스플레이 네임 가져오기");
            var request = new GetAccountInfoRequest
            {
                Email = userid + "@rand.com",
            };
            PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
        }
        else
        {
            LoadingPanel.SetActive(false);
            scene_move_manager.userid=userid;
            scene_move_manager.SceneNum=1;
            SceneManager.LoadScene("LoadingScene");
            //StartCoroutine(NextScene());
        }
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        scene_move_manager.displayname = result.AccountInfo.TitleInfo.DisplayName;
        Debug.Log("디스플레이 가져오기 완료");
        CheckPlayFabID();
    }

    public void OnGetAccountInfoFailure(PlayFabError error)
    {
        LoadingPanel.SetActive(false);
        Debug.Log("디스플레이 이름 조회 실패");
        LogText.text="디스플레이 이름 조회 실패";
    }
    IEnumerator NextScene()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnClick_DevLoginBtn()
    {
        ClickSound.Play();
        DevLogin.SetActive(false);
        LoadingPanel.SetActive(true);
        PlayFabIdCheck();
    }

    public void Register()
    {
        displayName = NickNameText.text;
        LoadingPanel.SetActive(true);
        ClickSound.Play();
        if (isGoogleLoginOk == true)
        {
            GoogleRegister();
        }
        else if(isGoogleLoginOk==false);
        {
            DevRegister();
        }
    }
    public void GoogleRegister()
    {
        var request = new RegisterPlayFabUserRequest { Email = Social.localUser.id + "@rand.com", Password = Social.localUser.id, Username = Social.localUser.userName, DisplayName=NickNameText.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { SaveLevel(1,100,0); }, (error) => { RegisterError();});

    }
    public void DevRegister()
    {
        var request = new RegisterPlayFabUserRequest { Email = userid + "@rand.com", Password = "suhan1234", Username = userid,DisplayName=NickNameText.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { SaveLevel(1,100,0); }, (error) => { RegisterError();});
    }
    public void RegisterError()
    {
        Debug.LogError("회원가입 실패");
        LogText.text="회원가입 실패";
        LoadingPanel.SetActive(false);
    }

    public void SaveLevel(int level, int maxEx, int curEx)
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "level", level.ToString() }, { "maxEx", maxEx.ToString() }, {"curEx",curEx.ToString()} } };
        PlayFabClientAPI.UpdateUserData(request,OnDataSaved,OnDataSaveFailure);
    }

    public void OnDataSaved(UpdateUserDataResult result)
    {
        Debug.Log("레벨,전체 경험치, 현재 경험치 저장 성공");
        scene_move_manager.level=1;
        scene_move_manager.maxEx=100;
        scene_move_manager.curEx=0;
        scene_move_manager.karendia=40;
        PlayFabIdCheck();
    }

    public void OnDataSaveFailure(PlayFabError error)
    {
        Debug.Log("레벨 저장 실패");
        LoadingPanel.SetActive(false);
    }

    


    public void CheckPlayFabID()
    {
        Debug.Log("PlayFab아이디 가져오기");
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnAccountInfoLoaded, OnAccountInfoFailed);
    }

    public void OnAccountInfoLoaded(GetAccountInfoResult result)
    {
        playFabId = result.AccountInfo.PlayFabId;
        Debug.Log("PlayFab ID 가져오기 완료");
        Debug.Log("레벨 가져오기");
        GetLevelLoad();
    }

    public void OnAccountInfoFailed(PlayFabError error)
    {
        Debug.LogError("Account info load failed: " + error.ErrorMessage);
    }

    public void GetLevelLoad()
    {
        var request=new GetUserDataRequest() {PlayFabId = playFabId};
        PlayFabClientAPI.GetUserData(request, (result)=>
        {
            userDataDictionary = result.Data;

            // Dictionary<string, UserDataRecord>를 Dictionary<string, string>으로 변환하여 userDataStringDictionary에 저장
            foreach (var data in userDataDictionary)
            {
                userDataStringDictionary.Add(data.Key, data.Value.Value);
            }
            scene_move_manager.level=int.Parse(userDataStringDictionary["level"]);
            scene_move_manager.maxEx=float.Parse(userDataStringDictionary["maxEx"]);
            scene_move_manager.curEx=float.Parse(userDataStringDictionary["curEx"]);
            Debug.Log("레벨 가져오기 성공");
            PlayFabIdCheck();

        },(error)=>OnDataLoadFailed());
        
    }
    public void OnDataLoadFailed()
    {
        Debug.Log("레벨 불러오기 실패");
        LoadingPanel.SetActive(false);
    }
}