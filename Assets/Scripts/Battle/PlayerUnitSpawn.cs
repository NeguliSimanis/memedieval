using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the logic for spawning regular player units (not champions) in battle.
/// Attached to these game objects:
///         RecruitArcher
///         RecruitKnight
///         RecruitPeasant
///         DeployChampion
/// 
/// Part of the logic was originally in the Spawn.cs script.
/// 
/// 05.08.2018 Sīmanis Mikoss
/// </summary>

public class PlayerUnitSpawn : MonoBehaviour
{
    #region variables
    [SerializeField] private bool isChampion = false;

    public bool needToDisableSummonButton = false; // formerly isChampionDying
    private bool isUnitDying = false;
    private bool needToEnlargeCooldownBar = false; // used to check for
    private float spawnTimestamp;

    [SerializeField] private bool isTutorial = false; // spawn button is not hidden in tutorial
    [SerializeField] private Attack.Type captain;
    [SerializeField] private MeMedieval.Resources resources;
    [SerializeField] private float Cooldown;
    [SerializeField] private int unitCost;

    [Header("UI elements")]
    [SerializeField] private Image CooldownBar;
    [SerializeField] private Button unitButton;
    [SerializeField] Text unitCostProgressText; // changes as player accumulates resources. i.e. 7/20, 9/20, ATTACK!
    [SerializeField] Text unitFullCostText;  // remains the same throughout the battle

    private Color defaultCooldownBarColor = new Color(1, 1, 1, 1);
    private Color inactiveCooldownBarColor = new Color(0.682f, 0.684f, 0.684f, 1);

    public WaypointFollower Character;
    public Waypoint startingPoint;
    #endregion

    void Start()
    {
        CheckIfAvailable();
        SetPriceModifiers();
        DisplayStaticPrice();
        //unitCostProgressText.text = unitCost.ToString();
    }

    void DisplayStaticPrice()
    {
        unitFullCostText.text = unitCost.ToString();
    }

    void CheckIfAvailable()
    {
        if (captain == Attack.Type.Archer)
        {
            CheckChampClass(2);
        }
        else if (captain == Attack.Type.Knight)
        {
            CheckChampClass(1);
        }
        else
        {
            CheckChampClass(0);
        }
    }

    void CheckChampClass(int classID)
    {
        int champCount = 0;
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.properties.champClass == classID && champion.invitedToBattle)
            {
                champCount++;
            }
        }
        if (champCount == 0)
        {
            DisablePlayerSpawn();
        }
    }

    void DisablePlayerSpawn()
    {
        if (!isTutorial)
            gameObject.transform.parent.gameObject.SetActive(false);
    }

    void SetPriceModifiers()
    {
        GameObject player = PlayerProfile.Singleton.gameObject;
        
        // set champion effect
        ChampionEffect championEffect = player.GetComponent<ChampionEffect>();
        float tempCost = championEffect.priceCoefficient * unitCost;
        unitCost = Mathf.RoundToInt(tempCost);

        if (unitCost < championEffect.minUnitPrice)
        {
            unitCost = championEffect.minUnitPrice;
        }
    }

    void Update()
    {
        UpdateUnitCostText();

        if (unitButton != null && unitButton.enabled)
        {
            if (Attack.Type.Archer == captain && Health.Archer) unitButton.interactable = false;
            if (Attack.Type.Knight == captain && Health.Knight) unitButton.interactable = false;
            if (Attack.Type.Peasant == captain && Health.Peasant) unitButton.interactable = false;
        }

        // disabling player unit summoning buttons
        CheckIfDeadChampion();

        // disable button if not enough resources
        if (!resources.IsEnoughResources(unitCost))
        {
            UpdateCooldownBarColor(false);
            unitButton.interactable = false;
        }
        // show button if enough resources
        else
        {
            UpdateCooldownBarColor(true);
        }

        // disable spawning button during cooldown
        if (spawnTimestamp + Cooldown > Time.time)
        {
            unitButton.interactable = false;
        }

        // enable spawning button after cooldown
        else
        {
            unitButton.interactable = true;
        }

        UpdateCooldownBarFill();

        if (needToDisableSummonButton == true)
        {
            DisableSpawnButton();
        }
        if (isUnitDying == true)
        {
            DisableSpawnButton();
        }
    }

    void UpdateUnitCostText()
    {
        if (resources.IsEnoughResources(unitCost))
        {
            unitCostProgressText.text = "ATTACK!";
            // set cost text color to yellow if the cooldown is still active
            if (Time.time <= spawnTimestamp + Cooldown)
            {
                unitCostProgressText.color = new Color(1, 0.92f, 0.016f, 0.5f);
            }
            else
            unitCostProgressText.color = new Color(0, 0, 0);
        }
        else 
        {
            unitCostProgressText.text = resources.Amount.ToString();  //+ " / " + unitCost.ToString();
            unitCostProgressText.color = new Color(1, 0.92f, 0.016f, 1f);
        }
    }

    void UpdateCooldownBarColor(bool isEnoughResources)
    {
        if (isEnoughResources)
            CooldownBar.color = defaultCooldownBarColor;
        else
            CooldownBar.color = inactiveCooldownBarColor;
    }

    void UpdateCooldownBarFill()
    {
        // show cooldown on UI bar if available
        if (CooldownBar == null) return;
        CooldownBar.fillAmount = 1f - ((spawnTimestamp + Cooldown - Time.time) / Cooldown);

        // enlarge cooldown bar once unit is available for spawning
        if (CooldownBar.fillAmount < 1f)
        {
            needToEnlargeCooldownBar = true;
        }
        else if (needToEnlargeCooldownBar)
        {
            CooldownBar.gameObject.GetComponent<ResizeOnClick>().ChangeSize();
            needToEnlargeCooldownBar = false;
        }
    }

    void CheckIfDeadChampion()
    {
        if (!Health.Archer && captain == Attack.Type.Archer)
            return;
        if (!Health.Knight && captain == Attack.Type.Knight)
            return;
        if (!Health.Peasant && captain == Attack.Type.Peasant)
            return;
        // champion is dead, must disable spawning regular units
        StartDisablingUnitButt();
    }

    void DisableSpawnButton()
    {
        // wait until spawn button resizing effect is over before disabling button object
        if (gameObject.GetComponent<ResizeOnClick>().isResized != true)
        {
            unitButton.gameObject.SetActive(false);
        }
    }

    public void SpawnCharacter()
    { 
        if ((Character == null || spawnTimestamp + Cooldown >= Time.time) ||
            Health.Victory || Health.GameOver)
            return;

        if (resources.WasEnoughResources(unitCost))
        {
            WaypointFollower character = Instantiate(Character, startingPoint.transform.position, Quaternion.identity);
            character.SetTarget(startingPoint);
            spawnTimestamp = Time.time;
            if (isChampion)
            {
                SetChampionAbility(character.gameObject.GetComponent<Champion>());
                StartDisablingChampionButt();
            }
        }
    }

    void SetChampionAbility(Champion currentChampion)
    {
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            // found champion who was of same class and is invited to battle
            if (champion.invitedToBattle == true && champion.properties.GetChampionAttackType() == captain)
            {
                currentChampion.properties.currentChampionAbility = champion.properties.currentChampionAbility;
                break;
            }
        }
    }

    void StartDisablingUnitButt()
    {
        unitButton.interactable = false;
        unitButton.gameObject.SetActive(false);
        isUnitDying = true;
    }

    private void StartDisablingChampionButt()
    {
        unitButton.interactable = false;
        needToDisableSummonButton = true;
    }
}
