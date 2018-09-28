using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This tool is very helpful: http://www.easyrgb.com/en/convert.php
/// </summary>
public class Crests
{
    public static bool crestColorsSet = false;
    public static List<Color> crestColors = new List<Color>();
    public static List<Color> unusedArcherColors = new List<Color>();
    public static List<Color> unusedKnightColors = new List<Color>();
    public static List<Color> unusedPeasantColors = new List<Color>();

    // public static bool[] usedArcherColors = new bool[false, false, false, false, false];
    //public static bool[] usedKnightColors = new bool[false, false, false, false, false];

    public static void SetHeraldColors()
    {
        crestColors.Add(new Color(0.84314f, 0.82353f, 0.00000f));//0.136f,0.19f,1f, 1f));        // FFD200FF - BLUE
        crestColors.Add(new Color(0.86275f, 0.15686f, 0.11765f));//(0.008f, 0.863f, 0.863f, 1f)); // DC281EFF - 
        crestColors.Add(new Color(0f, 0.22353f, 0.65098f));//(0.61f, 1f, 0.651f, 1f));      // 0039A6FF
        crestColors.Add(new Color(0f, 0.60392f, 0.23922f));//(0.398f, 1f, 0.601f, 1f));     // 009A3DFF
        crestColors.Add(new Color(1f, 0.47451f, 0f));//(0.078f, 1f, 1f, 1f));         // FF7900FF
        crestColors.Add(new Color(0.71373f, 0.03922f, 0.60784f));//(0.861f, 0.945f, 0.714f, 1f)); // B60A9BFF

        ResetArcherColors();
        ResetKnightColors();
        ResetPeasantColors();

        crestColorsSet = true;
    }

    public static void ResetArcherColors()
    {
        foreach (Color color in crestColors)
        {
            unusedArcherColors.Add(color);
        }
    }

    public static void ResetPeasantColors()
    {
        foreach (Color color in crestColors)
        {
            unusedPeasantColors.Add(color);
        }
    }

    public static void ResetKnightColors()
    {
        foreach (Color color in crestColors)
        {
            unusedKnightColors.Add(color);
        }
    }
}
