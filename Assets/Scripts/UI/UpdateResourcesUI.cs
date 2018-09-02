using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateResourcesUI : MonoBehaviour {

    /// <summary>
    /// Updates how many resources the player has on the HUD
    /// 
    /// 29.07.2018 Sīmanis Mikoss
    /// </summary>
    /// <returns></returns>

    [SerializeField] private Text ducatCount;
    [SerializeField] private Text saltCount;
    [SerializeField] private Text meatCount; // added 01.09.2018

    void Start()
    {
        Debug.Log(gameObject.name);
        StartCoroutine(SetSaltCount());
        StartCoroutine(SetDucatCount());
        StartCoroutine(SetMeatCount());
    }

    public IEnumerator SetSaltCount()
    {
        while (true)
        {
            saltCount.text = PlayerProfile.Singleton.SaltCurrent.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator SetDucatCount()
    {
        while (true)
        {
            ducatCount.text = PlayerProfile.Singleton.DucatCurrent.ToString();
            yield return new WaitForSeconds(1);
        }
    }

    // added 01.09.2018
    public IEnumerator SetMeatCount()
    {
        while (true)
        {
            meatCount.text = PlayerProfile.Singleton.MeatCurrent.ToString();
            yield return new WaitForSeconds(1);
        }
    }
}
