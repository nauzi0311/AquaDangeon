using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreListItem : MonoBehaviour
{
    GameObject Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick(){
        int id = int.Parse(transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text.Split(" ")[0]);
        DetailData _data = new DetailData(PlayerPrefs.GetString("UUID"),id);
        string json = DetailData.Serialize<DetailData>(_data);
        StartCoroutine(Manager.GetComponent<ScoreSceneManager>().WebRequestPOST("detail",json));
        Manager.GetComponent<ScoreSceneManager>().SceneMove("LoadingScene","ScoreScene");
    }
}
