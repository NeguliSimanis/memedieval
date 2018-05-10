using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorAnimation : MonoBehaviour {

    GameObject canvasParent;
    Animator cursorAnimator;
    Image cursorImage;
    int xOffset = 45;
    int yOffset = -40;

    float clickDuration = 0.1f;
    float clickAnimationEndTime = 0f;

    [SerializeField]
    bool animateCursor = true;
    [SerializeField]
    bool testingAndroid = false;


    void Start()
    {
        if (animateCursor)
        {
            HideDefaultCursor();
            ScaleCursorAnimation();
        }
        GetComponents();
    }

    void GetComponents()
    {
        cursorAnimator = gameObject.GetComponent<Animator>();
        cursorImage = gameObject.GetComponent<Image>();
    }

    void ClearCursorAnimation(bool clearCursorAnimation = true)
    {
        if (clearCursorAnimation == true)
            cursorImage.enabled = false;
        else
            cursorImage.enabled = true;
    }

    void ScaleCursorAnimation()
    {
        canvasParent = transform.parent.gameObject;
        gameObject.transform.localScale = new Vector3(1f/canvasParent.transform.localScale.x, 1f / canvasParent.transform.localScale.y, 1f / canvasParent.transform.localScale.z);
    }

    void HideDefaultCursor()
    {
        Cursor.visible = false;
    }

    void ShowDefaultCursor()
    {
        Cursor.visible = true;
    }

    void PlayClickAnim()
    {
        cursorAnimator.SetTrigger("Click");
        if (Application.platform == RuntimePlatform.Android || testingAndroid == true)
            clickAnimationEndTime = Time.time + clickDuration;       
    }
       

    void MoveAnimationObject()
    {
        gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
    }

    void ManageAndroidCursor()
    {
        
        if (Time.time < clickAnimationEndTime)
        {
            Debug.Log("animating");
            ClearCursorAnimation(false);
            
        }     
        else
        {
            Debug.Log("not animating");
            ClearCursorAnimation();
        }
               
    }

	void Update ()
    {
        if (animateCursor)
        {
            MoveAnimationObject();
            if (Input.GetMouseButtonDown(0))
            {
                PlayClickAnim();
            }
            HideDefaultCursor();
        }
        else
        {
            ShowDefaultCursor();
        }

        if (Application.platform == RuntimePlatform.Android || testingAndroid == true)
        {
            ManageAndroidCursor();
        }
        
    }
}
