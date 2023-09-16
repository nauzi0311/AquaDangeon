using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalenderButton : MonoBehaviour
{
    GameObject Calender;
    GameObject SceneManager;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager = GameObject.FindWithTag("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick(){
        SceneManager.GetComponent<HomeSceneManager>().VisibleCalender();
    }
}
