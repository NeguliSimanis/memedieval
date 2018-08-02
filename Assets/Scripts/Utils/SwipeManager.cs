using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Usage notes:
/// - don't have multiple istances of SwipeManager in one scene
/// 
/// https://www.youtube.com/watch?v=poeXGuQ7eUo
/// 
/// </summary>

public enum SwipeDirection
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,
}

public class SwipeManager: MonoBehaviour
{
    private static SwipeManager instance;
    public static SwipeManager Instance { get { return instance; } }
    public SwipeDirection Direction { set; get; }

    private Vector2 touchPosition;
    private float swipeResistanceX = 50f;
    private float swipeResistanceY = 100f;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        Direction = SwipeDirection.None;

        // detect where the player presses mouse button/touch
        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 deltaSwipe = touchPosition - new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX)
            {
                // swipe on the x axis
                Direction |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }
        }
    }

    public bool IsSwiping(SwipeDirection dir)
    {
        if (dir == Direction)
        {
            return true;
        }
        else return false;
    }


}


