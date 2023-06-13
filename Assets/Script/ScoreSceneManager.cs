using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSceneManager : Manager
{
    Vector3 ListItemPos = new Vector3(0,-100,0) + new Vector3(0,500,0);
    Vector3 ListItemScale = new Vector3(1,1,1);
    [SerializeField]
    GameObject PListItem;
    [SerializeField]
    GameObject ListArea;
    ScoreData _data;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetComponent<GameDirector>().FadeOut();
        _data = GameDirector.GetScoreData();
        GenerateList(_data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackButtonOnClick(){
        string NextSceneName = "HomeScene";
        Director.GetComponent<GameDirector>().FadeMove(NextSceneName);
    }
    void GenerateList(ScoreData _data){
        int _count = _data.id_list.Count;
        for(int i = 0; i < _count; i++){
            var obj = Instantiate(PListItem);
            Transform _trans = obj.transform;
            _trans.SetParent(ListArea.transform);
            _trans.localPosition = ListItemPos;
            _trans.localScale = ListItemScale;
            ListItemPos += new Vector3(0,-200,0);
            _trans.Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = _data.id_list[i].ToString() + " " + _data.title_list[i];
        }
        Vector2 _now_size = ListArea.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta;
        ListArea.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_now_size.x,200*(_count - 8) - 30);
    }

    public IEnumerator WebRequestPOST(string uri,string json){
        yield return GameDirector.WebRequestPOST(uri,json);
        GameDirector.SetDetailData(QuestionData.Deserialize<QuestionData>(GameDirector.GetResponse()));
    }
}
