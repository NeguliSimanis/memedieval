using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the logic for spawning player champions in battle.
/// Together with PlayerUnitSpawn attached to these game objects:
///         N/A
/// 
/// Part of the logic was originally in the Spawn.cs script.
/// 
/// 05.08.2018 Sīmanis Mikoss
/// </summary>
/// 
public class PlayerChampionSpawn : MonoBehaviour {

    private bool isChampionDying = false;

    [SerializeField] private Attack.Type captain;
    [SerializeField] private Button championButton;

    public void SpawnChampion()
    {
        if (captain == Attack.Type.Archer)
        {
            StartDisablingChampionButt();
        }

        if (captain == Attack.Type.Knight)
        {
            StartDisablingChampionButt();
        }

        if (captain == Attack.Type.Peasant)
        {
            StartDisablingChampionButt();
        }
    }

    private void StartDisablingChampionButt()
    {
        championButton.interactable = false;
        gameObject.GetComponent<PlayerChampionSpawn>().isChampionDying = true;
    }
}
