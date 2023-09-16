using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BadgeSceneManager : Manager
{
    [SerializeField]
    Canvas DetailCanvas;
    [SerializeField]
    GameObject DetailBadge;
    [SerializeField]
    TextMeshProUGUI DetailCondition;
    [SerializeField]
    GameObject BadgeArea;
    [SerializeField]
    GameObject PrefabBadgeItem;
    Vector3 ListItemPos = new Vector3(0,-160,0);
    Vector3 ListItemScale = new Vector3(1,1,1);
    int badge_equip_num = 0;
    const int BADGE_COUNT = 49;
    // Start is called before the first frame update
    void Start()
    {
        Director.GetComponent<GameDirector>().FadeOut();
        GenerateBadgeList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackButtonOnClick(){
        Director.GetComponent<GameDirector>().FadeMove(GameDirector.GetPreSceneName());
    }
    public void DetailBackButtonOnClick(){
        DetailCanvas.sortingOrder = -1;
        badge_equip_num = 0;
    }
    public void DetailEquipButtonOnClick(){
        UserData _userData = GameDirector.GetUserData();
        _userData.equipment = badge_equip_num;
        GameDirector.SetUserData(_userData);
        EquipData _data = new EquipData();
        _data.device = PlayerPrefs.GetString("UUID");
        _data.equipment = badge_equip_num;
        string json = EquipData.Serialize<EquipData>(_data);
        StartCoroutine(GameDirector.WebRequestPOST("rank/equip/",json));
    }
    public void ChangeDetail(int badge_num){
        List<int> _userBadge = GameDirector.GetUserData().badge;
        string dir = "Badge/" + badge_num.ToString();
        Sprite detailBadge = Resources.Load<Sprite>(dir);
        if(_userBadge.Contains(badge_num)){
            DetailBadge.GetComponent<Image>().sprite = detailBadge;
            Color _c = DetailBadge.transform.parent.Find("Cover").gameObject.GetComponent<Image>().color;
            _c.a = 0.0f;
            DetailBadge.transform.parent.Find("Cover").gameObject.GetComponent<Image>().color = _c;
            DetailCondition.text = badge_conditions[badge_num];
            badge_equip_num = badge_num;
        }else{
            Color _c = DetailBadge.transform.parent.Find("Cover").gameObject.GetComponent<Image>().color;
            _c.a = 1.0f;
            DetailBadge.transform.parent.Find("Cover").gameObject.GetComponent<Image>().color = _c;
            DetailCondition.text = badge_conditions[badge_num];
        }
        DetailCanvas.sortingOrder = 3;
    }
    public void GenerateBadgeList(){
        List<int> _userBadge = GameDirector.GetUserData().badge;
        Vector3 _deltaY = new Vector3(0,-340,0);
        Vector3 _deltaX = new Vector3(320,0,0);
        for(int i = 0; i < BADGE_COUNT;i++){
            var obj = Instantiate(PrefabBadgeItem);
            Transform _trans = obj.transform;
            _trans.SetParent(BadgeArea.transform);
            RectTransform _rect = obj.GetComponent<RectTransform>();
            _rect.anchorMax = new Vector2(.5f,1.0f);
            _rect.anchorMin = new Vector2(.5f,1.0f);
            if((i % 16) == 0){
                _trans.localPosition = ListItemPos;
                ListItemPos += _deltaY;
            }else{
                switch((i % 16) % 3){
                    case 1:
                        _trans.localPosition = ListItemPos - _deltaX;
                        break;
                    case 2:
                        _trans.localPosition = ListItemPos;
                        break;
                    case 0:
                        _trans.localPosition = ListItemPos + _deltaX;
                        ListItemPos += _deltaY;
                        break;
                    default:
                        Debug.Log("Error");
                        break;
                }
            }
            _trans.localScale = ListItemScale;
            string dir = "Badge/" + i.ToString(); 
            Sprite _sprite = Resources.Load<Sprite>(dir);
            _trans.Find("Mask/Item/Badge").gameObject.GetComponent<Image>().sprite = _sprite;
            Color _color = _trans.Find("Mask/Item/Cover").gameObject.GetComponent<Image>().color;
            if(_userBadge.Contains(i)){
                _color.a = 0.0f;
            }else{
                _color.a = 1.0f;
            }
            _trans.Find("Mask/Item/Cover").gameObject.GetComponent<Image>().color = _color;
            obj.GetComponent<BadgeItem>().SetBadgeNum(i);
        }
        Vector2 _now_size = BadgeArea.GetComponent<RectTransform>().sizeDelta;
        BadgeArea.GetComponent<RectTransform>().sizeDelta = new Vector2(_now_size.x,1610*((BADGE_COUNT-1)/16 + 1));
    }
    List<string> badge_conditions = new List<string>(){
        "ソフトウェア演習1の問題を7種類解く",//0
        "Level 2になる",
        "2日問題を解く",
        "5問正解する",

        "Level 3になる",
        "3日問題を解く",
        "10問正解する",

        "Level 4になる",
        "4日問題を解く",
        "15問正解する",

        "Level 5になる",
        "5日問題を解く",
        "25問正解する",

        "Level 7になる",
        "7日問題を解く",
        "30問正解する",

        "これより手前のバッジを全て獲得する",

        "Level 9になる",
        "9日問題を解く",
        "45問正解する",

        "Level 11になる",
        "11日問題を解く",
        "60問正解する",

        "Level 13になる",
        "13日問題を解く",
        "75問正解する",

        "Level 15になる",
        "15日問題を解く",
        "90問正解する",

        "Level 17になる",
        "17日問題を解く",
        "110問正解する",

        "これより手前のバッジを全て獲得する",

        "Level 20になる",
        "21日問題を解く",
        "130問正解する",

        "Level 25になる",
        "25日題を解く",
        "150問正解する",

        "Level 30になる",
        "30日問題を解く",
        "170問正解する",

        "Level 35になる",
        "35日問題を解く",
        "190問正解する",

        "Level 40になる",
        "40日問題を解く",
        "210問正解する",

        "これより手前のバッジを全て獲得する",
    };
}


