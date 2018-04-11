using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceControl : MonoBehaviour {

    [SerializeField]
    bool unlimitedResources = false;

    #region Salt
    // return true if sufficient
    public bool CheckSalt(int requiredAmount) 
    {
        // resource check
        if (requiredAmount > PlayerProfile.Singleton.SaltCurrent && !unlimitedResources)
        {
            return false;
        }
            
        // spend salt
        else
        {
            return true;
        }
    }
    #endregion
}
