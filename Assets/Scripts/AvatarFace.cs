using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AvatarFace : MonoBehaviour {

    public Image FaceImage;
	public void SetFace(Texture2D t)
    {
        FaceImage.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
    }
}
