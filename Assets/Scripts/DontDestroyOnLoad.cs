using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    [SerializeField] public bool keepChildren = false;

    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);

        if (keepChildren)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<DontDestroyOnLoad>();
            }
        }
    }
}
