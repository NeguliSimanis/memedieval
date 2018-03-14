using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionEffect : MonoBehaviour {

    /*   
    CHAMPION SKILL EFFECTS
    Charm - Cheaper units
    Luck - Chance to obtain ducat after battle
    Wisdom - More exp from battles
    Brawn - More hp to units
    Wealth - More starting resources in battle
    Discipline - Less damage from castle arrows
    */

    #region charm - unit prices
    public float priceCoefficient = 1f;
    public int minUnitPrice = 1;
    private float charmEffect = 0.01f;
    #endregion

    #region luck - ducat chance
    public float ducatFindChance = 0f;
    private float maxLuckEffect = 1f;
    private float luckEffect = 0.02f;
    #endregion
    List<Champion> activeChampions = new List<Champion>();

    public void SetTotalEffect()
    {
        FindActiveChampions();
        SetCharmEffect();
        SetLuckEffect();
        SetWisdomEffect();
        SetBrawnEffect();
        SetWealthEffect();
        SetDisciplineEffect();
    }

    private void FindActiveChampions()
    {
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.invitedToBattle)
            {
                activeChampions.Add(champion);
                //Debug.Log(champion.properties.Name);
            }
        }
    }

    public void ResetChampionEffect()
    {
        ducatFindChance = 0f;
        foreach (Champion champion in activeChampions)
        {
            champion.invitedToBattle = false;
            Debug.Log(champion.properties.Name + " removed from active champions");         
        }
        activeChampions.Clear();
    }

    #region skill-specific functions
    // the actual effect is implemented in Spawn script
    private void SetCharmEffect()
    {
        int totalCharm = 0;
        foreach (Champion champion in activeChampions)
        {
            totalCharm = totalCharm + champion.properties.charm;
        }
         priceCoefficient = priceCoefficient - (totalCharm * charmEffect); 
    }
    private void SetLuckEffect()
    {
        int totalLuck = 0;
        foreach (Champion champion in activeChampions)
        {
            totalLuck = totalLuck + champion.properties.charm;
        }

        float totalLuckEffect = totalLuck * luckEffect;
        Debug.Log("total luck effect in championeffect" + totalLuckEffect);
        ducatFindChance = ducatFindChance + totalLuckEffect;
        Debug.Log("ducat find chance in championeffect " + ducatFindChance);
        if (ducatFindChance > maxLuckEffect)
        {
            Debug.Log("Very high luck");
            ducatFindChance = maxLuckEffect;
        }
    }
    private void SetWisdomEffect()
    {

    }

    private void SetBrawnEffect()
    {

    }
    private void SetWealthEffect()
    {

    }
    private void SetDisciplineEffect()
    {

    }
    #endregion

}
