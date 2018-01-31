using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleFaceCreate : MonoBehaviour {
    public AvatarFace avatarFacePrefab;
    public Texture2D defaultFace;
    // Use this for initialization
    void Start () {

            createAvatarFace(0, new Vector3(7.25f, 3.59f, -0.57f));
            createAvatarFace(1, new Vector3(7.25f, 0.4f, -0.57f));
            createAvatarFace(2, new Vector3(7.25f, -2.79f, -0.57f));
    }

    public void createAvatarFace( int class1, Vector3 pos)
    {
        AvatarFace face;
        var c = PlayerProfile.Singleton.champions.Where(x => x.properties.champClass == class1).FirstOrDefault();
        Texture2D facet = null;
        if (c != null)
        {
            facet = c.properties.LoadPictureAsTexture2D();
        }
        else
        {
            facet = defaultFace;
        }
        face = Instantiate(avatarFacePrefab);
        face.transform.position = pos;
        face.SetFace(facet);
        //face.transform.localPosition = new Vector3(-0.2f, 2.5f, 0);
        face.transform.localScale = new Vector3(0.015f, 0.015f, 0);
    }
}
