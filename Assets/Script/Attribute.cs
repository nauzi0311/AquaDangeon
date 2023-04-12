using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Attribute : MonoBehaviour
{
    [SerializeField]
    Slider ExpGauge;
    [SerializeField]
    TextMeshProUGUI Level;
    [SerializeField]
    TextMeshProUGUI Exp;
    GameObject Director;
    // Start is called before the first frame update
    void Start()
    {
        Director = GameObject.FindWithTag("GameDirector");
        Invoke("UpdateUserData",0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateUserData(){
        UserData userData = GameDirector.GetUserData();
        ConfigData config = GameDirector.GetConfigData();
        Level.text = "Lv" + userData.level.ToString();
        Exp.text = userData.exp.ToString() + "/" + config.level[userData.level];
        ExpGauge.maxValue = config.level[userData.level];
        ExpGauge.value = userData.exp;
    }
}
