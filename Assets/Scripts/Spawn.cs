using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Spawn : MonoBehaviour
{
    private static int PeasantCaptainsLeft;
    private static int ArcherCaptainsLeft;
    private static int KnightCaptainsLeft;
    private static float spawnTimestamp;
    private static float enemyTimestamp;

    [SerializeField] private Attack.Type captain;
    [SerializeField] private bool isCaptain;
    [SerializeField] private MeMedieval.Resources resources;
    [SerializeField] private float Cooldown;
    [SerializeField] private int UnitCost;
    [SerializeField] private bool Enemy;
    [SerializeField] private Image CooldownBar;
    [SerializeField] private Button unit;
    [SerializeField] private Button capt;

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
                Debug.Log("Knight spawned!");
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
                    var g = new GameObject();
                    g.AddComponent<SpriteRenderer>();
                    GameObject captainFace = g;

                    var h = character.GetComponentInChildren<Transform>();
                    g.transform.SetParent(h.transform);
                    GameObject thisCaptainFace = g;

                    //thisCaptainFace.transform.localScale += new Vector3(1F, 1f, 1f);
                    thisCaptainFace.transform.localPosition = new Vector3(-1.2f, 2.0f, 0);
                    thisCaptainFace.transform.localScale = new Vector3(0.4f, 0.4f, 0);
                    thisCaptainFace.transform.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";


                    AvatarFace face;
                    var c = PlayerProfile.Singleton.champions.Where(x => x.champClass == 0).First();
                    if (c != null)
                    {
                        face = Instantiate(avatarFacePrefab, h);
                        face.SetFace(c.picture);
                        face.transform.localPosition = new Vector3(-0.4f, 2.0f, 0);
                        face.transform.localScale = new Vector3(0.02f, 0.02f, 0);
                        //var p = c.picture;
                        Debug.Log(c.Name);

                        //thisCaptainFace.transform.GetComponent<SpriteRenderer>().sprite = Sprite.Create(p, new Rect(0, 0, p.width, p.height), Vector2.zero);
                        c.onBattle = true;

                    }

                    //face.transform.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                    Debug.Log("face added");
                }
            }

            else if (captain == Attack.Type.Knight)
            {
                if (Health.Knight) return;
                else if (isCaptain)
                {
                    var g = new GameObject();
                    g.AddComponent<SpriteRenderer>();
                    GameObject captainFace = g;

                    var h = character.GetComponentInChildren<Transform>();
                    g.transform.SetParent(h.transform);
                    GameObject thisCaptainFace = g;

                    //thisCaptainFace.transform.localScale += new Vector3(1F, 1f, 1f);
                    thisCaptainFace.transform.localPosition = new Vector3(-1.2f, 2.0f, 0);
                    thisCaptainFace.transform.localScale = new Vector3(0.4f, 0.4f, 0);
                    thisCaptainFace.transform.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                    AvatarFace face;
                    var c = PlayerProfile.Singleton.champions.Where(x => x.champClass == 1).First();
                    if (c != null)
                    {
                        face = Instantiate(avatarFacePrefab, h);
                        face.SetFace(c.picture);
                        face.transform.localPosition = new Vector3(-0.2f, 2.5f, 0);
                        face.transform.localScale = new Vector3(0.02f, 0.02f, 0);
                        //var p = c.picture;
                        //Debug.Log(c.Name);
                        //thisCaptainFace.transform.GetComponent<SpriteRenderer>().sprite =Sprite.Create(p,new Rect(0,0,p.width,p.height),Vector2.zero);
                        c.onBattle = true;
                    }
                    Debug.Log("face added");
                }
            }

            else if (captain == Attack.Type.Archer)
            {
                if (Health.Archer) return;
                else if (isCaptain)
                {
                    var g = new GameObject();
                    g.AddComponent<SpriteRenderer>();
                    GameObject captainFace = g;

                    var h = character.GetComponentInChildren<Transform>();
                    g.transform.SetParent(h.transform);
                    GameObject thisCaptainFace = g;

                    //thisCaptainFace.transform.localScale += new Vector3(1F, 1f, 1f);
                    thisCaptainFace.transform.localPosition = new Vector3(-1.8f, 2.0f, 0);
                    thisCaptainFace.transform.localScale = new Vector3(0.4f, 0.4f, 0);
                    thisCaptainFace.transform.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
                    AvatarFace face;
                    var c = PlayerProfile.Singleton.champions.Where(x => x.champClass == 2).First();
                    if (c != null)
                    {
                        face = Instantiate(avatarFacePrefab, h);
                        face.SetFace(c.picture);
                        face.transform.localPosition = new Vector3(-0.5f, 2.5f, 0);
                        face.transform.localScale = new Vector3(0.02f, 0.02f, 0);
                        //var p = c.picture;
                        //Debug.Log(c.Name);
                        //thisCaptainFace.transform.GetComponent<SpriteRenderer>().sprite = Sprite.Create(p, new Rect(0, 0, p.width, p.height), Vector2.zero);
                        c.onBattle = true;
                    }
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

