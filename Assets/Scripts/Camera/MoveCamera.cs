using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the main camera to better see the battlefield etc.
/// 
/// If you swipe right, camera moves left
/// If you swipe left, camera moves right
/// 
/// based on this script: https://www.youtube.com/watch?v=poeXGuQ7eUo
/// 
/// 02.08.2018 Sīmanis Mikoss
/// </summary>

public class MoveCamera : MonoBehaviour
{
    Camera myCamera;
    private Vector3 offset;
    private bool canSlideRight = false;
    private bool canSlideLeft = false;

    private bool isCameraSlidingRight = false;
    private bool isCameraSlidingLeft = false;

    [SerializeField]
    GameObject rightBorder;
    Renderer rightScreenBorder;
    [SerializeField]
    GameObject leftBorder;
    Renderer leftScreenBorder;

    private void Start()
    {
        leftScreenBorder = leftBorder.GetComponent<Renderer>();
        rightScreenBorder = rightBorder.GetComponent<Renderer>();
        myCamera = gameObject.GetComponent<Camera>();
    }

	private void Update ()
    {
        if (!rightScreenBorder.isVisible)
        {
            canSlideRight = true;
            if (SwipeManager.Instance.IsSwiping(SwipeDirection.Left))
            {
                // move camera to right side
                SlideCameraHorizontally(0.02f);
                isCameraSlidingRight = true;
                isCameraSlidingLeft = false;
            }
        }
        // left edge of the battlefield is visible
        else
        {
            canSlideRight = false;
        }
        if (!leftScreenBorder.isVisible)
        {
            canSlideLeft = true;
            if (SwipeManager.Instance.IsSwiping(SwipeDirection.Right))
            {
                // move camera to left side
                SlideCameraHorizontally(-0.02f);
                isCameraSlidingRight = false;
                isCameraSlidingLeft = true;
            }
        }
        // right edge of the battlefield is visible
        else
        {
            canSlideLeft = false;
        }
    }

    private void LateUpdate()
    {
        if (isCameraSlidingLeft && !canSlideLeft)
            return;
        if (isCameraSlidingRight && !canSlideRight)
            return;
        transform.position = transform.position + offset;
    }

    private void SlideCameraHorizontally(float xOffset)
    {
        offset = new Vector3(xOffset,0f);

    }
}
