using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
///     Adds exp to all champions who fought after the battle
///     Author: Sīmanis Mikoss
/// 
/// </summary>

public class ExpController : MonoBehaviour {

    bool isExpAllocated = false;
    public void AllocateChampionExp()
    {
        if (isExpAllocated)
        {
            return;
        }
        isExpAllocated = true;

        ChampionEffect championEffect = PlayerProfile.Singleton.gameObject.GetComponent<ChampionEffect>();

        foreach (Champion champion in PlayerProfile.Singleton.champions)
        {
            if (champion.invitedToBattle == true)
            {
                champion.EarnExp(championEffect.championFinalExpEarn);
                champion.onBattle = false;
            }
        }
    }
}
