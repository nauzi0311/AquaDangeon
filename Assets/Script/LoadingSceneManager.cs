using System.Collections;
using UnityEngine;
using TMPro;



public class LoadingSceneManager : Manager
{
    [SerializeField] GameObject Loading;
    public bool IsStop;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ViewProcess());
        StartCoroutine(LoadProcess());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ViewProcess(){
        bool IsStop = false;
        TextMeshProUGUI Text = Loading.GetComponent<TextMeshProUGUI>();
        while(!IsStop){
            string target = Text.text;
            if(target[target.Length - 3] == '.'){
                Text.text = target.Substring(0, target.Length - 3);
            }else{
                Text.text = target + '.';
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator LoadProcess(){
        string PreSceneName = GameDirector.GetPreSceneName();
        if(PreSceneName == "TopScene"){
            if(!string.IsNullOrEmpty(PlayerPrefs.GetString("UUID"))){
                DeviceData _data = new DeviceData(PlayerPrefs.GetString("UUID"));
                string json = DeviceData.Serialize<DeviceData>(_data);
                yield return StartCoroutine(GameDirector.WebRequestPOST("index/",json));
                if(GameDirector.GetResponse() == "new"){
                    PlayerPrefs.DeleteKey("UUID");
                    StartCoroutine(GameDirector.SceneMove("RegisterScene","TopScene"));
                    yield break;
                }
                GameDirector.SetUserData(UserData.Deserialize<UserData>(GameDirector.GetResponse()));
                Debug.Log(GameDirector.GetUserData().config);
                GameDirector.SetConfigData(GameDirector.GetUserData().config);
                IsStop = true;
                StartCoroutine(GameDirector.SceneMove("HomeScene","TopScene"));
            }else{
                StartCoroutine(GameDirector.SceneMove("RegisterScene","TopScene"));
            }
        }
        if(PreSceneName == "RegisterScene"){
            ConfigData _data = ConfigData.Deserialize<ConfigData>(GameDirector.GetResponse());
            GameDirector.SetConfigData(_data);
            StartCoroutine(GameDirector.SceneMove("HomeScene","TopScene"));
        }
        if(PreSceneName == "DungeonSelectScene"){
            yield return new WaitUntil(() => GameDirector.IsQuestionReady());
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(GameDirector.SceneMove("DungeonScene"));
        }
        if(PreSceneName == "DungeonScene" || PreSceneName == "FailureScene"){
            int _count = GameDirector.GetQuestionsCount();
            if(GameDirector.correct_list[_count-1] || GameDirector.correct_list[_count-2]){
                GameDirector.OneRankUP();
            }else{
                GameDirector.OneRankDown();
            }
            yield return new WaitForSeconds(3.0f);
            StartCoroutine(GameDirector.SceneMove("DungeonScene"));
        }
        if(PreSceneName == "ScoreScene"){
            yield return new WaitUntil(() => GameDirector.IsDetailReady());
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(GameDirector.SceneMove("DetailScene","ScoreScene"));
        }
        if(PreSceneName == "ResultScene"){
            yield return new WaitUntil(() => GameDirector.IsDetailReady());
            yield return new WaitForSeconds(1.0f);
            StartCoroutine(GameDirector.SceneMove("DetailScene","ResultScene"));
        }
    }
}
