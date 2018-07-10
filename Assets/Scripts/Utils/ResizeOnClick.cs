using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// temporarily resizes gameobject 
public class ResizeOnClick : MonoBehaviour {

    [SerializeField]
    float resizeAmount;
    [SerializeField]
    float resizeDuration = 0.15f;
    Vector3 defaultSize;
    public bool isResized = false;

    private void Start()
    {
        defaultSize = gameObject.transform.localScale;
    }

    public void ChangeSize()
    {
        if (!isResized)
            defaultSize = this.gameObject.transform.localScale;
        isResized = true;
        float xResize = gameObject.transform.localScale.x * resizeAmount;
        float yResize = gameObject.transform.localScale.y * resizeAmount;
        float zResize = gameObject.transform.localScale.z * resizeAmount;
        gameObject.transform.localScale = new Vector3(xResize, yResize, zResize);
        StartCoroutine(StartResizeCountdown());
    }

    IEnumerator StartResizeCountdown()
    {
        yield return new WaitForSeconds(resizeDuration);
        ResetOriginalSize();
    }

    private void ResetOriginalSize()
    {
        isResized = false;
        gameObject.transform.localScale = defaultSize;
    }
}
