using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjectIfLocked : MonoBehaviour
{
    /// <summary>
    /// The game mechanic represented by this object
    /// </summary>
    [SerializeField]
    MechanicType mechanic;
    [SerializeField]
    bool disableIfLocked = true; // if false, the object is HIDDEN when you DEFEAT a castle

    private void Start()
    {
        CreateUnlockMechanics();

        #region Defeating a castle S H O W S this mechanic
        if (!UnlockMechanics.current.CheckIfUnlocked(mechanic) && disableIfLocked)
        {
            gameObject.SetActive(false);
        }
        #endregion
        #region Defeating a castle H I D E S this mechanic
        else if (!disableIfLocked)
        {
            // castle destroyed - hide
            if (UnlockMechanics.current.CheckIfUnlocked(mechanic))
                gameObject.SetActive(false);
            // castle not destroyed - show
            else
                gameObject.SetActive(true);
        }
        #endregion
    }

    void CreateUnlockMechanics()
    {
        if (UnlockMechanics.current == null)
        {
            UnlockMechanics.current = new UnlockMechanics();
        }
    }
}
