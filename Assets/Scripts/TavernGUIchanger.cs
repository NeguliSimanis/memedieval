using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernGUIchanger : MonoBehaviour
{
	public Text ducatcount;    // Artis added 
    public Text saltcount;
    public GameObject[] layouts;
    
    public void changeLayout(int layoutID)
    {
        for(int i=0;i<layouts.Length;i++)
        {
            layouts[i].SetActive(false);
        }
        layouts[layoutID].SetActive(true);
    }

    public IEnumerator SetSaltCount()
    {
        
        while (true)
        {
            saltcount.text = PlayerProfile.Singleton.SaltCurrent.ToString();
            Debug.Log("displaying salt");
            yield return new WaitForSeconds(1);
        }
    }

	//For Ducat obtaining
	public IEnumerator SetDucatCount()
	{

		while (true)
		{
			ducatcount.text = PlayerProfile.Singleton.DucatCurrent.ToString();
			yield return new WaitForSeconds(1);
		}
	}


    private void Start()
    {
        StartCoroutine(SetSaltCount());
		StartCoroutine(SetDucatCount());
    }
}
