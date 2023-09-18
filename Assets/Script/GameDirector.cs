using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameDirector : MonoBehaviour
{
    [SerializeField]
    public Fade fade;
    private float _fade_time = 1f;
    //private static string URL = "http://localhost:3000/soft1/";
    private static string URL = "https://se.is.kit.ac.jp/beakfish/api/soft1/";
    private static UserData userdata = new UserData();
    private static ScoreData list = new ScoreData();
    private static RankingData ranking = new RankingData();
    private static ConfigData config = new ConfigData();
    private static string response;
    private static string PreSceneName;
    //NEED RESET
    private static QuestionData detail = new QuestionData();
    private static QuestionDataSet questions = new QuestionDataSet();
    private static bool IsQuestion = false,IsDetail = false;
    private static int QuestionCount = 0;
    private static int now_rank = 3;
    private static int now_times = 0;
    private static bool Is_update = true;
    public static List<int> id_list = new List<int>();
    public static List<bool> correct_list = new List<bool>();
    public static List<int> second_list = new List<int>();
    public static List<int> user_answer = new List<int>();
    public static List<int> exp_list = new List<int>();
    public static List<int> point_list = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Loading Methods
    public static void OneRankUP(){
        now_rank++;
    }
    public static void OneRankDown(){
        if(now_rank > 1){
            now_rank--;
        }
    }
    public static int GetRank(){
        return now_rank;
    }
    public static int GetTimes(){
        return now_times;
    }
    public static void SetTimes(int _time){
        now_times = _time;
    }
    //Index Methods
    public static void SetConfigData(ConfigData _data){
        config = _data;
    }
    public static ConfigData GetConfigData(){
        return config;
    }
    public static void SetUserData(UserData _data){
        userdata.SetData(_data);
    }
    public static UserData GetUserData(){
        return userdata;
    }
    //Question Methods
    public static void SetQuestionDataSet(QuestionDataSet _data){
        questions.SetData(_data);
        IsQuestion = true;
    }
    public static QuestionDataSet GetQuestionDataSet(){
        return questions;
    }
    public static int GetQuestionsCount(){
        return QuestionCount;
    }
    public static QuestionData GetQuestionData(){
        for(int i = 0; i < questions.quest.Count-1;i++){
            if(questions.quest[i].rank == now_rank && !questions.question_used[i]){
                questions.question_used[i] = true;
                QuestionCount++;
                return questions.quest[i];
            }
        }
        Debug.Log("QuestionCount: " + QuestionCount);
        if(QuestionCount == 6){
            if(now_rank == 6){
                questions.question_used[questions.quest.Count -1] = true;
                QuestionCount++;
                return questions.quest[questions.quest.Count-1];
            }else{
                if(now_rank == 0)now_rank = 1;
                for(int i = 0; i < questions.quest.Count-1;i++){
                    if(questions.quest[i].rank == now_rank){
                        questions.question_used[i] = true;
                        QuestionCount++;
                        return questions.quest[i];
                    }
                }
            }
        }
        Debug.Log("No Question");
        return null;
    }
    public static bool IsQuestionReady(){
        return IsQuestion;
    }
    //Score Methods
    public static void SetScoreData(ScoreData _data){
        list.SetData(_data);
    }
    public static void ResetDetailData(){
        detail = new QuestionData();
        IsDetail = false;
    }
    public static ScoreData GetScoreData(){
        return list;
    }
    //Ranking Methods
    public static void SetRankingData(RankingData _data){
        ranking.SetData(_data);
    }
    public static RankingData GetRankingData(){
        return ranking;
    }
    //Detail Methods
    public static void SetDetailData(QuestionData _data){
        detail.SetData(_data);
        IsDetail = true;
    }
    public static QuestionData GetDetailData(){
        return detail;
    }
    public static bool IsDetailReady(){
        return IsDetail;
    }
    //Result Methods
    public static void AddResultData(int id,bool is_correct,int second,int u_answer,int exp,int point) {
        id_list.Add(id);
        correct_list.Add(is_correct);
        second_list.Add(second);
        user_answer.Add(u_answer);
        exp_list.Add(exp);
        point_list.Add(point);
        Debug.Log("id_list: " + string.Join(",",id_list.Select(n => n.ToString())) +
        "\ncorrect_list: " + string.Join(",",correct_list.Select(n => n.ToString())) +
        "\nuser_answer: " + string.Join(",",user_answer.Select(n => n.ToString())) +
        "\nused_question : " + string.Join(",",questions.question_used.Select(n => n.ToString())) + 
        "\nexp_list : " + string.Join(",",exp_list.Select(n => n.ToString())) + 
        "\npoint_list : " + string.Join(",",point_list.Select(n => n.ToString())));
    }
    public static bool GetIs_update(){
        return Is_update;
    }
    public static void SetIs_update(){
        Is_update = false;
    }
    //Home Methods
    public static void ResetAllData(){
        questions = new QuestionDataSet();
        detail = new QuestionData();
        IsQuestion = false;
        IsDetail = false;
        QuestionCount = 0;
        now_rank = 3;
        now_times = 0;
        id_list = new List<int>();
        correct_list = new List<bool>();
        second_list = new List<int>();
        user_answer = new List<int>();
        exp_list = new List<int>();
        point_list = new List<int>();
        Is_update = true;
    }
    //General Methods
    public static string GetPreSceneName(){
        return PreSceneName;
    }
    public static string GetResponse(){
        return response;
    }
    public void FadeOut(){
        Invoke("fadeout",0.1f);
    }
    private void fadeout(){
        fade.FadeOut(_fade_time,() =>{});
    }
    public void FadeMove(string NextSceneName,string NowSceneName = null){
        fade.FadeIn(_fade_time,() => {
            StartCoroutine(SceneMove(NextSceneName,NowSceneName));
        });
    }
    public static IEnumerator SceneMove(string NextSceneName,string NowSceneName = null){
        if(NowSceneName != null) PreSceneName = NowSceneName;
        yield return SceneManager.LoadSceneAsync(NextSceneName);
    }
    public static IEnumerator WebRequestGET(string uri){
        using UnityWebRequest req = new UnityWebRequest();
        req.method = UnityWebRequest.kHttpVerbGET;
        req.url = URL + uri;

        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.ConnectionError ||
            req.result == UnityWebRequest.Result.DataProcessingError ||
            req.result == UnityWebRequest.Result.ProtocolError){
            Debug.Log(req.error);
        }else{
            response = req.downloadHandler.text;
        }
    }
    public static IEnumerator WebRequestPOST(string uri,string json){
        using UnityWebRequest req = new UnityWebRequest();
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.url = URL + uri;
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.ConnectionError ||
            req.result == UnityWebRequest.Result.DataProcessingError ||
            req.result == UnityWebRequest.Result.ProtocolError){
            Debug.Log(req.error);
        }else{
            response = req.downloadHandler.text;
        }
    }


}
