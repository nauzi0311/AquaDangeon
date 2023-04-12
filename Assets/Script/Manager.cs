using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Manager : MonoBehaviour
{
    public GameObject Director;
    void Start() {
        Director = GameObject.FindWithTag("GameDirector");
    }

    void Update() {
        
    }
    public void SceneMove(string NextSceneName,string NowSceneName = null){
        Director.GetComponent<GameDirector>().FadeMove(NextSceneName,NowSceneName);
    }
}


