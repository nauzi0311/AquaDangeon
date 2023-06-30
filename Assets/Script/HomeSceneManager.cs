using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HomeSceneManager : Manager
{
    [SerializeField]
    EventSystem eventSystem;
    [SerializeField]
    GameObject CalenderCanvas,Calender;
    [SerializeField]
    TextMeshProUGUI Month,Year;
    string[] MonthNames = {"","Jan", "Feb", "Mar", "Apr", "May", "Jun","Jul","Aug","Sep","Oct","Nov","Dec"};
    int month,year;
    // Start is called before the first frame update
    void Start()
    {
        year = DateTime.Now.Year;
        month = DateTime.Now.Month;
        CalenderCanvas = GameObject.Find("CalenderCanvas");
        Calender = GameObject.Find("Calender");
        InvisibleCalender();
        Director.GetComponent<GameDirector>().FadeOut();
        CalenderInitialize();
        GameDirector.ResetAllData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CalenderInitialize(){
        WriteDay();
        WriteCheck();
        MonthChange();
        YearChange();
    }
    public void ScoreButtonOnClick(){
        StartCoroutine(ScoreProcess());
    }
    public void BadgeButtonOnClick(){
        Director.GetComponent<GameDirector>().FadeMove("BadgeScene","HomeScene");
    }
    public void RankingButtonOnClick(){
        StartCoroutine(RankingProcess());
    }
    public void CalenderButtonOnClick(){
        var obj = eventSystem.currentSelectedGameObject;
        TextMeshProUGUI _TMpro = obj.transform.Find("Day").GetComponent<TextMeshProUGUI>();
        Color _color = _TMpro.color;
        if(_color.a != 1.0f){
            return;
        }
        string day = int.Parse(_TMpro.text).ToString("00");
        string month = Array.IndexOf(MonthNames,Month.text).ToString("00");
        string year = Year.text;
        string date = year + "-" + month + "-" + day;
        Debug.Log(date);
        CalenderData _data = new CalenderData(PlayerPrefs.GetString("UUID"),date);
        string json = CalenderData.Serialize<CalenderData>(_data);
        StartCoroutine(GameDirector.WebRequestPOST("calender",json));
    }
    public IEnumerator ScoreProcess(){
        string json = "{\"device\":\"" + PlayerPrefs.GetString("UUID") + "\"," +
        "\"level\":" + GameDirector.GetUserData().level + "}";
        yield return StartCoroutine(GameDirector.WebRequestPOST("score",json));
        Debug.Log(GameDirector.GetResponse());
        GameDirector.SetScoreData(ScoreData.Deserialize<ScoreData>(GameDirector.GetResponse()));
        Director.GetComponent<GameDirector>().FadeMove("ScoreScene","HomeScene");
    }
    public IEnumerator RankingProcess(){
        string json ="{\"device\":\"" + PlayerPrefs.GetString("UUID") + "\"}";
        yield return StartCoroutine(GameDirector.WebRequestPOST("rank",json));
        Debug.Log(GameDirector.GetResponse());
        GameDirector.SetRankingData(RankingData.Deserialize<RankingData>(GameDirector.GetResponse()));
        Director.GetComponent<GameDirector>().FadeMove("RankingScene","HomeScene");
    }
    //Calender Process
    public void MonthChange(){
        Month.text = MonthNames[month];
    }
    public void YearChange(){
        Year.text = year.ToString();
    }
    public void WriteDay(){
        GameObject Main = Calender.transform.Find("Main").gameObject;
        DateTime FirstDay = new DateTime(year,month,1);
        int FirstDayOfWeek = (int)FirstDay.DayOfWeek;
        int LastDayOfWeek;
        if(month == 12){
            LastDayOfWeek = (int)new DateTime(year + 1,1,1).AddDays(-1).DayOfWeek;
        }else{
            LastDayOfWeek = (int)new DateTime(year,month+ 1,1).AddDays(-1).DayOfWeek;
        }
        for(int i = 1;i <= 6;i++){
            string FindName = "Week" + i;
            GameObject Week = Main.transform.Find(FindName).gameObject;
            for(int j = 0;j < 7;j++){
                Transform Image = Week.transform.GetChild(j).Find("Image");
                Image.gameObject.SetActive(false);
                GameObject Day = Week.transform.GetChild(j).Find("Day").gameObject;
                Day.GetComponent<TextMeshProUGUI>().SetText(FirstDay.AddDays((i-1)*7+j-FirstDayOfWeek).Day.ToString());
                Day.GetComponent<TextMeshProUGUI>().alpha = 1.0f;
                if(i == 1 && j < FirstDayOfWeek){
                    Day.GetComponent<TextMeshProUGUI>().alpha = 0.4f;
                }
                if(FirstDayOfWeek - LastDayOfWeek >= 4){
                    if(i == 6 && LastDayOfWeek < j){
                        Day.GetComponent<TextMeshProUGUI>().alpha = 0.4f;
                    }
                }else{
                    if(i == 5 && LastDayOfWeek < j){
                        Day.GetComponent<TextMeshProUGUI>().alpha = 0.4f;
                    }
                    if(i == 6){
                        Day.GetComponent<TextMeshProUGUI>().alpha = 0.4f;
                    }
                }
            }
        }
    }
    public void NextMonth(){
        month += 1;
        if(month > 12){
            year += 1;
            month -= 12;
            YearChange();
        }
        WriteDay();
        WriteCheck();
        MonthChange();
    }
    public void PreviousMonth(){
        month -= 1;
        if(month < 1){
            year -= 1;
            month += 12;
            YearChange();
        }
        WriteDay();
        WriteCheck();
        MonthChange();
    }
    public void VisibleCalender(){
        CalenderCanvas.GetComponent<Canvas>().sortingOrder = 2;
        CalenderData _data = new CalenderData(PlayerPrefs.GetString("UUID"),"Calender Button Clicked");
        string json = CalenderData.Serialize<CalenderData>(_data);
        StartCoroutine(GameDirector.WebRequestPOST("calender",json));
    }
    public void InvisibleCalender(){
        CalenderCanvas.GetComponent<Canvas>().sortingOrder = -1;
    }
    void WriteCheck(){
        UserData _userData = GameDirector.GetUserData();
        GameObject Main = Calender.transform.Find("Main").gameObject;
        string date_until_month = year.ToString() + "-" + month.ToString("00") + "-";
        for(int i = 1;i <= 6;i++){
            string FindName = "Week" + i;
            GameObject Week = Main.transform.Find(FindName).gameObject;
            for(int j = 0;j < 7;j++){
                GameObject Day = Week.transform.GetChild(j).Find("Day").gameObject;
                Debug.Log("i:" + i + " j:" + j + " alpha:" + Day.GetComponent<TextMeshProUGUI>().alpha);
                if(Day.GetComponent<TextMeshProUGUI>().alpha != 1.0){
                    Transform Image = Week.transform.GetChild(j).Find("Image");
                    Image.gameObject.SetActive(false);
                    continue;
                }
                string check = date_until_month + int.Parse(Day.GetComponent<TextMeshProUGUI>().text).ToString("00");
                if(_userData.date.Contains(check)){
                    Transform Image = Week.transform.GetChild(j).Find("Image");
                    Image.gameObject.SetActive(true);
                }
            }
        }
    }
}
