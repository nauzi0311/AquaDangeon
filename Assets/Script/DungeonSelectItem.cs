using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonSelectItem : MonoBehaviour
{
    public bool available = false;
    public int times;
    GameObject Available;
    GameObject Manager;
    UserData _userData;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("SceneManager");
        Available = transform.Find("Available").gameObject;
        _userData = GameDirector.GetUserData();
        if(times <= _userData.level){
            available = true;
            Color _c = Available.GetComponent<Image>().color;
            _c.a = 0.0f;
            Available.GetComponent<Image>().color = _c; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveToNextScene(){
        if(available){
            StartCoroutine(Manager.GetComponent<DungeonSelectSceneManager>().GetQuestions(times));
            Manager.GetComponent<Manager>().SceneMove("LoadingScene","DungeonSelectScene");
        }
    }
}
