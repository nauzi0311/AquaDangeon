using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JsonSerializable{
    public static T Deserialize<T>(string json){
        Debug.Log(json);
        return JsonUtility.FromJson<T>(json);
    }
    public static string Serialize<T>(T _data){
        return JsonUtility.ToJson(_data);
    }
}

[System.Serializable]
public class VersionData : JsonSerializable{
    public string version;
    public string url;
}

[System.Serializable]
public class UserData: JsonSerializable{
    public int school_num;
    public int level;
    public int exp;
    public int point;
    public List<int> correct_id;
    public int correct_count;
    public List<int> badge;
    public List<string> date;
    public int equipment;
    public ConfigData config;
    public void SetData(UserData _data){
        this.school_num = _data.school_num;
        this.level = _data.level;
        this.exp = _data.exp;
        this.point = _data.point;
        this.correct_id = _data.correct_id;
        this.correct_count = _data.correct_count;
        this.badge = _data.badge;
        this.date = _data.date;
        this.equipment = _data.equipment;
        this.config = _data.config;
    }
    public void SetFirstData(int _s_num){
        this.school_num = _s_num;
        this.level = 1;
        this.exp = 0;
        this.point = 0;
        this.correct_id = new List<int>();
        this.correct_count = 0;
        this.badge = new List<int>();
        this.date = new List<string>();
        this.equipment = -1;
    }
}

[System.Serializable]
public class DeviceData : JsonSerializable{
    public string device;
    public DeviceData(string _d){
        this.device = _d;
    }
}

[System.Serializable]
public class SignUpData : JsonSerializable{
    public string device;
    public string username;
    public string id;
    public int school_num;
    public SignUpData(string _d,string _u,string _p,int _s){
        this.device = _d;
        this.username = _u;
        this.id = _p;
        this.school_num = _s;
    }
}

[System.Serializable]
public class RequestData : JsonSerializable{
    public string device;
    public string course;
    public int times;
    public RequestData(string _d,string _c,int _t){
        device = _d;
        course = _c;
        times = _t;
    }
}

[System.Serializable]
public class QuestionData : JsonSerializable{
    public int id;
    public string title;
    public string question;
    public string source;
    public string output;
    public List<string> choice;
    public int answer;
    public int exp;
    public int point;
    public int rank;
    public void SetData(QuestionData _data){
        id = _data.id;
        title = _data.title;
        question = _data.question;
        source = _data.source;
        output = _data.output;
        choice = _data.choice;
        answer = _data.answer;
        exp = _data.exp;
        point = _data.point;
        rank = _data.rank;
    }
}

[System.Serializable]
public class QuestionDataSet : JsonSerializable{
    public List<QuestionData> quest;
    public List<bool> question_used;
    public void SetData(QuestionDataSet _data){
        quest = _data.quest;
        question_used = new List<bool>();
        for(int i = 0; i < quest.Count;i++){
            question_used.Add(false);
        }
    }
}

[System.Serializable]
public class CalenderData : JsonSerializable{
    public string device;
    public string date;
    public CalenderData(string _d,string _date){
        device = _d;
        date = _date;
    }
}

[System.Serializable]
public class DetailData : JsonSerializable{
    public string device;
    public int id;
    public DetailData(string _d,int _id){
        device = _d;
        id = _id;
    }
}

[System.Serializable]
public class ConfigData : JsonSerializable{
    public List<int> level;
}

[System.Serializable]
public class UserRankData : JsonSerializable{
    public string device;
    public string username;
    public int level;
    public int exp;
    public int equipment;
}

[System.Serializable]
public class RankingData : JsonSerializable{
    public List<UserRankData> ranking;
    public int count;
    public void SetData(RankingData _data){
        ranking = _data.ranking;
        count = _data.count;
    }
    public void View(){
        for(int i = 0; i < ranking.Count; i++){
            Debug.Log("device:" + ranking[i].device + 
            "\nusername:" + ranking[i].username);
        }
    }
}

[System.Serializable]
public class EquipData : JsonSerializable{
    public string device;
    public int equipment;
}

[System.Serializable]
public class ResultData : JsonSerializable{
    public string device;
    public int level;
    public int exp;
    public int point;
    public List<int> id_list;
    public List<bool> correct_list;
    public List<int> second_list;
    public List<int> user_answer;
    public List<int> badge;

    public void SetData(string _d,int _l,int _e,int _p,List<int> _id_l,List<bool> _co_l,List<int> _s_l,List<int> _u_a,List<int> _b){
        this.device = _d;
        this.level = _l;
        this.exp = _e;
        this.point = _p;
        this.id_list = _id_l;
        this.correct_list = _co_l;
        this.second_list = _s_l;
        this.user_answer = _u_a;
        this.badge = _b;
    }
}

[System.Serializable]
public class ScoreData : JsonSerializable{
    public List<int> id_list;
    public List<string> title_list;
    public void SetData(ScoreData _data){
        id_list = _data.id_list;
        title_list = _data.title_list;
    }
}

[System.Serializable]
public class RecommendData : JsonSerializable{
    public string device;
    public string course;
    public int times;
    public int rank;
    public string recommend;
}