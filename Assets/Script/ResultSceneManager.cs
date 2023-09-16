using System.Data.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class ResultSceneManager : Manager
{
    Vector3 ListItemPos = new Vector3(0,-125,0);
    Vector3 ListItemScale = new Vector3(1,1,1);
    [SerializeField]
    GameObject PrefabResultListItem;
    [SerializeField]
    GameObject ListArea;
    [SerializeField]
    Canvas NewBadgeCanvas;
    [SerializeField]
    TextMeshProUGUI DebugArea;
    QuestionDataSet _questions;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetComponent<GameDirector>().FadeOut();
        _questions = GameDirector.GetQuestionDataSet();
        GenerateList();
        if(GameDirector.GetIs_update()){
            StartCoroutine(ResultProcess());
            GameDirector.SetIs_update();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool UserDataUpdate(){
        bool result_calcexp = CalcExpPoint();
        bool result_addcorrect = AddCorrectList();
        bool result_adddate = AddDate();
        bool result_badgeDetect = BadgeDetect();
        return result_calcexp && result_addcorrect && result_adddate && result_badgeDetect;
    }
    void GenerateList(){
        for(int i = 0; i < GameDirector.correct_list.Count;i++){
            QuestionData _quest = null;
            for(int j = 0;j < _questions.question_used.Count;j++){
                if(_questions.quest[j].id == GameDirector.id_list[i]){
                    _quest = _questions.quest[j];
                }
            }
            var obj = Instantiate(PrefabResultListItem);
            Transform _trans = obj.transform;
            _trans.SetParent(ListArea.transform);
            _trans.localPosition = ListItemPos;
            _trans.localScale = ListItemScale;
            ListItemPos += new Vector3(0,-250,0);
            _trans.Find("id").gameObject.GetComponent<TextMeshProUGUI>().text = GameDirector.id_list[i].ToString();
            _trans.Find("answer").gameObject.GetComponent<TextMeshProUGUI>().text = _quest.choice[_quest.answer - 1];
            if(GameDirector.user_answer[i] >= 1 && GameDirector.user_answer[i] <= 4){
                _trans.Find("yourans").gameObject.GetComponent<TextMeshProUGUI>().text = _quest.choice[GameDirector.user_answer[i] - 1];
            }else if(GameDirector.user_answer[i] == 5){
                _trans.Find("yourans").gameObject.GetComponent<TextMeshProUGUI>().text = "Skip";
            }else{
                _trans.Find("yourans").gameObject.GetComponent<TextMeshProUGUI>().text = "TimeOut";
            }
            if(GameDirector.second_list[i] >= 30 || !GameDirector.correct_list[i]){
                _trans.Find("bonus").gameObject.SetActive(false);
            }
        }
    }
    bool CalcExpPoint(){
        int sum_exp = 0,sum_point = 0;
        var _expList = GameDirector.exp_list;
        var _pointList = GameDirector.point_list;
        QuestionDataSet _dataSet = GameDirector.GetQuestionDataSet();
        for(int i = 0; i < _expList.Count;i++){
            if(GameDirector.correct_list[i]){
                if(GameDirector.second_list[i] >= 30){
                    sum_exp += _expList[i];
                }else{
                    sum_exp += (int)(_expList[i]*3/2);
                }
                sum_point += _pointList[i];
            }
        }
        UserData _data = GameDirector.GetUserData();
        ConfigData _config = GameDirector.GetConfigData();
        _data.exp += sum_exp;
        _data.point += sum_point;
        while(_config.level[_data.level] <= _data.exp){
            _data.exp -= _config.level[_data.level];
            _data.level++;
        }
        GameDirector.SetUserData(_data);
        return true;
    }
    bool AddCorrectList(){
        UserData _data = GameDirector.GetUserData();
        for(int i = 0;i < GameDirector.correct_list.Count;i++){
            if(GameDirector.correct_list[i]){
                if(!_data.correct_id.Contains(GameDirector.id_list[i])){
                    _data.correct_id.Add(GameDirector.id_list[i]);
                }
                _data.correct_count++;
            }
        }
        GameDirector.SetUserData(_data);
        return true;
    }
    bool AddDate(){
        UserData _data = GameDirector.GetUserData();
        if(!_data.date.Contains(DateTime.Now.ToString("yyyy-MM-dd"))){
            _data.date.Add(DateTime.Now.ToString("yyyy-MM-dd"));
        }
        GameDirector.SetUserData(_data);
        return true;
    }
    public void BadgeButtonOnClick(){
        Director.GetComponent<GameDirector>().FadeMove("BadgeScene","ResultScene");
    }
    public void HomeButtonOnClick(){
        Director.GetComponent<GameDirector>().FadeMove("HomeScene","ResultScene");
    }
    public void RankingButtonOnClick(){
        StartCoroutine(RankingProcess());
    }
    public IEnumerator WebRequestPOST(string url, string json){
        yield return GameDirector.WebRequestPOST(url, json);
        GameDirector.SetDetailData(QuestionData.Deserialize<QuestionData>(GameDirector.GetResponse()));
    }
    public IEnumerator RankingProcess(){
        string json ="{\"device\":\"" + PlayerPrefs.GetString("UUID") + "\"}";
        yield return StartCoroutine(GameDirector.WebRequestPOST("rank",json));
        GameDirector.SetRankingData(RankingData.Deserialize<RankingData>(GameDirector.GetResponse()));
        Director.GetComponent<GameDirector>().FadeMove("RankingScene","ResultScene");
    }
    public IEnumerator ResultProcess(){
        yield return new WaitUntil(() => UserDataUpdate());
        UserData _userData = GameDirector.GetUserData();
        ResultData _data = new ResultData();
        _data.SetData(
            PlayerPrefs.GetString("UUID"),
            _userData.level,
            _userData.exp,
            _userData.point,
            GameDirector.id_list,
            GameDirector.correct_list,
            GameDirector.second_list,
            GameDirector.user_answer,
            _userData.badge
        );
        string json = ResultData.Serialize<ResultData>(_data);
        Debug.Log(json);
        yield return GameDirector.WebRequestPOST("result",json);
    }
    public IEnumerator NewBadgeNotion(){
        NewBadgeCanvas.sortingOrder = 3;
        yield return new WaitForSeconds(1.5f);
        NewBadgeCanvas.sortingOrder = -1;
    }
    bool BadgeDetect(){
        UserData _data = GameDirector.GetUserData();
        bool FirstBadge(){
            if(_data.correct_id.Count >= 7){
                if(!_data.badge.Contains(0)){
                    _data.badge.Add(0);
                    return true;
                }
            }
            return false;
        }
        bool LevelBadge(){
            List<int> levelBadgeID = new List<int>{
                1,4,7,10,13,
                17,20,23,26,29,
                33,36,39,42,45
            };
            List<int> levelCondition = new List<int>{
                2,3,4,5,7,
                9,11,13,15,17,
                20,25,30,35,40
            };
            int i;
            bool ans = false;
            for(i = 0;levelCondition[i] <= _data.level;i++);i--;
            while(i >= 0){
                if(!_data.badge.Contains(levelBadgeID[i--])){
                    _data.badge.Add(levelBadgeID[i+1]);
                    ans = true;
                }
            }
            return ans;
        }
        bool QuestionCountBadge(){
            List<int> countBadgeID = new List<int>{
                3,6,9,12,15,
                19,22,25,28,31,
                35,38,41,44,47
            };
            List<int> countCondition = new List<int>{
                5,10,15,25,30,
                45,60,75,90,110,
                130,150,170,190,210
            };
            int i;
            bool ans = false;
            for(i = 0;countCondition[i] <= _data.correct_count;i++);i--;
            while(i >= 0){
                if(!_data.badge.Contains(countBadgeID[i--])){
                    _data.badge.Add(countBadgeID[i+1]);
                    ans = true;
                }
            }
            return ans;
        }
        bool LoginBadge(){
            List<int> loginBadgeID = new List<int>{
                2,5,8,11,14,
                18,21,24,27,30,
                34,37,40,43,46
            };
            List<int> loginCondition = new List<int>{
                2,3,4,5,7,
                9,11,13,15,17,
                21,25,30,35,40
            };
            int i;
            bool ans = false;
            //ログイン日数のチェック
            for(i = 0;loginCondition[i] <= _data.date.Count;i++);i--;
            while(i >= 0){
                if(!_data.badge.Contains(loginBadgeID[i--])){
                    _data.badge.Add(loginBadgeID[i+1]);
                    ans = true;
                }
            }
            return ans;
        }
        bool AllCollectBadge(){
            List<int> allCollectBadgeID = new List<int>{
                16,32,48
            };
            bool ans = false;
            for(int i = 0;i < allCollectBadgeID.Count;i++){
                if(_data.badge.Count < allCollectBadgeID[i])break;
                if(_data.badge[allCollectBadgeID[i] - 1] == allCollectBadgeID[i] - 1){
                    _data.badge.Add(allCollectBadgeID[i]);
                    ans = true;
                }
            }
            return ans;
        }

        bool new_badge = false;
        bool f_badge = FirstBadge();
        bool l_badge = LevelBadge();
        bool qc_badge = QuestionCountBadge();
        bool log_badge = LoginBadge();
        _data.badge.Sort();
        bool AC_badge = AllCollectBadge();
        new_badge = f_badge || l_badge || qc_badge || log_badge || AC_badge;
        if(new_badge){
            StartCoroutine(NewBadgeNotion());
        }
        GameDirector.SetUserData(_data);
        return true;
    }
}
