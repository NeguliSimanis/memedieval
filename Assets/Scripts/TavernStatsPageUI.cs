using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernStatsPageUI : MonoBehaviour {

    public GameObject ChampSelectButtonPrefab;
    public GameObject ChampSelect;
    private List<GameObject> _champButtons = new List<GameObject>();
    public Champion _activeChamp;

    public Image avatar;
    public Text ChampionName;
    public Text ChampionLvl;
    public Text ClassName;

    public GameObject SkillList;
    public GameObject SkillItem;
    public Text BioText;
    public Text Motto;
    public GameObject[] UnitImage;

    public Text[] skillNumbers;
    public Button[] skillButtons;

    public Text skillPointsText;

    public void ChangeChamp(Champion c)
    {
        _activeChamp = c;
        var p = _activeChamp.properties.LoadPictureAsTexture2D();
        var rect = new Rect(0, 0, p.width, p.height);
        avatar.sprite = Sprite.Create(_activeChamp.properties.LoadPictureAsTexture2D(), rect, Vector2.zero);
        ChampionName.text = _activeChamp.properties.Name;
        ChampionLvl.text = _activeChamp.properties.level.ToString();
        switch(_activeChamp.properties.champClass)
        {
            case 0:
                ClassName.text = "PEASANT";
                UnitImage[1].SetActive(false);
                UnitImage[2].SetActive(false);
                UnitImage[0].SetActive(true);
                break;
            case 1:
                ClassName.text = "KNIGHT";
                UnitImage[1].SetActive(true);
                UnitImage[2].SetActive(false);
                UnitImage[0].SetActive(false);
                break;
            case 2:
                ClassName.text = "ARCHER";
                UnitImage[1].SetActive(false);
                UnitImage[2].SetActive(true);
                UnitImage[0].SetActive(false);
                break;
        }
        BioText.text = _activeChamp.properties.bio;
        Motto.text = _activeChamp.properties.quote;
        UpdateSkillNumbers();
    }

    private void OnEnable()
    {
        var p = PlayerProfile.Singleton;
        for (int i = 0; i < p.champions.Count; i++)
        {
            //hack

            var champ = p.champions[i];
            ChangeChamp(champ);
            UnLoadSkills();
            loadSkills();
            var buttonp = Instantiate(ChampSelectButtonPrefab);
            var button = buttonp.GetComponent<Image>();
            button.gameObject.SetActive(true);
            button.transform.SetParent(ChampSelect.transform);
            var p1 = _activeChamp.properties.LoadPictureAsTexture2D();          
            var rect = new Rect(0, 0, p1.width, p1.height);
            button.sprite= Sprite.Create(champ.properties.LoadPictureAsTexture2D(), rect, Vector2.zero);
            button.GetComponentInChildren<Text>().text = champ.properties.Name;
            buttonp.GetComponent<Button>().onClick.AddListener(() => { ChangeChamp(champ); });
            _champButtons.Add(button.gameObject);
        }
        
    }
    private void OnDisable()
    {
        foreach (var b in _champButtons)
            Destroy(b);
    }
    private void loadSkills()
    {
        var ChampSkills = _activeChamp.GetComponentInChildren<StatsContainer>();
        for(int i=0;i<ChampSkills.stats.Count;i++)
        {
            var skill = ChampSkills.stats[i];
            var item = Instantiate(SkillItem);
            SkillItemList.Add(item.gameObject);
            var list=item.GetComponentsInChildren<Text>();
            list[0].text = skill.name;
            list[1].text = skill.value.ToString();
            item.transform.SetParent(SkillList.transform);
            item.gameObject.SetActive(true);

                var b = item.gameObject.GetComponentInChildren<Button>();
                b.onClick.AddListener(() => {
                if (_activeChamp.properties.skillpoints > 0)
                {
                    skill.value++; _activeChamp.properties.skillpoints--;
                    list[1].text=skill.value.ToString();
                }});
            
        }
    }
    public List<GameObject> SkillItemList = new List<GameObject>();
    private void UnLoadSkills()
    {
        foreach (var game in SkillItemList)
        {
            GameObject.Destroy(game);
        }
    }

    public void UpdateSkillNumbers()
    {
        var skills = _activeChamp.GetComponentInChildren<StatsContainer>().stats;
        skillPointsText.text = _activeChamp.properties.skillpoints + " skillpoints";
        for(int i = 0; i < 6; i++)
        {
            skillNumbers[i].text = skills[i].value.ToString();
        }

        if(_activeChamp.properties.skillpoints <= 0)
            foreach(var b in skillButtons)
                b.gameObject.SetActive(false);
        else
            foreach (var b in skillButtons)
                b.gameObject.SetActive(true);
    }

    public void AddSkillPoint(int skillIndex)
    {
        if (_activeChamp.properties.skillpoints > 0)
        {
            var skills = _activeChamp.GetComponentInChildren<StatsContainer>();
            skills.stats[skillIndex].value++;
            _activeChamp.properties.skillpoints--;
            //skillText.text = skills.stats[skillIndex].value.ToString();
        }
        UpdateSkillNumbers();
    }
}
