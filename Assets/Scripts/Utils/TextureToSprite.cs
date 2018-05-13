using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureToSprite {

    public static Sprite LoadPictureAsSprite(Texture2D newTexture2D)
    {
        Sprite newSprite = Sprite.Create(newTexture2D, new Rect(0.0f, 0.0f, newTexture2D.width, newTexture2D.height), new Vector2(0.5f, 0.5f), 100.0f);
        return newSprite;
    }
}
