using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeGameToFitScreen : MonoBehaviour {
    
    /// <summary>
    /// QUICK FIX
    /// Resizes certaing game elements to fit 16:10 resolution if it's detected
    /// 
    /// 19.07.2018, Sīmanis Mikoss
    /// </summary>

    Camera thisCamera;
    [SerializeField]
    Canvas[] sceneCanvas;

    void Start()
    {
        Debug.Log(Screen.currentResolution);

        thisCamera = gameObject.GetComponent<Camera>();
        Debug.Log(thisCamera.pixelWidth);
        //Debug.Log(thisCamera.pixelHeight); 

        if (thisCamera.pixelWidth / thisCamera.pixelHeight == 16 / 10)
        {
            if (thisCamera.pixelWidth == 1280 || thisCamera.pixelWidth == 1920)
            {
               // Debug.Log("NO");
                return;
            }
            Resize();
        }
    }

    void Resize()
    {
        //Debug.Log("YES");
        //thisCamera.rect = new Rect(0, 0.05f, 1, 0.9f);

        foreach (Canvas canva in sceneCanvas)
        {
            RectTransform currentRect = canva.GetComponent<RectTransform>();
            currentRect.localScale = new Vector3(currentRect.localScale.x * 0.9f, currentRect.localScale.y, currentRect.localScale.z);
        }
    }
}
