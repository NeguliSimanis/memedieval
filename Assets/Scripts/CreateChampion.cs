using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Script is attached to CHARCREATE, ChampionCreate objects in the tavern scene
 * ChampionCreate is tagged in the scene as "ChampionCreator"
 * CHARCREATE is untagged
 */

public class CreateChampion : MonoBehaviour
{
    #region variable to deterimine if we need to use camera and some other methods
    [SerializeField]
    private bool scriptAttachedToRecruitPanel;
    private bool useCamera; // false if previus is false
    #endregion

    public GameObject[] ChampionsPrefabs;
    public GameObject StatsContainerPrefab;
    public InputField Name;

    private Sprite championFace;
    public static GameObject firstChampionFace;

    private WebCamDevice[] devices;
    private int camID;
    private bool camAvailable;
    private WebCamTexture backCam;
    public Texture2D pic;

    public RawImage background;

    public bool isMan;

    #region bio strings
    public string[] Sentence1part1= { "{0} was born", "{0} fondly recalls the time that was spent",
    "{0} spent the childhood","The scars borne by {0} were struck" };
    public string[] Sentence1part2 = {"in a poor family", "in a church","in a noble family","under the starry sky",
    "on the battlefield","in the woods","on a mountain","under the bridge","in secrecy","in a strange land" };
    public string[] Sentence1part3 = { "when the bell tolled midnight.","under the guise of the night.","a long time ago.","and without shame.","illuminated by God’s light.",
    "and guided by truth in sword."};
    public string[] Sentence2part1 = { " But everything changed when ", " But one faithful day ", " That all changed when " };
    public string[] Sentence2part2 = { "he infidels attacked ", "a pastor arrived to the village ", "{0} lost their dog in the woods ", "the vikings arrived" };
    public string[] Sentence2part3 = { "and took their spouse away.", "and praised the Lord.", "and burned everything in sight.", "and drank all the mead in the village." };
    public string[] Sentence3part1 = { " Now {0}", " Ever since, {0}", " Evermore, {0}" };
    public string[] Sentence3part2 = { "roams the lands", "is on a quest", "offers their sword for hire", "spends the days at the tavern" };
    public string[] Sentence3part3 = { "in search of vengeance.", "in search of cranberries.", "to find their true self.", "to clear thy name." };
    #endregion

    #region motto strings
    public string[] Motto = {
        "An veritas, an nihil",
        "De oppresso Liber",
        "Deus vult",
        "Ubi concordia, ibi victoria",
        "Vae victis!",
        "Nota Bene",
        "Tum podem extulit horridulum",
        "Fortes fortuna iuvat"
    };
    #endregion

    #region Champion text generation
    public string MakeMotto()
    {
        int r = Random.Range(0, Motto.Length);
        return Motto[r];
    }

    public string MakeSentence1(string charname)
    {
        int r1 = Random.Range(0, Sentence1part1.Length);
        int r2 = Random.Range(0, Sentence1part2.Length);
        int r3 = Random.Range(0, Sentence1part3.Length);
        string Sentence = string.Format(Sentence1part1[r1], charname) + " " +
            string.Format(Sentence1part2[r2], charname) + " " +
            string.Format(Sentence1part3[r3], charname);
        return Sentence;
    }

    public string MakeSentence2(string charname)
    {
        int r1 = Random.Range(0, Sentence2part1.Length);
        int r2 = Random.Range(0, Sentence2part2.Length);
        int r3 = Random.Range(0, Sentence2part3.Length);
        string Sentence = string.Format(Sentence2part1[r1], charname) + " " +
            string.Format(Sentence2part2[r2], charname) + " " +
            string.Format(Sentence2part3[r3], charname);
        return Sentence;
    }

    public string MakeSentence3(string charname)
    {
        int r1 = Random.Range(0, Sentence3part1.Length);
        int r2 = Random.Range(0, Sentence3part2.Length);
        int r3 = Random.Range(0, Sentence3part3.Length);
        string Sentence = string.Format(Sentence3part1[r1], charname)+ " " +
           string.Format(Sentence3part2[r2], charname)+ " " +
           string.Format(Sentence3part3[r3], charname);
        return Sentence;
    }

    public string MakeBio(string charname)
    {
        return MakeSentence1(charname) + MakeSentence2(charname) + MakeSentence3(charname);
    }
    #endregion

    public Champion createChamp()
    {
        var g = new GameObject();
        var c = g.AddComponent<Champion>();
        return c;
    }

    public void StartUsingWebcam()
    {
        devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("No Camera Detected");
            camAvailable = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name,Screen.width, Screen.height);
                camID = i;
            }
            // if no backcamera detected, use front camera
            if (i == devices.Length - 1 && backCam == null)
            {
                Debug.Log("Unable to find back camera");
                backCam = new WebCamTexture(devices[i].name,Screen.width,Screen.height);
                camID = i;
            }
        }
        backCam.Play();
        background.texture = backCam;
        camAvailable = true;
    }

    void Update()
    {
        if (!camAvailable)
            return;
        float ratio = (float)backCam.width / (float)backCam.height;
        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera
        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void TakePhoto()
    {
        StartCoroutine(TestPhoto());
    }

    public void SwitchCamera()
    {
        backCam.Stop();
        if (camID > 1)
        {
            camID--;
            //backCam = backCam = new WebCamTexture(devices[camID].name, Screen.width, Screen.height);
        }
        else if (devices.Length > 1)
        {
            camID++;
            //backCam = backCam = new WebCamTexture(devices[camID].name, Screen.width, Screen.height);
        }
        backCam = backCam = new WebCamTexture(devices[camID].name, 640, 480);

        backCam.Play();
        background.texture = backCam;
        camAvailable = true;
    }

    private IEnumerator TestPhoto()
    {
        // NOTE - you almost certainly have to do this here:
        yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 

        Texture2D photo = new Texture2D(backCam.width, backCam.height);
        photo.SetPixels(backCam.GetPixels());
        photo.Apply();
        pic = photo;
        backCam.Stop();
        //add the new captain face
        //CopyCaptainFace();
    }

    //private void CopyCaptainFace()
    //{

    //    // display the captain face
    //    firstCaptainFace = Instantiate(captainFaceMask, captainFacePanel.transform);
    //    firstCaptainFace.transform.localScale = new Vector3(0.8F, 0.8f, 0.8f);
    //    //firstCaptainFace.transform.GetChild(0).GetComponent<>

    //    // store the captain face
    //    GameObject firstCopy = Instantiate(firstCaptainFace, gameManager.transform);
    //    firstCopy.SetActive(false);
    //    firstCopy.gameObject.tag = "Peasant face";

    //    Debug.Log("Load next scene");
    //    backCam.Stop();
    //}

    
    public void setMan()
    {
        isMan = true;
    }
    public void setWoman()
    {
        isMan = false;
    }

    public void SaveInfo()
    {
        int champClassID = Random.Range(0, ChampionsPrefabs.Length);
        var player = PlayerProfile.Singleton;
        var pgo = player.gameObject;
        var championObject = Instantiate<GameObject>(ChampionsPrefabs[champClassID]);
 
        championObject.SetActive(false);
        championObject.transform.parent = pgo.transform;

        var champo = championObject.GetComponent<Champion>();     
        string Name1 = Name.text;
        champo.properties.champClass = champClassID;
        player.champions.Add(champo);
        champo.properties.Name = Name1;
        champo.properties.isMan = isMan;
        champo.properties.SetPicture(pic);
        champo.properties.bio = MakeBio(Name1);
        champo.properties.quote = MakeMotto();

        var stats = Instantiate(StatsContainerPrefab);
        stats.transform.parent = champo.transform;

        var o = FindObjectOfType<TavernGUIchanger>();
        PlayerProfile.Singleton.SaltCurrent -= 5;
        o.ChangeLayout(0);

    }

    public void LoadChampionFromSave(ChampionData championData)
    {
        int champClassID = championData.champClass;
        var player = PlayerProfile.Singleton;
        var pgo = player.gameObject;
        var championObject = Instantiate<GameObject>(ChampionsPrefabs[champClassID]);

        championObject.SetActive(false);
        championObject.transform.parent = pgo.transform;

        var champo = championObject.GetComponent<Champion>();
        string Name1 = championData.Name;

        // set champion properties
        champo.properties.champClass = champClassID;
        player.champions.Add(champo);
        champo.properties.Name = Name1;
        champo.properties.isMan = championData.isMan;
        champo.properties.SetPicture(championData.LoadPictureAsTexture2D());
        champo.properties.bio = championData.bio;
        champo.properties.quote = championData.quote;

        var stats = Instantiate(StatsContainerPrefab);
        stats.transform.parent = champo.transform;
    }

    private void OnEnable()
    {
        useCamera = scriptAttachedToRecruitPanel;
        if (useCamera)
            StartUsingWebcam();
    }
    public void StopCamera()
    {
        backCam.Stop();
    }
}
