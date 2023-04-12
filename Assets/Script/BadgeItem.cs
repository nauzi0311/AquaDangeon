using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeItem : MonoBehaviour
{
    int badge_num = 0;
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
        SceneManager.GetComponent<BadgeSceneManager>().ChangeDetail(badge_num);
    }
    public void SetBadgeNum(int _badge_num){
        badge_num = _badge_num;
    }
}
