using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FailureSceneManager : Manager
{
    [SerializeField]
    TextMeshProUGUI ID;
    [SerializeField]
    TextMeshProUGUI Question;
    [SerializeField]
    TextMeshProUGUI Source;
    [SerializeField]
    TextMeshProUGUI Out;
    QuestionData _data;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetComponent<GameDirector>().FadeOut();
        _data = GameDirector.GetDetailData();
        ID.text = _data.id.ToString();
        Question.text = _data.question;
        Source.text = _data.source.Replace("???",_data.choice[_data.answer-1]);
        Out.text = _data.output.Replace("???",_data.choice[_data.answer-1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextButtonOnClick(){
        int _count = GameDirector.GetQuestionsCount();
        string NextSceneName = null;
        if(_count == 7){
            NextSceneName = "ResultScene";
        }else if((_count % 2) == 0){
            NextSceneName = "LoadingScene";
        }else{
            NextSceneName = "DungeonScene";
        }
        Director.GetComponent<GameDirector>().FadeMove(NextSceneName,"FailureScene");
    }
}
