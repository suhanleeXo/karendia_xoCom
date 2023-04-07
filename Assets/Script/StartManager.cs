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

    public GameObject Register_UI;
    public Text NickNameText;
    public bool isGoogleLoginOk;
    public string userid;
    public string displayName;

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

    public void GoogleLogin() //구글 로그인 여부
    {
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
                    PlayFabIdCheck();
                }
                else
                {
                    Debug.Log("구글 로그인 실패");
                    DevLogin.SetActive(true);
                    LoadingPanel.SetActive(false);
                }
            });
        }
    }

    public void PlayFabIdCheck() //playfab에 id 있는지 확인
    {
        if (isGoogleLoginOk == true)
        {
            userid = Social.localUser.id;
        }
        else
        {
            userid = InputFieldText.text;
        }
        var request = new LoginWithEmailAddressRequest { Email = userid + "@rand.com", Password = "123456" };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { Login_Success(); }, (error) => { Register_UI.SetActive(true); });
    }

    public void Login_Success() //id가 있다면 displayname 받기
    {
        scene_move_manager.SceneNum = 1;
        StartCoroutine(NextScene());
    }

    private void OnGetPlayerProfileSuccess(GetPlayerProfileResult result) //------------------------------------03_1 : 저장된 닉네임이 있다면 다음 씬으로 이동
    {
        displayName = result.PlayerProfile.DisplayName;
        Debug.Log("Display Name: " + displayName);
        LoadingPanel.SetActive(false);
        scene_move_manager.SceneNum = 1;
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        SceneManager.LoadScene("LoadingScene");
    }

    //private void ongetplayerprofileerror(playfaberror error)//------------------------------------------------회원가입 창 띄우기
    //{
    //    register_ui.setactive(true);
    //}

    public void OnClick_RegisterComplete()//-------------------------------------------------------------------회원가입 완료
    {
        ClickSound.Play();
        displayName = NickNameText.text;
        PlayFabIdCheck();
    }

    public void OnClick_DevLoginBtn()
    {
        ClickSound.Play();
        DevLogin.SetActive(false);
        PlayFabIdCheck();
    }

    public void Register()
    {
        LoadingPanel.SetActive(true);
        ClickSound.Play();
        if (isGoogleLoginOk == true)
        {
            userid = Social.localUser.id;

        }
        else
        {
            userid = InputFieldText.text;
        }
        displayName = NickNameText.text;
        var request = new RegisterPlayFabUserRequest
        {
            Email = userid + "@rand.com",
            Password = "123456",
            Username = userid,
            DisplayName = NickNameText.text
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { PlayFabIdCheck(); }, (error) => { Debug.LogError("HTTP Response Code: " + error.HttpCode); });
    }
}