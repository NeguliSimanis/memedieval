using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Sets the tent color and pattern in battle according to Champions crest
/// 
/// 05.10.2018 Sīmanis Mikoss
/// </summary>
public class TentCrest : MonoBehaviour
{
    public ChampionData championData;
    [SerializeField]
    Image tentImage;    

	void Start ()
    {
        SetTentColor();
        SetTentPattern();
	}

    void SetTentPattern()
    {
        for (int i = 0; i < Crests.crestPatternCount; i++)
        {
            if (i == championData.crestPatternID)
            {
                tentImage.gameObject.transform.GetChild(i).transform.gameObject.SetActive(true);
            }
            else
            {
                tentImage.gameObject.transform.GetChild(i).transform.gameObject.SetActive(false);
            }
        }
    }

    void SetTentColor()
    {
        tentImage.color = championData.crestColor;
    }
    
}
