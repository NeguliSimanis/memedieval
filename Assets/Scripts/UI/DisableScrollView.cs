using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Disables the scroll rect the attached component if there are less than X children
/// (includes inactive children)
/// </summary>
public class DisableScrollView : MonoBehaviour {

    ScrollRect scrollViewToDisable;
    [SerializeField]
    Transform parentToCheck;
    [SerializeField]
    int minChildCount = 4; // if parent has any less children, scrollrect is disabled

	void Start ()
    {
        GetComponents();
        DisableIfFewChildren();
	}

    void GetComponents()
    {
        scrollViewToDisable = gameObject.GetComponent<ScrollRect>();
    }
	
    void DisableIfFewChildren()
    {
        if (parentToCheck.childCount < minChildCount)
        {
            scrollViewToDisable.enabled = false;
        }
        else
            scrollViewToDisable.enabled = true;
    }
}
