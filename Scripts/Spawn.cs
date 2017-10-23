using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn : MonoBehaviour
{
    private static int PeasantCaptainsLeft;
    private static int ArcherCaptainsLeft;
    private static int KnightCaptainsLeft;
    private static float spawnTimestamp;
    private static float enemyTimestamp;

    [SerializeField] private Attack.Type captain;
    [SerializeField] private bool isCaptain;
    [SerializeField] private Resources resources;
    [SerializeField] private float Cooldown;
    [SerializeField] private int UnitCost;
    [SerializeField] private bool Enemy;
    [SerializeField] private Image CooldownBar;
    [SerializeField] private Button unit;
    [SerializeField] private Button capt;

    public Text UnitCostText;

    public WaypointFollower Character;
    public Waypoint startingPoint;


    void Awake()
    {
        ResetAllValues();
    }


    void Start()
    {
        if (UnitCostText != null)
        {
            UnitCostText.text = "Cost: " + UnitCost;
        }
    }


    void Update()
    {
        if (isCaptain && unit != null && unit.enabled)
        {
            //Debug.Log("isCaptain1");
            if (Attack.Type.Archer == captain && Health.Archer) unit.interactable = false;
            if (Attack.Type.Knight == captain && Health.Knight) unit.interactable = false;
            if (Attack.Type.Peasant == captain && Health.Peasant) unit.interactable = false;
        }

        if (CooldownBar == null) return;
        if (!isCaptain && !Enemy)
            CooldownBar.fillAmount = (spawnTimestamp + Cooldown - Time.time) / Cooldown;
    }


    public void SpawnCharacter()
    {
        if (captain == Attack.Type.Archer)
        {
            if (Health.Archer) return;
            else if (isCaptain)
            {
                if (!resources.IsEnoughResources(UnitCost)) return;
                if (ArcherCaptainsLeft == 0) return;
                ArcherCaptainsLeft--;
                capt.interactable = false;
            }
        }

        if (captain == Attack.Type.Knight)
        {
            if (Health.Knight) return;
            else if (isCaptain)
            {
                if (!resources.IsEnoughResources(UnitCost)) return;
                if (KnightCaptainsLeft == 0) return;
                KnightCaptainsLeft--;
                capt.interactable = false;
            }
        }

        if (captain == Attack.Type.Peasant)
        {
            if (Health.Peasant) return;
            else if (isCaptain)
            {
                if (!resources.IsEnoughResources(UnitCost)) return;
                if (PeasantCaptainsLeft == 0) return;
                PeasantCaptainsLeft--;
                capt.interactable = false;
            }
        }

        if ((Enemy && (Character == null || enemyTimestamp + Cooldown >= Time.time)) ||
            (!Enemy && (Character == null || spawnTimestamp + Cooldown >= Time.time)) ||
            Health.Victory || Health.GameOver) return;

        if (resources.WasEnoughResources(UnitCost))
        {
            WaypointFollower character = Instantiate(Character, startingPoint.transform.position, Quaternion.identity);
            character.SetTarget(startingPoint);

            // add face if captain
            if (captain == Attack.Type.Peasant)
            {
                if (Health.Peasant) return;
                else if (isCaptain)
                {
                    GameObject captainFace = GameObject.FindWithTag("Peasant face");
                    GameObject thisCaptainFace = Instantiate(captainFace, character.transform);

                    //thisCaptainFace.transform.localScale += new Vector3(1F, 1f, 1f);
                    thisCaptainFace.transform.localPosition = new Vector3(0, 2.3f, 0);
                    thisCaptainFace.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                    thisCaptainFace.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                    Debug.Log("face added");
                }
            }

            else if (captain == Attack.Type.Knight)
            {
                if (Health.Knight) return;
                else if (isCaptain)
                {
                    GameObject captainFace = GameObject.FindWithTag("Knight face");
                    GameObject thisCaptainFace = Instantiate(captainFace, character.transform);

                    //thisCaptainFace.transform.localScale += new Vector3(1F, 1f, 1f);
                    thisCaptainFace.transform.localPosition = new Vector3(-0.2f, 2.5f, 0);
                    thisCaptainFace.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                    thisCaptainFace.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                    Debug.Log("face added");
                }
            }

            else if (captain == Attack.Type.Archer)
            {
                if (Health.Archer) return;
                else if (isCaptain)
                {
                    GameObject captainFace = GameObject.FindWithTag("Archer face");
                    GameObject thisCaptainFace = Instantiate(captainFace, character.transform);

                    //thisCaptainFace.transform.localScale += new Vector3(1F, 1f, 1f);
                    thisCaptainFace.transform.localPosition = new Vector3(-1f, 2.5f, 0);
                    thisCaptainFace.transform.localScale = new Vector3(0.2f, 0.2f, 0);
                    thisCaptainFace.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                    Debug.Log("face added");
                }
            }

            if (Enemy) enemyTimestamp = Time.time;
            else spawnTimestamp = Time.time;
        }
    }
    

    public static void ResetAllValues()
    {
        PeasantCaptainsLeft = 1;
        ArcherCaptainsLeft = 1;
        KnightCaptainsLeft = 1;
    }
}

