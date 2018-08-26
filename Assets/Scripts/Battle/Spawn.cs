﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Spawn : MonoBehaviour
{
    /************************
     
     Script is attached to:
    no
    game objects

     ************************/
    private bool isChampionDying = false;
    private bool isUnitDying = false;
    private bool needToEnlargeCooldownBar = false; // used to check for

    public static int PeasantCaptainsLeft;
    public static int ArcherCaptainsLeft;
    public static int KnightCaptainsLeft;
    private static float spawnTimestamp;
    private static float enemyTimestamp;

    [SerializeField] private bool isTutorial = false; // spawn button is not hidden in tutorial
    [SerializeField] private Attack.Type captain;
    [SerializeField] private bool isCaptain;
    [SerializeField] private MeMedieval.Resources resources;
    [SerializeField] private float Cooldown;
    [SerializeField] private int unitCost;
    [SerializeField] private bool Enemy;
    [SerializeField] private Image CooldownBar;
    [SerializeField] private Button unitButton;
    [SerializeField] private Button championButton;
    [SerializeField] private Image[] hideThisWhenInsufficientMeat;

    public Text UnitCostText;

    public WaypointFollower Character;
    public Waypoint startingPoint;

    public AvatarFace avatarFacePrefab;

    void Awake()
    {
        ResetAllValues();
    }

    void Start()
    {
        if (!Enemy)
        {
            CheckIfAvailable();
            SetPriceModifiers();
            UnitCostText.text = unitCost.ToString();
        }
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
        HideButtonIfNotResources();

        if (isCaptain && unitButton != null && unitButton.enabled)
        {
            if (Attack.Type.Archer == captain && Health.Archer) unitButton.interactable = false;
            if (Attack.Type.Knight == captain && Health.Knight) unitButton.interactable = false;
            if (Attack.Type.Peasant == captain && Health.Peasant) unitButton.interactable = false;
        }

        // disabling player unit summoning buttons
        if (!Enemy)
        {
            CheckIfDeadChampion();

            // disable button if not enough resources
            if (!resources.IsEnoughResources(unitCost))
            {
                if (isCaptain)
                    championButton.interactable = false;
                else
                    unitButton.interactable = false;
            }

            // disable spawning button during cooldown
            if (spawnTimestamp + Cooldown > Time.time)
            {
                unitButton.interactable = false;
                championButton.interactable = false;
            }

            // enable spawning button after cooldown
            else
            {
                championButton.interactable = true;
                unitButton.interactable = true;
            }
        }

        if (!Enemy)
        {
            UpdateCooldownBar();
        }

        if (isChampionDying == true)
        {
            DisableSpawnButton(true);          
        }
        if (isUnitDying == true)
        {
            DisableSpawnButton(false);
        }
    }

    void HideButtonIfNotResources()
    {
       /* if 
        foreach (Image elementToHide in hideThisWhenInsufficientMeat)
        {

        }*/
    }

    void UpdateCooldownBar()
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

    void DisableSpawnButton(bool isChampion)
    {
        // wait until spawn button resizing effect is over before disabling button object
        if (gameObject.GetComponent<ResizeOnClick>().isResized != true)
        {
            if (isChampion)
                championButton.gameObject.SetActive(false);
            else
                unitButton.gameObject.SetActive(false);
        }           
    }

    public void SpawnCharacter()
    {
        if (captain == Attack.Type.Archer)
        {
            if (Health.Archer) return;
            else if (isCaptain)
            {
                if (!resources.IsEnoughResources(unitCost)) return;
                if (ArcherCaptainsLeft == 0) return;
                ArcherCaptainsLeft--;
                StartDisablingChampionButt();
            }
        }

        if (captain == Attack.Type.Knight)
        {
            if (Health.Knight) return;
            else if (isCaptain)
            {
                //Debug.Log("Knight spawned!");
                if (!resources.IsEnoughResources(unitCost)) return;
                if (KnightCaptainsLeft == 0) return;
                KnightCaptainsLeft--;
                StartDisablingChampionButt();
            }
        }

        if (captain == Attack.Type.Peasant)
        {
            if (Health.Peasant) return;
            else if (isCaptain)
            {
                if (!resources.IsEnoughResources(unitCost)) return;
                if (PeasantCaptainsLeft == 0) return;
                PeasantCaptainsLeft--;
                StartDisablingChampionButt();
            }
        }

        if ((Enemy && (Character == null || enemyTimestamp + Cooldown >= Time.time)) ||
            (!Enemy && (Character == null || spawnTimestamp + Cooldown >= Time.time)) ||
            Health.Victory || Health.GameOver) return;

        if (resources.WasEnoughResources(unitCost))
        {
            WaypointFollower character = Instantiate(Character, startingPoint.transform.position, Quaternion.identity);
            character.SetTarget(startingPoint);

            // add face if captain
            if (captain == Attack.Type.Peasant)
            {
                if (Health.Peasant) return;
                else if (isCaptain)
                {
                    AddChampionFace(character, 0);
                }
            }

            else if (captain == Attack.Type.Knight)
            {
                if (Health.Knight) return;
                else if (isCaptain)
                {
                    AddChampionFace(character, 1);
                }
            }

            else if (captain == Attack.Type.Archer)
            {
                if (Health.Archer) return;
                else if (isCaptain)
                {
                    AddChampionFace(character, 2);
                }
            }

            if (Enemy) enemyTimestamp = Time.time;
            else spawnTimestamp = Time.time;
        }
    }

    void StartDisablingChampionButt()
    {
        championButton.interactable = false;
        isChampionDying = true;
    }

    void StartDisablingUnitButt()
    {
        unitButton.interactable = false;
        unitButton.gameObject.SetActive(false);
        isUnitDying = true;
    }

    public static void ResetAllValues()
    {
        PeasantCaptainsLeft = 0;
        ArcherCaptainsLeft = 0;
        KnightCaptainsLeft = 0;
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.GetClassName() == "Archer" && champion.invitedToBattle)
            {
                ArcherCaptainsLeft = 1;
            }
            else if (champion.GetClassName() == "Peasant" && champion.invitedToBattle)
            {
                PeasantCaptainsLeft = 1;
            }
            else if (champion.GetClassName() == "Knight" && champion.invitedToBattle)
            {
                KnightCaptainsLeft = 1;
            }
        }
    }

    private void TransformChampionFace(AvatarFace championFace, int championClassID)
    {
        Vector3 facePosition;
        Vector3 faceScale = new Vector3(0.02f, 0.02f, 0);

        if (championClassID == 0) // peasant
        {
            facePosition = new Vector3(-0.2f, 2.5f, 0);
        }
        else if (championClassID == 1) // knight
        {
            facePosition = new Vector3(-0.2f, 2.5f, 0);
        }
        else // archer
        {
            facePosition = new Vector3(-0.35f, 2f, 0);
        }

        championFace.transform.localPosition = facePosition;
        championFace.transform.localScale = faceScale;
    }

    private void AddChampionFace(WaypointFollower championUnit, int championClassID) // 1 = knight
    {

        var g = new GameObject();
        g.AddComponent<SpriteRenderer>();
        GameObject captainFace = g;

        var h = championUnit.GetComponentInChildren<Transform>();
        g.transform.SetParent(h.transform);
        GameObject thisCaptainFace = g;

        
        thisCaptainFace.transform.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

        AvatarFace face;

        Champion summonedChampion = PlayerProfile.Singleton.champions.Where(x => x.properties.champClass == championClassID).First();
        if (summonedChampion != null)
        {
            if (summonedChampion.properties.isCameraPicture)
            {
                face = Instantiate(avatarFacePrefab, h);
                face.SetFace(summonedChampion.properties.LoadPictureAsTexture2D());
                TransformChampionFace(face, championClassID);
            }

            championUnit.GetComponent<Champion>().properties.currentChampionAbility = summonedChampion.properties.currentChampionAbility;
            summonedChampion.onBattle = true;
        }
    }
}

