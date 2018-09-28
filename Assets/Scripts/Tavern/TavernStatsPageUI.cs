using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernStatsPageUI : MonoBehaviour {

    #region global variables
    public GameObject ChampSelectButtonPrefab;
    public GameObject ChampSelectPanel;
    private List<ChampionButton> selectChampionButtObjects = new List<ChampionButton>();
    public Champion activeChampion;

    public Image avatar;
    public Text ChampionName;
    public Text ClassName;

    [SerializeField] GameObject defaultStatsPanel;

    [Header("Champion head")]
    [SerializeField] GameObject cameraPictureContainer;
    [SerializeField] GameObject defaultPictureContainer;

    [Header("Firing")]
    [SerializeField] Button championFireButton;
    [SerializeField] GameObject championFirePopup;
    [SerializeField] Button confirmChampionFire;
    [SerializeField] Button cancelChampionFire;
    [SerializeField] Text championFireText;
    string championFireTextIntro = "Fire ";
    string questionMark = "?";

    [Header("Experience")]
    [SerializeField] Image championExpBar;
    [SerializeField] Text championExpText;

    [Header("Skills")]
    public GameObject SkillList;
    public GameObject SkillItem;

    public Text[] skillNumbers;
    public Button[] skillButtons;

    public Text skillPointsText;
    public List<GameObject> SkillItemList = new List<GameObject>();

    [Header("Abilities")]
    [SerializeField]
    private Text abilityText;

    [Header("Flair")]
    public Text BioText;
    public Text Motto;
    public GameObject[] UnitImage;

    [Header("Crest")]
    [SerializeField] Image championCrestImage;
    #endregion

    private void Start()
    {
        AddButtonListeners();
    }

    private void AddButtonListeners()
    {
        //championFireButton.onClick.AddListener(OpenChampion);
        confirmChampionFire.onClick.AddListener(FireChampion);
    }

    private void FireChampion()
    {
        // hide the fired champion button. 
        foreach (ChampionButton selectChampionButt in selectChampionButtObjects)
        {
            if (selectChampionButt.champion == activeChampion)
            {
                // note - all buttons will be removed when user exits stats page
                Destroy(selectChampionButt.buttonObject);
            }
        }

        // delete the champion
        activeChampion.DeleteChampion();
        DisableChampionFirePopup();

        // display another champion if available
        if (PlayerProfile.Singleton.champions.Count > 0)
        {
            activeChampion = PlayerProfile.Singleton.champions[0];
            ChangeChamp(activeChampion);
        }
        else
        {
            defaultStatsPanel.SetActive(true);
        }     
    }

    private void UpdateFireChampionText()
    {
        // e.g. Fire Ron Burgundy?
        championFireText.text = championFireTextIntro + activeChampion.properties.Name + questionMark;
    }

    private void UpdateChampionName()
    {
        ChampionName.text = activeChampion.properties.Name;
    }

    private void ChangeChampionHeadPicture()
    {
        // loads camera picture if possible
        if (activeChampion.properties.isCameraPicture == true)
        {
            cameraPictureContainer.SetActive(true);
            defaultPictureContainer.SetActive(false);
            var p = activeChampion.properties.LoadPictureAsTexture2D();
            var rect = new Rect(0, 0, p.width, p.height);
            avatar.sprite = Sprite.Create(activeChampion.properties.LoadPictureAsTexture2D(), rect, Vector2.zero);
        }
        // loads default picture
        else
        {
            cameraPictureContainer.SetActive(false);
            defaultPictureContainer.SetActive(true);
            ChampionPictureActivator.ActivateChampionPicture(defaultPictureContainer, activeChampion.properties.GetChampionClass());
        }
    }

    private void UpdateChampionCrest()
    {
        championCrestImage.color = activeChampion.properties.crestColor;
    }

    public void ChangeChamp(Champion c)
    {
        activeChampion = c;
        ChangeChampionHeadPicture();
        UpdateChampionName();
        UpdateFireChampionText();
        UpdateChampionCrest();

        switch (activeChampion.properties.champClass)
        {
            case 0:
                UnitImage[1].SetActive(false);
                UnitImage[2].SetActive(false); // archer
                UnitImage[0].SetActive(true);  // peasant
                break;
            case 1:              
                UnitImage[1].SetActive(true);
                UnitImage[2].SetActive(false); // archer
                UnitImage[0].SetActive(false);  // peasant
                break;  
            case 2:
                UnitImage[1].SetActive(false);
                UnitImage[2].SetActive(true); // archer
                UnitImage[0].SetActive(false);  // peasant
                break;
        }
        SetChampionClassText();
        SetChampionFlairText();
        SetChampionAbilityText();
        SetChampionExpBar();
        UpdateSkillNumbers();
        CheckUnspentSkillpoints();
    }

    private void SetChampionClassText()
    {
        // e.g Level 1
        ClassName.text = "Level " + (activeChampion.properties.level + 1);

        // e.g Level 1 Archer
        ClassName.text = ClassName.text + " " + activeChampion.properties.GetChampionClass();
    }

    private void SetChampionFlairText()
    {
        BioText.text = activeChampion.properties.bio;
        Motto.text = activeChampion.properties.quote;
    }

    private void SetChampionExpBar()
    {
        championExpBar.fillAmount = (float)activeChampion.properties.currentExp / (float)activeChampion.properties.nextLevelExp;
        championExpText.text = activeChampion.properties.currentExp.ToString() + " / " + activeChampion.properties.nextLevelExp.ToString() + " exp";
    }

    private void InstantiateChampionButton(Champion currentChampion)
    {
        var buttonPrefab = Instantiate(ChampSelectButtonPrefab, ChampSelectPanel.transform);
        var buttonImage = buttonPrefab.GetComponent<Image>();
        buttonImage.gameObject.SetActive(true);
        //buttonImage.transform.SetParent(ChampSelectPanel.transform);

        // load picture taken by camera
        if (currentChampion.properties.isCameraPicture == true)
        {
            var p1 = activeChampion.properties.LoadPictureAsTexture2D();
            var rect = new Rect(0, 0, p1.width, p1.height);
            buttonImage.sprite = Sprite.Create(currentChampion.properties.LoadPictureAsTexture2D(), rect, Vector2.zero);
        }
        // load default picture
        else
        {
            ChampionPictureActivator.ActivateChampionPicture(buttonPrefab, currentChampion.GetClassName());
        }

        // load champion name
        buttonImage.GetComponentInChildren<Text>().text = currentChampion.properties.Name;

        // add button listener
        buttonPrefab.GetComponent<Button>().onClick.AddListener(() => { ChangeChamp(currentChampion); });

        buttonImage.gameObject.name = currentChampion.properties.Name;
        ChampionButton newChampionButton = new ChampionButton(currentChampion, buttonPrefab);
        selectChampionButtObjects.Add(newChampionButton);
    }

    private void OnEnable()
    {
        DisableChampionFirePopup();

        var p = PlayerProfile.Singleton;

        // player has no champions - display default window
        if (p.champions.Count == 0)
        {
            defaultStatsPanel.SetActive(true);
            return;
        }
        else
            defaultStatsPanel.SetActive(false);

        // unload all champion information
        for (int i = 0; i < p.champions.Count; i++)
        {
            var currentChampion = p.champions[i];
            ChangeChamp(currentChampion); // TO-DO: this should be executed only once per loop
            UnLoadSkills();
            LoadSkills();
            InstantiateChampionButton(currentChampion);
        }
        
    }
    private void OnDisable()
    {
        DisableChampionFirePopup();
        if (PlayerProfile.Singleton.champions.Count == 0)
            return;

        // delete all champion buttons
        // NB - this is ineffective and might cause memory leak, should be changed
        foreach (ChampionButton championButton in selectChampionButtObjects)
        {
            championButton.DestroyButtonObject();
        }
        selectChampionButtObjects.Clear();
    }

    private void DisableChampionFirePopup()
    {
        championFirePopup.SetActive(false);
    }

    private void LoadSkills()
    {

        var ChampSkills = activeChampion.GetComponentInChildren<StatsContainer>();
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
                if (activeChampion.properties.skillpoints > 0)
                    if (activeChampion.properties.skillpoints > 0)
                    {
                        skill.value++; activeChampion.properties.skillpoints--;
                        skill.value++; activeChampion.properties.skillpoints--;
                        list[1].text = skill.value.ToString();
                    }
            });
        }
    }

    
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
        skillNumbers[0].text = activeChampion.properties.charm.ToString();
        skillNumbers[1].text = activeChampion.properties.discipline.ToString();
        skillNumbers[2].text = activeChampion.properties.brawn.ToString();
        skillNumbers[3].text = activeChampion.properties.wisdom.ToString();
        skillNumbers[4].text = activeChampion.properties.luck.ToString();
        skillNumbers[5].text = activeChampion.properties.wealth.ToString();
    }

    private void CheckUnspentSkillpoints()
    {
        if (activeChampion.properties.skillpoints <= 0)
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

            if (activeChampion.properties.skillpoints == 1)
                skillPointsText.text = activeChampion.properties.skillpoints + " skillpoint";
            else
            skillPointsText.text = activeChampion.properties.skillpoints + " skillpoints";
        }
    }

    public void AddSkillPoint(int skillIndex)
    {
        if (activeChampion.properties.skillpoints > 0)
        {
            switch (skillIndex)
            {
                case 0: // charm
                    activeChampion.properties.charm++;
                    break;
                case 1: // discipline
                    activeChampion.properties.discipline++;
                    break;
                case 2: // brawn
                    activeChampion.properties.brawn++;
                    break;
                case 3: // wisdom
                    activeChampion.properties.wisdom++;
                    break;
                case 4: // luck
                    activeChampion.properties.luck++;
                    break;
                case 5: // wealth
                    activeChampion.properties.wealth++;
                    break;
            }

            var skills = activeChampion.GetComponentInChildren<StatsContainer>();
            skills.stats[skillIndex].value++;
            activeChampion.properties.skillpoints--;
            UpdateSkillNumbers();
            CheckUnspentSkillpoints();
        }
    }

    void SetChampionAbilityText()
    {
        abilityText.text = "Ability: " + activeChampion.properties.GetAbilityString();
    }
}
