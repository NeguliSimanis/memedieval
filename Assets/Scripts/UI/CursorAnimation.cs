using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorAnimation : MonoBehaviour {

    //GameObject cursorClickObject;
    //GameObject canvas;
    //string canvasName = "Canvas";

    Animator cursorAnimator;

    int xOffset = 45;
    int yOffset = -40;

    void Start()
    {
        HideCursor();
        //canvas = GameObject.Find(canvasName);
       // cursorClickObject = canvas.GetComponent<CursorAnimFinder>().cursorAnim;
        cursorAnimator = gameObject.GetComponent<Animator>();
    }

    void HideCursor()
    {
        Cursor.visible = false;
    }


    void ShowCursor()
    {
        Cursor.visible = true;
    }

    void PlayClickAnim()
    {
        Debug.Log("playing animation");
        cursorAnimator.SetTrigger("Click");
    }
        

	void Update ()
    {
        gameObject.transform.position = new Vector3(Input.mousePosition.x + xOffset, Input.mousePosition.y + yOffset, Input.mousePosition.z);

        if (Input.GetMouseButtonDown(0))
        {
            PlayClickAnim();
            
        }

       /* if (isAnimActive)
        {
            if (Time.time > clickEndTime)
            {
               // ShowCursor();
                EndClickAnim();
            }
        }*/
    }
}
