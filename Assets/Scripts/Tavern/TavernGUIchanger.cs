using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernGUIchanger : MonoBehaviour
{
    private ResourceControl resourceControl;

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

        // intialize variables
        tavernDialogueContol = this.gameObject.GetComponent<TavernDialogueControl>();
        resourceControl = PlayerProfile.Singleton.gameObject.GetComponent<ResourceControl>();
    }

    public void ChangeLayout(int layoutID)
    {     
        if (layouts[layoutID].gameObject.tag == statsPanelTag)
        {
            // player has no champions and tries to open champion stats - ignore request
            if (PlayerProfile.Singleton.champions.Count == 0)
            {
                tavernDialogueContol.SayNoChampions();
                return;
            }
        }

        
        else if (layouts[layoutID].gameObject.tag == championRecruitTag)
        {
            // player has too many champions - ignore request
            if (PlayerProfile.Singleton.champions.Count >= 5)
            {
                tavernDialogueContol.SayTooManyChampions();
                return;
            }

            // player resource check
            if (!resourceControl.CheckSalt(GameData.current.newChampionCost))
            {
                tavernDialogueContol.SayNotEnoughSalt();
                Debug.Log("required salt " + GameData.current.newChampionCost);
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
