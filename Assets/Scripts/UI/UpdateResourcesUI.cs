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

    void Start()
    {
        StartCoroutine(SetSaltCount());
        StartCoroutine(SetDucatCount());
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
}
