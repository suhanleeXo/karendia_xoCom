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
    
    void Start()
    {
        StartCoroutine("TouchActive");
        scene_move_manager=GameObject.Find("SceneMoveManager").GetComponent<SceneMoveManager>();

        PlayGamesPlatform.DebugLogEnabled=true; //디버그용 변수
        PlayGamesPlatform.Activate();//구글 관련 서비스 활성화
        
        
    }

    IEnumerator TouchActive()
    {
        yield return YieldInstructionCache.WaitForSeconds(3.5f);
        TouchUI.SetActive(true);
    }

    public void OnClick_StartUI()
    {
        LoadingPanel.SetActive(true);
        ClickSound.Play();
        Login();
    }
    
    public void DevLogin_Success()
    {
        scene_move_manager.SceneNum=1;
        ClickSound.Play();
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return YieldInstructionCache.WaitForSeconds(1f);
        SceneManager.LoadScene("LoadingScene");
    }

    
    public void Login()
    {
        LoadingPanel.SetActive(true);
        if(PlayGamesPlatform.Instance.localUser.authenticated==false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success){
                    Debug.Log("구글 로그인 성공"); 
                    PlayFabLogin();
                }
            else {
                Debug.Log("구글 로그인 실패");
                DevLogin.SetActive(true);
                LoadingPanel.SetActive(false);            
            }
        });
        }
        
    }
    public void OnClick_DevLogin()
    {
        LoadingPanel.SetActive(true);
        var request = new LoginWithEmailAddressRequest { Email = InputFieldText.text + "@rand.com", Password = InputFieldText.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => DevLogin_Success(), (error) => DevRegister());
    }

    public void DevRegister()
    {
        var request = new RegisterPlayFabUserRequest { Email = InputFieldText.text + "@rand.com", Password = InputFieldText.text, Username = InputFieldText.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { OnClick_DevLogin(); }, (error) => Debug.Log("로그인 실패"));
    }
    public void PlayFabLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = Social.localUser.id + "@rand.com", Password = Social.localUser.id };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => DevLogin_Success(), (error) => PlayFabRegister());
    }

    public void PlayFabRegister()
    {
        var request = new RegisterPlayFabUserRequest { Email = Social.localUser.id + "@rand.com", Password = Social.localUser.id, Username = Social.localUser.userName };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { PlayFabLogin(); }, (error) => Debug.Log("로그인 실패"));
    }
    
    
}
