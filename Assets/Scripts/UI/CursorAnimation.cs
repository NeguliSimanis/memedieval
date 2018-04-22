using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorAnimation : MonoBehaviour {

    //GameObject cursorClickObject;
    GameObject canvasParent;
    
    //string canvasName = "Canvas";

    Animator cursorAnimator;

    int xOffset = 45;
    int yOffset = -40;

    bool isActive = false;
    void Start()
    {
        HideCursor();
        SetScale();
        cursorAnimator = gameObject.GetComponent<Animator>();
        
       // cursorClickObject = canvas.GetComponent<CursorAnimFinder>().cursorAnim;
    }


    void SetScale()
    {
        //Debug.Log("setting scale");
        canvasParent = transform.parent.gameObject;
        gameObject.transform.localScale = new Vector3(1f/canvasParent.transform.localScale.x, 1f / canvasParent.transform.localScale.y, 1f / canvasParent.transform.localScale.z);
        /*if (canvasParent.transform.localScale.x > 1.2)
            gameObject.transform.localScale = new Vector3(canvasParent.transform.localScale.x / 3, canvasParent.transform.localScale.y / 3, canvasParent.transform.localScale.z / 3);
        else */
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
        cursorAnimator.SetTrigger("Click");
    }
       

	void Update ()
    {
        gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z); //new Vector3(Input.mousePosition.x + xOffset, Input.mousePosition.y + yOffset, Input.mousePosition.z);
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("called click");
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
