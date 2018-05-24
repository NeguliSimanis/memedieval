using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Flips texture2d vertically
 * Based on code taken from here: https://gist.github.com/grimmdev/c494d54526146d1d16db
 */

public class FlipTexture2D : MonoBehaviour{

    public Texture2D FlipText(Texture2D original)
    {
        // We create a new texture so we don't change the old one!
        Texture2D flip = new Texture2D(original.width, original.height);

        // These for loops are for running through each individual pixel and then replacing them in the new texture.
        /*for (int i = 0; i < flip.width; i++)
        {
            for (int j = 0; j < flip.height; j++)
            {
                flip.SetPixel(flip.width - i - 1, j, original.GetPixel(i, j));
            }
        }*/

        for (int i = 0; i < flip.width; i++)
        {
            for (int j = 0; j < flip.height; j++)
            {
                flip.SetPixel(flip.width - i - 1, flip.height - j -1, original.GetPixel(i, j));
            }
        }

        // We apply the changes to our new texture
        flip.Apply();
        // Then we send it on our marry little way!
        return flip;
    }
}
