using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitTents : MonoBehaviour
{
    #region variables
    [SerializeField]
    private MeMedieval.Resources resources;

    [Header("Tents")]
    [SerializeField]
    GameObject knightTent;
    [SerializeField]
    GameObject archerTent;
    [SerializeField]
    GameObject peasantTent;
    #endregion

    void Start ()
    {
		// go through all player champions
        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.invitedToBattle == true)
            {
                SpawnTent(champion.properties.GetChampionAttackType(), champion.properties.championID, champion.properties);
                //Debug.Log("Adding champion " + champion.properties.GetFirstName() + " with ability " + champion.properties.GetAbilityString() + " and id " + champion.properties.championID);
                //Debug.Log("Adding champion with id " + champion.GetID());
            }
        }
	}

    void SpawnTent(Attack.Type unitType, int championID, ChampionData championData)
    {
        GameObject tent;
        if (unitType == Attack.Type.Archer)
        {
            tent = archerTent;
        }
        else if (unitType == Attack.Type.Knight)
        {
            tent = knightTent;
        }
        else //(unitType == Attack.Type.Peasant)
        {
            tent = peasantTent;
        }
        SetTentCrest(tent, championData);
        Instantiate(tent, transform);

        // set champion ID for the button that summmons champions. NB - if the child number is different, script won't work
       // Debug.Log("adding tent with name" + tent.transform.GetChild(1).gameObject.name + " and with champion ID " + championID);
        
        tent.transform.GetChild(1).gameObject.GetComponent<PlayerUnitSpawn>().SetChampionID(championID);
    }

    void SetTentCrest(GameObject tent, ChampionData championData)
    {
        if (tent.GetComponent<TentCrest>() != null)
            tent.GetComponent<TentCrest>().championData = championData;
    }
}
