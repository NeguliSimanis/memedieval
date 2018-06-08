using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionSkillGenerator : MonoBehaviour {

    /*
     * ABOUT:
     * script gives new champions random skills
     */

    private Champion champion;

    // ID's same as in CreateChampion
    private int archerID = 0;
    private int knightID = 1;
    private int peasantID = 2;

    private int[] charmMinValues = { 0, -1, -1 };
    private int[] charmMaxValues = { 10, 8, 5 };

    private int[] luckMinValues = { 1, 0, -1 };
    private int[] luckMaxValues = { 10, 8, 5 };

    private int[] wisdomMinValues = { 1, 0, 1 };
    private int[] wisdomMaxValues = { 6, 5, 13 };

    private int[] brawnMinValues = { -1, 2, 1 };
    private int[] brawnMaxValues = { 4, 12, 8 };

    private int[] wealthMinValues = { 0, 1, -1 };
    private int[] wealthMaxValues = { 7, 10, 14 };

    private int[] disciplineMinValues = { 1, 0, -1 };
    private int[] disciplineMaxValues = { 5, 12, 4 };

    private int[] minTotalSkills = { 4, 6, 3 };
    private int[] maxTotalSkills = { 12, 12, 14 };

    public void GenerateChampionSkills(Champion generatedChampion)
    {
        champion = generatedChampion;
        champion.properties.skillpoints = 1;

        switch (champion.properties.champClass)
        {
            case 0: // archer
                SetArcherSkills();
                break;
            case 1: // knight
                SetKnightSkills();
                break;
            case 2: // peasant
                SetPeasantSkills();
                break;

            default: // error
                Debug.Log("champion skill generation error");
                return;
                break;               
        }
    }

    void SetArcherSkills()
    {
        // determine total skillpoint amount
        int totalSkillpoints = Random.Range(minTotalSkills[archerID], maxTotalSkills[archerID] + 1);

        // allocate dominant skills - charm, luck
        champion.properties.charm = ChooseSkillValue(charmMinValues[archerID], charmMaxValues[archerID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.charm;

        champion.properties.luck = ChooseSkillValue(luckMinValues[archerID], luckMaxValues[archerID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.luck;

        // allocate regular skills - wisdom, wealth
        champion.properties.wisdom = ChooseSkillValue(wisdomMinValues[archerID], wisdomMaxValues[archerID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.wisdom;

        champion.properties.wealth = ChooseSkillValue(wealthMinValues[archerID], wealthMaxValues[archerID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.wealth;

        // allocate minor skills - discipline, brawn
        champion.properties.discipline = ChooseSkillValue(disciplineMinValues[archerID], disciplineMaxValues[archerID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.discipline;

        champion.properties.brawn = ChooseSkillValue(brawnMinValues[archerID], brawnMaxValues[archerID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.brawn;
    }

    int ChooseSkillValue(int minValue, int maxValue, int remainingSkillpoints)
    {
        int skillValue = Random.Range(minValue, maxValue+1);
        if (skillValue > remainingSkillpoints)
        {
            while (skillValue > remainingSkillpoints)
            {
                //if (skillValue < minValue) Debug.Log("break doesn't work");
                skillValue--;
                if (skillValue == minValue) break;
            }
        }
       // if (skillValue < minValue) skillValue = minValue;
        return skillValue;
    }

    void SetKnightSkills()
    {
        int totalSkillpoints = Random.Range(minTotalSkills[knightID], maxTotalSkills[knightID] + 1);

        // allocate dominant skills - brawn, discipline
        champion.properties.brawn = ChooseSkillValue(brawnMinValues[knightID], brawnMaxValues[knightID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.brawn;

        champion.properties.discipline = ChooseSkillValue(disciplineMinValues[knightID], disciplineMaxValues[knightID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.discipline;

        // allocate regular skills - wealth, charm
        champion.properties.wealth = ChooseSkillValue(wealthMinValues[knightID], wealthMaxValues[knightID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.wealth;

        champion.properties.charm = ChooseSkillValue(charmMinValues[knightID], charmMaxValues[knightID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.charm;

        // allocate minor skills - wisdom, luck
        champion.properties.wisdom = ChooseSkillValue(wisdomMinValues[knightID], wisdomMaxValues[knightID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.wisdom;

        champion.properties.luck = ChooseSkillValue(luckMinValues[knightID], luckMaxValues[knightID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.luck;

    }

    void SetPeasantSkills()
    {
        int totalSkillpoints = Random.Range(minTotalSkills[peasantID], maxTotalSkills[peasantID] + 1);

        // allocate dominant skills - wealth, wisdom
        champion.properties.wealth = ChooseSkillValue(wealthMinValues[peasantID], wealthMaxValues[peasantID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.wealth;

        champion.properties.wisdom = ChooseSkillValue(wisdomMinValues[peasantID], wisdomMaxValues[peasantID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.wisdom;

        // allocate regular skills - brawn, luck
        champion.properties.brawn = ChooseSkillValue(brawnMinValues[peasantID], brawnMaxValues[peasantID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.brawn;

        champion.properties.luck = ChooseSkillValue(luckMinValues[peasantID], luckMaxValues[peasantID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.luck;

        // allocate minor skills - charm, discipline
        champion.properties.charm = ChooseSkillValue(charmMinValues[peasantID], charmMaxValues[peasantID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.charm;

        champion.properties.discipline = ChooseSkillValue(disciplineMinValues[peasantID], disciplineMaxValues[peasantID], totalSkillpoints);
        totalSkillpoints = totalSkillpoints - champion.properties.discipline;
    }
}
