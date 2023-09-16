using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetailSceneManager : Manager
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
        Source.text = _data.source;
        Out.text = _data.output;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackButtonOnClick(){
        Director.GetComponent<GameDirector>().FadeMove(GameDirector.GetPreSceneName());
    }
    public void HomeButtonOnClick(){
        Director.GetComponent<GameDirector>().FadeMove("HomeScene");
    }
}
