using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingSceneManager : Manager
{
    RankingData data;
    Vector3 ListItemPos = new Vector3(0,-100,0);
    Vector3 ListItemScale = new Vector3(1,1,1);
    [SerializeField]
    GameObject PrefabRankingList;
    [SerializeField]
    GameObject ListArea;
    [SerializeField]
    GameObject UserRank;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetComponent<GameDirector>().FadeOut();
        data = GameDirector.GetRankingData();
        UserRank.GetComponent<TextMeshProUGUI>().text = data.count.ToString();
        GenerateRankingList(data);
    }  

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackButtonOnClick(){
        string NextSceneName = GameDirector.GetPreSceneName();
        Director.GetComponent<GameDirector>().FadeMove(NextSceneName);
    }
    void GenerateRankingList(RankingData _data){
        Transform Area = ListArea.transform;
        int _count = data.ranking.Count;
        for(int i = 0; i < data.ranking.Count;i++){
            var obj = Instantiate(PrefabRankingList);
            Transform _trans = obj.transform;
            _trans.SetParent(Area);
            _trans.localPosition = ListItemPos;
            _trans.localScale = ListItemScale;
            ListItemPos += new Vector3(0,-200,0);
            ChangeIcon(_trans, _data.ranking[i]);
            ChangeRank(_trans, i);
            ChangeOrdinal(_trans,i);
            ChangeUserName(_trans, _data.ranking[i]);
        }
        Vector2 _now_size = ListArea.GetComponent<RectTransform>().sizeDelta;
        ListArea.GetComponent<RectTransform>().sizeDelta = new Vector2(_now_size.x,200*(_count));
    }
    void ChangeUserName(Transform _t,UserRankData _d){
        _t.Find("UserName").gameObject.GetComponent<TextMeshProUGUI>().text =_d.username;
    }
    void ChangeIcon(Transform _t,UserRankData _d){
        string dir = "Badge/" + _d.equipment.ToString();
        Sprite equip_image = Resources.Load<Sprite>(dir);
        _t.Find("BadgeIcon/Mask/Icon").gameObject.GetComponent<Image>().sprite = equip_image;
    }
    void ChangeRank(Transform _t,int i){
        string[] color = {"<color=#EFA621>","<color=#938F89>","<color=#FF6F00>"};
        if(i <= 2){
            _t.Find("RankItem/Rank").gameObject.GetComponent<TextMeshProUGUI>().text = color[i] + (i + 1).ToString() + "</color>";
        }else{
            _t.Find("RankItem/Rank").gameObject.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
        }
    }
    void ChangeOrdinal(Transform _t,int i){
        string ordinal = "th";
        if((i % 10) <= 3 && (i % 10) >= 1){
            if((i/10) != 1){
                switch (i % 10){
                    case 1:
                        ordinal = "st";
                        break;
                    case 2:
                        ordinal = "nd";
                        break;
                    case 3:
                        ordinal = "rd";
                        break;
                    default:
                        ordinal = "th";
                        break;
                }
            }
        }
        _t.Find("RankItem/Ordinal").gameObject.GetComponent<TextMeshProUGUI>().text = ordinal;
    }
}
