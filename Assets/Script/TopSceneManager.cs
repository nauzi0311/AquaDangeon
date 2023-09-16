using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopSceneManager : Manager
{
    [SerializeField] GameObject Tap;
    [SerializeField] int AlertCount;
    [SerializeField] float duration;
    Canvas VersionCanvas;
    // Start is called before the first frame update
    void Start()
    {
        VersionCanvas = GameObject.Find("VersionCanvas").GetComponent<Canvas>();
        PlayerPrefs.SetString("Version","1-0-0");
        StartCoroutine(CheckVersion());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(VersionCanvas.sortingOrder >= 1){
                return;
            }
            StartCoroutine(Alert());
        }
    }

    IEnumerator Alert(){
        int i;
        for (i = 0; i < AlertCount;i++){
            Tap.gameObject.SetActive(!Tap.gameObject.activeSelf);
            yield return new WaitForSeconds(duration);
        }
        if(i == AlertCount){
            this.Director.GetComponent<GameDirector>().FadeMove("LoadingScene","TopScene");
        }
    }

    IEnumerator CheckVersion(){
        string now_version = PlayerPrefs.GetString("Version");
        if(now_version != null){
            string json = "{\"version\":\"" + now_version + "\"}";
            yield return GameDirector.WebRequestPOST("index/version/",json);
            VersionData _data = VersionData.Deserialize<VersionData>(GameDirector.GetResponse());
            if(now_version != _data.version){
                VersionCanvas.GetComponent<Canvas>().sortingOrder = 2;
                TextMeshProUGUI Version = VersionCanvas.transform.Find("Version").GetComponent<TextMeshProUGUI>();
                Version.text += ("\n<u>" + _data.url + "</u>");
            }
        }
    }
}
