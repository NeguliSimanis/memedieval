using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUIToFitScreen : MonoBehaviour
{
    /// <summary>
    /// Scales or moves UI so that it would be visible on screen
    /// </summary>

    [SerializeField]
    bool activateOnStart;
    
    RectTransform myRectTransform;
    RectTransform changableRectTransform; // can be from a parent object or where this script is attached
    bool isFullyVisible;

    // determines whether parent or other UI needs to be moved instead
    [SerializeField]
    bool changeWithParent;
    [SerializeField]
    bool changeWithOtherUI;
    [SerializeField]
    RectTransform otherRectTransform;

    // determines whether UI will be resized or moved if it does not fit
    [SerializeField]
    bool needResize; 

    [SerializeField]
    Camera mainCamera;

    // used to determine in which direction should UI be moved if it's not visible
    [SerializeField]
    bool isOnRightSideOfScreen;
    [SerializeField]
    bool isTopOfScreen;     

    void Start()
    {
        if (activateOnStart)
            StartFit();
    }

    public void StartFit()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        if (changeWithParent)
        {
            changableRectTransform = gameObject.transform.parent.gameObject.GetComponent<RectTransform>();
        }
        else
        {
            changableRectTransform = myRectTransform;
        }
        CheckIfFullyVisibile();
    }

    void CheckIfFullyVisibile()
    {
        isFullyVisible = myRectTransform.IsFullyVisibleFrom(mainCamera);
        if (!isFullyVisible)
        {
            FitToScreen();
        }
    }

    void FitToScreen()
    {
        if (needResize)
            ResizeToFitScreen();
        else
            MoveToFitScreen();
        CheckIfFullyVisibile();
    }

    void ResizeToFitScreen()
    {
        changableRectTransform.localScale = new Vector3(changableRectTransform.localScale.x * 0.9f, changableRectTransform.localScale.y, changableRectTransform.localScale.z);
        
        // NB! - this can cause bugs if other object is parent!
        if (changeWithOtherUI)
            otherRectTransform.localScale = new Vector3(otherRectTransform.localScale.x * 0.9f, otherRectTransform.localScale.y, otherRectTransform.localScale.z);
    }

    void MoveToFitScreen()
    {
        float xDistance = 0.1f;
        if (isOnRightSideOfScreen)
            xDistance = -0.1f;
        changableRectTransform.gameObject.transform.Translate(xDistance, 0f, 0f);

        if (changeWithOtherUI)
            otherRectTransform.gameObject.transform.Translate(xDistance, 0f, 0f);
    }
}
