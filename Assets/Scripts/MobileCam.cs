using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// code taken from https://www.youtube.com/watch?v=c6NXkZWXHnc
/// and https://stackoverflow.com/questions/24496438/can-i-take-a-photo-in-unity-using-the-devices-camera
/// </summary>

public class MobileCam : MonoBehaviour
{
    
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;
    //public bool frontFacing;

    [SerializeField] SpriteRenderer testingImage;

    private int captainFaceCount = 0;

    [SerializeField] GameObject gameManager; // for storing captain faces
    [SerializeField] GameObject captainFaceMask;
    [SerializeField] GameObject captainFacePanel;

    private Sprite captainFace;
    public static GameObject firstCaptainFace;
    public static GameObject secondCaptainFace;
    public static GameObject thirdCaptainFace;

    private WebCamDevice[] devices;
    private int camID;

    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        defaultBackground = background.texture;
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
            if (i == devices.Length-1 && backCam==null)
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
        fit.aspectRatio = ratio; // Set the aspect ratio

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

       // photo = CalculateTexture(photo.height,photo.width,(float)photo.height,0F,0F, photo);

        // create a sprite from the taken photo
        captainFace = Sprite.Create(photo, new Rect(0.0f, 0.0f, photo.width, photo.height), new Vector2(0.5f, 0.5f), 60f);

        // attach the sprite to a an gameobject for testing purposes
        testingImage.sprite = captainFace;

        //add the new captain face
        CopyCaptainFace();
    }

    private void CopyCaptainFace()
    {
        captainFaceCount++;

        if (captainFaceCount == 1)
        {
            // display the captain face
            firstCaptainFace = Instantiate(captainFaceMask, captainFacePanel.transform);
            firstCaptainFace.transform.localScale = new Vector3(0.8F, 0.8f, 0.8f);
            //firstCaptainFace.transform.GetChild(0).GetComponent<>

            // store the captain face
            GameObject firstCopy = Instantiate(firstCaptainFace, gameManager.transform);
            firstCopy.SetActive(false);
            firstCopy.gameObject.tag = "Peasant face";
        }

        else if (captainFaceCount == 2)
        {
            secondCaptainFace = Instantiate(captainFaceMask, captainFacePanel.transform);
            firstCaptainFace.transform.localPosition -= new Vector3(0F, 15f, 0f);
            secondCaptainFace.transform.localScale = new Vector3(0.8F, 0.8f, 0.8f);

            // store the captain face
            GameObject secondCopy = Instantiate(secondCaptainFace, gameManager.transform);
            secondCopy.SetActive(false);
            secondCopy.gameObject.tag = "Knight face";
        }
        
        else if (captainFaceCount == 3)
        {
            thirdCaptainFace = Instantiate(captainFaceMask, captainFacePanel.transform);
            thirdCaptainFace.transform.localScale = new Vector3(0.8F, 0.8f, 0.8f);
            thirdCaptainFace.transform.localPosition -= new Vector3(0F, -15f, 0f);

            // store the captain face
            GameObject thirdCopy = Instantiate(thirdCaptainFace, gameManager.transform);
            thirdCopy.SetActive(false);
            thirdCopy.gameObject.tag = "Archer face";
        }

        else
        {
            Debug.Log("Load next scene");
            backCam.Stop();
            return;
        }

        testingImage.sprite = null;
    }
    public void StopCamera()
    {
        backCam.Stop();
    }
}