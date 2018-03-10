using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernCustomizeUI : MonoBehaviour {

    /*
     * script switches between different tabs when viewing owned champions
     */

    public GameObject SkillsContainer, UnitsContainer, BattleStatsContainer;

	public void ButtonSkillsClick()
    {
        //Skills tabu ieslēdz, pārejos izslēdz
        UnitsContainer.SetActive(false);
        BattleStatsContainer.SetActive(false);
        SkillsContainer.SetActive(true);
    }

    public void ButtonUnitsClick()
    {
        //Skills tabu ieslēdz, pārejos izslēdz
        UnitsContainer.SetActive(true);
        BattleStatsContainer.SetActive(false);
        SkillsContainer.SetActive(false);
    }

    public void ButtonStatsClick()
    {
        //Skills tabu ieslēdz, pārejos izslēdz
        UnitsContainer.SetActive(false);
        BattleStatsContainer.SetActive(true);
        SkillsContainer.SetActive(false);
    }
}
