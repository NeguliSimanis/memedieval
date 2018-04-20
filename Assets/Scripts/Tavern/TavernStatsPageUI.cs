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
    public Text ClassName;

    [Header("Experience")]
    [SerializeField] Image championExpBar;
    [SerializeField] Text championExpText;

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
        switch(_activeChamp.properties.champClass)
        {
            case 0:
                ClassName.text = "Level " + (c.properties.level+1) + " Peasant";
                UnitImage[1].SetActive(false);
                UnitImage[2].SetActive(false);
                UnitImage[0].SetActive(true);
                break;
            case 1:
                ClassName.text = "Level " + (c.properties.level + 1) + " Knight";
                UnitImage[1].SetActive(true);
                UnitImage[2].SetActive(false);
                UnitImage[0].SetActive(false);
                break;  
            case 2:
                ClassName.text = "Level " + (c.properties.level + 1) + " Archer";
                UnitImage[1].SetActive(false);
                UnitImage[2].SetActive(true);
                UnitImage[0].SetActive(false);
                break;
        }
        
        SetChampionFlairText();
        SetChampionExpBar();
        UpdateSkillNumbers();
        CheckUnspentSkillpoints();
    }

    private void SetChampionFlairText()
    {
        BioText.text = _activeChamp.properties.bio;
        Motto.text = _activeChamp.properties.quote;
    }

    private void SetChampionExpBar()
    {
        championExpBar.fillAmount = (float)_activeChamp.properties.currentExp / (float)_activeChamp.properties.nextLevelExp;
        championExpText.text = _activeChamp.properties.currentExp.ToString() + " / " + _activeChamp.properties.nextLevelExp.ToString() + " exp";
    }

    private void OnEnable()
    {
        var p = PlayerProfile.Singleton;
        for (int i = 0; i < p.champions.Count; i++)
        {
            var currentChampion = p.champions[i];
            ChangeChamp(currentChampion);
            UnLoadSkills();
            loadSkills();

            var buttonp = Instantiate(ChampSelectButtonPrefab);
            var button = buttonp.GetComponent<Image>();
            button.gameObject.SetActive(true);
            button.transform.SetParent(ChampSelect.transform);

            var p1 = _activeChamp.properties.LoadPictureAsTexture2D();          
            var rect = new Rect(0, 0, p1.width, p1.height);
            button.sprite= Sprite.Create(currentChampion.properties.LoadPictureAsTexture2D(), rect, Vector2.zero);
            button.GetComponentInChildren<Text>().text = currentChampion.properties.Name;
            buttonp.GetComponent<Button>().onClick.AddListener(() => { ChangeChamp(currentChampion); });
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
        for (int i = 0; i < ChampSkills.stats.Count; i++)
        {
            var skill = ChampSkills.stats[i];
            var item = Instantiate(SkillItem);
            SkillItemList.Add(item.gameObject);
            var list = item.GetComponentsInChildren<Text>();
            list[0].text = skill.name;
            list[1].text = skill.value.ToString();
            item.transform.SetParent(SkillList.transform);
            item.gameObject.SetActive(true);

            var b = item.gameObject.GetComponentInChildren<Button>();
            b.onClick.AddListener(() =>
            {
                if (_activeChamp.properties.skillpoints > 0)
                    if (_activeChamp.properties.skillpoints > 0)
                    {
                        skill.value++; _activeChamp.properties.skillpoints--;
                        skill.value++; _activeChamp.properties.skillpoints--;
                        list[1].text = skill.value.ToString();
                    }
            });
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
        // bad hack
        skillNumbers[0].text = _activeChamp.properties.charm.ToString();
        skillNumbers[1].text = _activeChamp.properties.discipline.ToString();
        skillNumbers[2].text = _activeChamp.properties.brawn.ToString();
        skillNumbers[3].text = _activeChamp.properties.wisdom.ToString();
        skillNumbers[4].text = _activeChamp.properties.luck.ToString();
        skillNumbers[5].text = _activeChamp.properties.wealth.ToString();
    }

    private void CheckUnspentSkillpoints()
    {
        if (_activeChamp.properties.skillpoints <= 0)
        {
            // disable adding skillpoints
            foreach (var b in skillButtons)
                b.gameObject.SetActive(false);

            // disable skillpoint text
            skillPointsText.enabled = false;
        }
        else
        {
            // enable adding skillpoints
            foreach (var b in skillButtons)
                b.gameObject.SetActive(true);

            // enable skillpoint text
            skillPointsText.enabled = true;

            if (_activeChamp.properties.skillpoints == 1)
                skillPointsText.text = _activeChamp.properties.skillpoints + " skillpoint";
            else
            skillPointsText.text = _activeChamp.properties.skillpoints + " skillpoints";
        }
    }

    public void AddSkillPoint(int skillIndex)
    {
        if (_activeChamp.properties.skillpoints > 0)
        {
            switch (skillIndex)
            {
                case 0: // charm
                    _activeChamp.properties.charm++;
                    break;
                case 1: // discipline
                    _activeChamp.properties.discipline++;
                    break;
                case 2: // brawn
                    _activeChamp.properties.brawn++;
                    break;
                case 3: // wisdom
                    _activeChamp.properties.wisdom++;
                    break;
                case 4: // luck
                    _activeChamp.properties.luck++;
                    break;
                case 5: // wealth
                    _activeChamp.properties.wealth++;
                    break;
            }

            var skills = _activeChamp.GetComponentInChildren<StatsContainer>();
            skills.stats[skillIndex].value++;
            _activeChamp.properties.skillpoints--;
            UpdateSkillNumbers();
            CheckUnspentSkillpoints();
        }
    }
}
