using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterSceneManager : Manager
{
    [SerializeField]
    GameObject SchoolID;
    [SerializeField]
    GameObject UserName;
    [SerializeField]
    GameObject Password;

    [SerializeField]
    GameObject ErrorMessage;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetComponent<GameDirector>().FadeOut();
        ErrorMessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RegisterButtonClick(){
        string device = System.Guid.NewGuid().ToString();
        string username = UserName.GetComponent<TMP_InputField>().text;
        string id = Password.GetComponent<TMP_InputField>().text;
        int school_num = int.Parse(SchoolID.GetComponent<TMP_InputField>().text);
        if(school_num < 10000000 || school_num > 99999999){
            ErrorMessage.SetActive(true);
            return;
        }
        UserData _first = new UserData();
        _first.SetFirstData(school_num);
        GameDirector.SetUserData(_first);
        StartCoroutine(RegisterProcess(device,username,id,school_num));
        PlayerPrefs.SetString("UUID",device);
    }
    public IEnumerator RegisterProcess(string device,string username,string id,int school_num){
        SignUpData _data = new SignUpData(device,username, id, school_num);
        string json = SignUpData.Serialize<SignUpData>(_data);
        Director.GetComponent<GameDirector>().FadeMove("LoadingScene","RegisterScene");
        yield return GameDirector.WebRequestPOST("index/signup/", json);
    }
}
