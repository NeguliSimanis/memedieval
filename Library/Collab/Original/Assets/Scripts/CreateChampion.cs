using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateChampion : MonoBehaviour
{
    public GameObject[] ChampionsPrefabs;
    public GameObject StatsContainerPrefab;
    public InputField Name;
    private bool camAvailable;
    private WebCamTexture backCam;
    public Texture2D pic;

    public RawImage background;

    //veidojam championa klasi
    public Champion createChamp()
    {
        var g = new GameObject();
        var c = g.AddComponent<Champion>();
        return c;
    }

    private Sprite captainFace;
    public static GameObject firstCaptainFace;

    private WebCamDevice[] devices;
    private int camID;

    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
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
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                camID = i;
            }
            // if no backcamera detected, use front camera
            if (i == devices.Length - 1 && backCam == null)
            {
                Debug.Log("Unable to find back camera");
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
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
            backCam = backCam = new WebCamTexture(devices[camID].name, Screen.width, Screen.height);
        }
        else if (devices.Length > 1)
        {
            camID++;
            backCam = backCam = new WebCamTexture(devices[camID].name, Screen.width, Screen.height);
        }

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
       // backCam.Stop();
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
    public bool isMan;
    public void setMan()
    {
        isMan = true;
    }
    public void setwoman()
    {
        isMan = false;
    }
    public void SaveInfo()
    {
        string Name1 = Name.text;
        int number = Random.Range(0, ChampionsPrefabs.Length);

        var player = PlayerProfile.Singleton;
        var pgo = player.gameObject;
        var champ = Instantiate<GameObject>(ChampionsPrefabs[number]);
        champ.SetActive(false);
        champ.transform.parent = pgo.transform;
        var champo = champ.GetComponent<Champion>();
        champo.champClass = number;
        player.champions.Add(champo);
        champo.Name = Name1;
        champo.isMan = isMan;
        champo.picture = pic;
        var stats = Instantiate(StatsContainerPrefab);
        stats.transform.parent = champo.transform;
        var o = FindObjectOfType<TavernGUIchanger>();
        o.changeLayout(0);
    }

    private void OnEnable()
    {
        Start();
    }
}
