using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonSceneManager : Manager
{
    [SerializeField]
    GameObject Question;
    [SerializeField]
    GameObject Source;
    [SerializeField]
    GameObject Out;
    [SerializeField]
    List<GameObject> ChoiceItems;
    [SerializeField]
    TextMeshProUGUI Timer;
    QuestionData _question;
    GameObject OKCanvas,BadCanvas;
    Coroutine _timer;
    string NextSceneName;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetComponent<GameDirector>().FadeOut();
        _question = GameDirector.GetQuestionData();
        Question.GetComponent<TextMeshProUGUI>().text = _question.question;
        Source.GetComponent<TextMeshProUGUI>().text = _question.source;
        SourceAreaAdjust();
        Out.GetComponent<TextMeshProUGUI>().text = _question.output;
        for(int i = 0; i < ChoiceItems.Count; i++){
            ChoiceItems[i].GetComponentInChildren<TextMeshProUGUI>().text = _question.choice[i];
        }
        OKCanvas = GameObject.Find("OKCanvas");
        BadCanvas = GameObject.Find("BadCanvas");
        _timer = StartCoroutine(TimerStart());
        if(GameDirector.GetQuestionsCount() % 2 == 0){
            NextSceneName = "LoadingScene";
        }else if(GameDirector.GetQuestionsCount() == 7){
            NextSceneName = "ResultScene";
        }else{
            NextSceneName = "DungeonScene";
        }
        Debug.Log("Rank : " + _question.rank + 
        "\nID:" + _question.id +
        "\nQuestion:" + _question.source + 
        "\nAnswer:" + _question.answer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChoiceOnClick(int ans_count){
        StopCoroutine(_timer);
        if(_question.answer == ans_count){
            OKCanvas.GetComponent<Canvas>().sortingOrder = 2;
        }else{
            BadCanvas.GetComponent<Canvas>().sortingOrder = 2;
            NextSceneName = "FailureScene";
            GameDirector.SetDetailData(_question);
        }
        GameDirector.AddResultData(_question.id,_question.answer == ans_count,60 - int.Parse(Timer.text),ans_count,_question.exp,_question.point);
        Invoke("FadeMove",0.5f);
    }
    public void SkipOnClick(){
        NextSceneName = "FailureScene";
        GameDirector.SetDetailData(_question);
        GameDirector.AddResultData(_question.id,false,60 - int.Parse(Timer.text),5,_question.exp,_question.point);
        Director.GetComponent<GameDirector>().FadeMove(NextSceneName,"DungeonScene");
    }
    IEnumerator TimerStart(){
        int time = int.Parse(Timer.text);
        while(time > 0){
            yield return new WaitForSeconds(1.0f);
            Timer.text = (--time).ToString();
        }
        BadCanvas.GetComponent<Canvas>().sortingOrder = 2;
        NextSceneName = "FailureScene";
        GameDirector.SetDetailData(_question);
        GameDirector.AddResultData(_question.id,false,61,6,_question.exp,_question.point);
        Director.GetComponent<GameDirector>().FadeMove(NextSceneName,"DungeonScene");
    }
    public void FadeMove(){
        Director.GetComponent<GameDirector>().FadeMove(NextSceneName,"DungeonScene");
    }
    public void SourceAreaAdjust(){
        int line_count = _question.source.Length - _question.source.Replace("\n", "").Length;
        Debug.Log(line_count);
        Transform _parent = Source.transform.parent;
        Vector2 _size = _parent.gameObject.GetComponent<RectTransform>().sizeDelta;
        //17行分を表示できる
        if(_size.y < 1000*line_count/17){
            _size.y = 1000*line_count/17;
            _parent.gameObject.GetComponent<RectTransform>().sizeDelta = _size;
        }
    }
}
