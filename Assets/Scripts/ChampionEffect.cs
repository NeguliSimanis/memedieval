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

    #region unit prices
    int minUnitPrice = 1;
    #endregion

    public void SetTotalEffect()
    {
        SetCharmEffect();
        SetLuckEffect();
        SetWisdomEffect();
        SetWealthEffect();
        SetWisdomEffect();
        SetDisciplineEffect();
    }

    #region skill-specific functions
    private void SetCharmEffect()
    {

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
