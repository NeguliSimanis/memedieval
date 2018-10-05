using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This tool is very helpful: http://www.easyrgb.com/en/convert.php
/// </summary>
public class Crests
{
    public static int crestPatternCount = 2; // depends on how many crest pattern child objects you set in scene
    public static bool crestColorsSet = false;
    public static List<Color> crestColors = new List<Color>();
    public static List<Color> unusedArcherColors = new List<Color>();
    public static List<Color> unusedKnightColors = new List<Color>();
    public static List<Color> unusedPeasantColors = new List<Color>();

    public static void SetHeraldColors()
    {
        crestColors.Add(new Color(0.84314f, 0.82353f, 0.00000f));   // YELLOW
        crestColors.Add(new Color(0.86275f, 0.15686f, 0.11765f));   // RED
        crestColors.Add(new Color(0f, 0.22353f, 0.65098f));         // BLUE
        crestColors.Add(new Color(0f, 0.60392f, 0.23922f));         // GREEN
        crestColors.Add(new Color(1f, 0.47451f, 0f));               // ORANGE
        crestColors.Add(new Color(0.71373f, 0.03922f, 0.60784f));   // PURPLE

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
