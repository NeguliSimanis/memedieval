using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MechanicType
{
    None,

    // Map
    Map_SelectChampions,

    // Battle
    Battle_SummonChampion,
    Battle_UseChampionAbility,

    // Tavern
    Tavern_Enter, // enter tavern
    Tavern_Drink,
    Tavern_RecruitChampion,
    Tavern_AddSkillpoints,

    // Market
    Market_Enter,
}

public class Mechanic
{
    public int unlockCastleID;
    public MechanicType type;
    public bool isEnabled = false;
    public Mechanic(int mechanicUnlockCastle, MechanicType mechanicType)
    {
        unlockCastleID = mechanicUnlockCastle;
        type = mechanicType;
    }
}

public class UnlockMechanics
{
    public static List<Mechanic> allMechanics = new List<Mechanic>();

    private bool isUnlockingMechanics = false;
    public static UnlockMechanics current;

    // bool shows that game mechnic has been unlocked
	//public static Dictionary<MechanicType, bool> gameMechanics = new Dictionary<MechanicType, bool>();

    // int shows the castle ID, Mechanic shows the game mechanic that is unlocked by destroying it
    //public static Dictionary<int, MechanicType> castleUnlocksMechanic = new Dictionary<int, MechanicType>();
    
    // CONSTRUCTOR
    public UnlockMechanics()
    {
        SetupMechanicsList();
       // SetupGameMechanicsDictionary();
        //SetupUnlocksDictionary();
        
    }
    void SetupMechanicsList()
    {
        // Map
        allMechanics.Add(new Mechanic(0,MechanicType.Map_SelectChampions));

        // Battle
        allMechanics.Add(new Mechanic(0, MechanicType.Battle_SummonChampion));
        allMechanics.Add(new Mechanic(0, MechanicType.Battle_UseChampionAbility));

        // Tavern
        allMechanics.Add(new Mechanic(0, MechanicType.Tavern_Enter));
        allMechanics.Add(new Mechanic(0, MechanicType.Tavern_Drink));
        allMechanics.Add(new Mechanic(0, MechanicType.Tavern_RecruitChampion));
        allMechanics.Add(new Mechanic(0, MechanicType.Tavern_AddSkillpoints));

        // Market
        allMechanics.Add(new Mechanic(2, MechanicType.Market_Enter));
    }
    /*
    /// <summary>
    /// Initializes the values in dictionary that lists the castle IDs where the mechanic is unlocked
    /// </summary>
    private void SetupUnlocksDictionary()
    {
        
        castleUnlocksMechanic.Add(1, MechanicType.Tavern_Enter);
        //castleUnlocksMechanic.Add(1)
    }

    /// <summary>
    /// Initializes the values in dictionary that lists whether the particular mechanic has been unlocked
    /// </summary>
    private void SetupGameMechanicsDictionary()
    {
        // get all enum values
        var mechanics = System.Enum.GetValues(typeof(MechanicType));

        // initialize them in game mechanics dictionary
        foreach (MechanicType mechanic in mechanics)
        {
            gameMechanics.Add(mechanic, false);
        }
    }*/

    /// <summary>
    /// called when you destroy a castle
    /// </summary>
    public void Unlock (int destroyedCastleID)
    {
        Debug.Log("Destroyed castle id: " + destroyedCastleID);

        //MechanicType mechanic;

        // castle doesn't unlock anything
        foreach (Mechanic mechanic in allMechanics)
        {
            if (mechanic.unlockCastleID == destroyedCastleID)
            {
                Debug.Log("Castle unlocks " + mechanic.type);
                mechanic.isEnabled = true;
            }
        }
	}

    /// <summary>
    ///  unlocks all mechanics - used for testing
    /// </summary>
    public void UnlockAll()
    {
        if (isUnlockingMechanics)
            return;
        isUnlockingMechanics = true;
        foreach (Mechanic mechanic in allMechanics)
        {
            mechanic.isEnabled = true;
        }
    }

    /*public int GetMechanicUnlockID(MechanicType mechanic)
    {
        //int temp = castleUnlocksMechanic.FirstOrDefault(x => x.Value == "one").Key;
        foreach (KeyValuePair<int, MechanicType> currentMechanic in castleUnlocksMechanic)
        {
            if (currentMechanic.Value == mechanic)
            {
                return currentMechanic.Key;
            }
        }
        return -1;
    }*/

    void GetCurrentGameData()
    {
        if (GameData.current == null)
        {
            GameData.current = new GameData();
        }
    }


    public Mechanic FindMechanicByType(MechanicType mechanicType)
    {
        Mechanic temp = new Mechanic(-1, MechanicType.None);
        foreach (Mechanic currentMechanic in allMechanics)
        {
            if (currentMechanic.type == mechanicType)
            {
                return currentMechanic;
            }
        }
        return temp;
    }

    public bool CheckIfUnlocked(MechanicType mechanicType)
    {
        Mechanic currentMechanic = FindMechanicByType(mechanicType);
        if (currentMechanic.isEnabled)
            return true;

        /* mechanic is locked - it's possible that player has loaded game with some destroyed castles
         * so we must check game data if the highest destroyed castle number allows this mechanic */
        GetCurrentGameData();
        if (currentMechanic.unlockCastleID <= GameData.current.HighestDestroyedCastleID())
        {
            currentMechanic.isEnabled = true;
            return true;
        }
        return false;
    }
    /*public bool CheckIfUnlocked(MechanicType mechanic)
    {
        bool isUnlocked = false;

        // first look up in dictionary
        gameMechanics.TryGetValue(mechanic, out isUnlocked);

        /* mechanic is locked - it's possible that player has loaded game with some destroyed castles
         * so we must check game data if the highest destroyed castle number allows this mechanic */
    /*if (isUnlocked == false)
    {
        GetCurrentGameData();
        if (GetMechanicUnlockID(mechanic) <= GameData.current.HighestDestroyedCastleID())
        {
            gameMechanics[mechanic] = true;
            isUnlocked = true;
        }
    }
    return isUnlocked;
}*/
}
