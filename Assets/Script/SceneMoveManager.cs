using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class SceneMoveManager : MonoBehaviour
{
    public int SceneNum;
    public string userid;
    public string displayname;
    public int level;
    public float maxEx;
    public float curEx;
    public int karendia;
    
    // Start is called before the first frame update
    void Start()
    {
        var obj = FindObjectsOfType<SceneMoveManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
