using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour
{
    [SerializeField]    Image progressBar;
    SceneMoveManager scene_move_manager;
    AsyncOperation op;
    public Text infotext;
    private void Start()
    {
        scene_move_manager=GameObject.Find("SceneMoveManager").GetComponent<SceneMoveManager>();
        StartCoroutine(LoadScene());
        int randNum=Random.Range(0,3);
        switch(randNum){
            case 0 :
                infotext.text="카렌디아는 별의 운석 조각 중 일부입니다.";
                break;
            case 1 :
                infotext.text="개발자를 위해 인앱결제를 생활화합시다.";
                break;
            case 2 :
                infotext.text="카렌디아를 소환해 강력한 무기를 얻으세요!";
                break;
        }
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        
        switch(scene_move_manager.SceneNum){
            case 1 : //StartScene
                op = SceneManager.LoadSceneAsync("StartScene");
                break;
            case 2 : //DragonMain
                op = SceneManager.LoadSceneAsync("DragonMain");
                break;
            case 3 : //BlessingScene
                op = SceneManager.LoadSceneAsync("BlessingScene");
                break;
        }
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
