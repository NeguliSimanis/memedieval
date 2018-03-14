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

    List<Champion> activeChampions = new List<Champion>();

    public void SetTotalEffect()
    {
        FindActiveChampions();
        SetCharmEffect();
        SetLuckEffect();
        SetWisdomEffect();
        SetWealthEffect();
        SetWisdomEffect();
        SetDisciplineEffect();
    }

    private void FindActiveChampions()
    {
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.invitedToBattle)
            {
                activeChampions.Add(champion);
                Debug.Log(champion.properties.Name);
            }
        }
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
         Debug.Log("Price coefficient is " + priceCoefficient);
        
        
    }
    private void SetLuckEffect()
    {

    }
    private void SetWisdomEffect()
    {

    }
    private void SetWealthEffect()
    {

    }
    private void SetDisciplineEffect()
    {

    }
    #endregion

    #region functions for specific effects
    private void SetUnitDiscount()
    {

    }
    #endregion
}
