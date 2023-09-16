using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlySceneMoveButton : MonoBehaviour
{
    GameObject Manager;
    [SerializeField]
    string NextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToNextScene(){
        Manager.GetComponent<Manager>().SceneMove(NextSceneName);
    }
}
