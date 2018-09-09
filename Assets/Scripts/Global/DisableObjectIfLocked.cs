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
   
    private void Start()
    {
        if (UnlockMechanics.current == null)
        {
            UnlockMechanics.current = new UnlockMechanics();
        }
        if (!UnlockMechanics.current.CheckIfUnlocked(mechanic))
        {
            gameObject.SetActive(false);
        }
    }
}
