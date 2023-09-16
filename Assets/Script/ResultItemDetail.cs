using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultItemDetail : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ID;
    GameObject Manager;
    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindWithTag("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick(){
        int id = int.Parse(ID.text);
        DetailData data = new DetailData(PlayerPrefs.GetString("UUID"),id);
        string json = DetailData.Serialize<DetailData>(data);
        StartCoroutine(Manager.GetComponent<ResultSceneManager>().WebRequestPOST("detail",json));
        Manager.GetComponent<ResultSceneManager>().SceneMove("LoadingScene","ResultScene");
    }
}
