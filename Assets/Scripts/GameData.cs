using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script:
 * > stores all data that has to be saved/loaded 
 * > converts the loadable variables to a serializable format
 */

[System.Serializable]
public class GameData
{
    public static GameData current;
    public List<ChampionData> championList = new List<ChampionData>();

    #region resources
    public int salt = 0;
    public int ducats = 0;
    #endregion
}

[System.Serializable]
public class ChampionData
{
    //public bool onBattle; - no need to save this
    public string Name;
    public bool isMan; 
    public int level;   
    public int champClass;
    public string bio;
    public string quote;
    public bool isDead;

    #region Picture stuff
    public byte[] picture;
    private int pictureWidth = 1280;
    private int pictureHeight = 720;
    private int pictureMipmapCount = 11;
    private TextureFormat pictureFormat = TextureFormat.RGBA32;

    #region skills
    public int skillpoints = 3;
    public int charm = 0;
    public int discipline = 0;
    public int brawn = 0;
    public int wisdom = 0;
    public int luck = 0;
    public int wealth = 0;
    #endregion

    public void SetPicture(Texture2D texture)
    {
       /* pictureWidth = texture.width;
        pictureHeight = texture.height;
        pictureFormat = texture.format;
        Debug.Log("picture width " + pictureWidth);
        Debug.Log("picture height " + pictureHeight);
        Debug.Log("picture Format " + pictureFormat);
        Debug.Log("mipmap count " + texture.mipmapCount);*/

        picture = texture.GetRawTextureData();
    }

    public Texture2D LoadPictureAsTexture2D()
    {
        Texture2D tex = new Texture2D(pictureWidth, pictureHeight, pictureFormat, false);
        tex.LoadRawTextureData(picture);
        tex.Apply();
        return tex;
    }
    #endregion
}
