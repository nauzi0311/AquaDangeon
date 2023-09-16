using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSelectSceneManager : Manager
{
    UserData _userdata;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetComponent<GameDirector>().FadeOut();
        GameDirector.ResetDetailData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator GetQuestions(int times){
        GameDirector.SetTimes(times);
        string device = PlayerPrefs.GetString("UUID");
        string course = "soft1";
        RequestData _data = new RequestData(device, course,times);
        string json = RequestData.Serialize<RequestData>(_data);
        yield return StartCoroutine(GameDirector.WebRequestPOST("quest/",json));
        GameDirector.SetQuestionDataSet(QuestionDataSet.Deserialize<QuestionDataSet>(GameDirector.GetResponse()));
    }
}
