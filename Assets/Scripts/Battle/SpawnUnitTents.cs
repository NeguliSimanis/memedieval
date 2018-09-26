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
                SpawnTent(champion.properties.GetChampionAttackType(), champion.properties.championID);
                Debug.Log("Adding champion " + champion.properties.GetFirstName() + " with ability " + champion.properties.GetAbilityString() + " and id " + champion.properties.championID);
                //Debug.Log("Adding champion with id " + champion.GetID());
            }
        }
	}

    void SpawnTent(Attack.Type unitType, int championID)
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
        Instantiate(tent, transform);
        tent.transform.GetChild(0).gameObject.GetComponent<PlayerUnitSpawn>().championID = championID;
        tent.transform.GetChild(1).gameObject.GetComponent<PlayerUnitSpawn>().championID = championID;
    }
}
