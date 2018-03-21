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
    private float defaultPriceCoefficient = 1f;
    public int minUnitPrice = 1;
    private float charmEffect = 0.01f;
    #endregion

    #region luck - ducat chance
    public float ducatFindChance = 0f;
    private float defaultDucatFindChance = 0f;
    private float maxLuckEffect = 1f;
    private float luckEffect = 0.02f;
    #endregion

    #region wisdom - more exp
    public int championFinalExpEarn = 10;
    private float playerExpCoefficient = 1f;
    private float defaultExpCoefficent = 1f;
    private int defaultExpEarn = 10;
    private float wisdomEffect = 0.05f;
    #endregion

    #region brawn - more unit hp
    public float playerUnitHPCoefficient = 1f;
    private float defaultPlayerUnitHPCoefficient = 1f;
    private float brawnEffect = 0.01f;
    #endregion

    #region wealth - more starting resources
    public float startingMeatCoefficient = 1f;
    private float defaultStartingMeatCoefficient = 1f;
    private float wealthEffect = 0.05f;
    #endregion

    #region discipline - less damage from castle arrows
    public float castleArrowDamageCoefficient = 1f;
    private float defaultCastleArrowDamageCoeffient = 1f;
    private float disciplineEffect = 0.95f;
    public int minCastleArrowDamage = 1;
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
        // reset charm
        priceCoefficient = defaultPriceCoefficient;

        // reset luck
        ducatFindChance = defaultDucatFindChance;
        
        // reset wisdom
        playerExpCoefficient = defaultExpCoefficent;

        // reset brawn
        playerUnitHPCoefficient = defaultPlayerUnitHPCoefficient;

        // reset wealth
        startingMeatCoefficient = defaultStartingMeatCoefficient;

        // reset discipline
        castleArrowDamageCoefficient = defaultCastleArrowDamageCoeffient;

        foreach (Champion champion in activeChampions)
        {
            champion.invitedToBattle = false;    
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
            totalLuck = totalLuck + champion.properties.luck;
        }

        float totalLuckEffect = totalLuck * luckEffect;
        ducatFindChance = ducatFindChance + totalLuckEffect;
        if (ducatFindChance > maxLuckEffect)
        {
            Debug.Log("Very high luck");
            ducatFindChance = maxLuckEffect;
        }
    }
    private void SetWisdomEffect()
    {
        int totalWisdom = 0;
        foreach (Champion champion in activeChampions)
        {
            totalWisdom = totalWisdom + champion.properties.wisdom;
        }
        playerExpCoefficient = playerExpCoefficient + (totalWisdom * wisdomEffect);
        championFinalExpEarn = Mathf.RoundToInt(defaultExpEarn * playerExpCoefficient);
    }

    private void SetBrawnEffect()
    {
        int totalBrawn = 0;
        foreach (Champion champion in activeChampions)
        {
            totalBrawn = totalBrawn + champion.properties.brawn;
        }
        playerUnitHPCoefficient = playerUnitHPCoefficient + (totalBrawn * brawnEffect);
    }
    private void SetWealthEffect()
    {
        int totalWealth = 0;
            foreach (Champion champion in activeChampions)
            {
                totalWealth = totalWealth + champion.properties.wealth;
            }
        startingMeatCoefficient = startingMeatCoefficient + (totalWealth * wealthEffect);
    }

    private void SetDisciplineEffect()
    {
        int totalDiscipline = 0;
        foreach (Champion champion in activeChampions)
        {
            totalDiscipline = totalDiscipline + champion.properties.discipline;
        }

        while (totalDiscipline > 0)
        {
            castleArrowDamageCoefficient = castleArrowDamageCoefficient * disciplineEffect;
            totalDiscipline--; 
        }
    }
    #endregion

}
