using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTap : MonoBehaviour {

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
