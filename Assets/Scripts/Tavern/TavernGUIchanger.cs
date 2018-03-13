using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernGUIchanger : MonoBehaviour
{
	public Text ducatcount;    
    public Text saltcount;
    public GameObject[] layouts;

    private TavernDialogueControl tavernDialogueContol;
    private string statsPanelTag = "Tavern champion stats";
    private string championRecruitTag = "Tavern champion recruit";

    private void Start()
    {
        StartCoroutine(SetSaltCount());
        StartCoroutine(SetDucatCount());
        tavernDialogueContol = this.gameObject.GetComponent<TavernDialogueControl>();
    }

    public void ChangeLayout(int layoutID)
    {
        // player has no champions and tries to open champion stats - ignore request
        if (layouts[layoutID].gameObject.tag == statsPanelTag)
        {
            if (PlayerProfile.Singleton.champions.Count == 0)
            {
                tavernDialogueContol.SayNoChampions();
                return;
            }
        }

        // player has too many champions - ignore request
        else if (layouts[layoutID].gameObject.tag == championRecruitTag)
        {
            if (PlayerProfile.Singleton.champions.Count >= 5)
            {
                tavernDialogueContol.SayTooManyChampions();
                return;
            }
              
        }

        // valid request
        for(int i=0;i<layouts.Length;i++)
        {
            layouts[i].SetActive(false);
        }
        layouts[layoutID].SetActive(true);
        tavernDialogueContol.ResetDialogue();
    }

    public IEnumerator SetSaltCount()
    {
        
        while (true)
        {
            saltcount.text = PlayerProfile.Singleton.SaltCurrent.ToString();
            yield return new WaitForSeconds(1);
        }
    }

	public IEnumerator SetDucatCount()
	{
		while (true)
		{
			ducatcount.text = PlayerProfile.Singleton.DucatCurrent.ToString();
			yield return new WaitForSeconds(1);
		}
	}

}
