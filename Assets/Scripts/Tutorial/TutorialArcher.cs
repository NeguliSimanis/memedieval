using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArcher : MonoBehaviour {

    [SerializeField]
    Tutorial tutorial;

    void Start()
    {
        Debug.Log("for sparta");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player unit")
        {
            Debug.Log("TUTORIAL COMPLETE");
            tutorial.DefeatTutorialEnemy(this.gameObject);
        }
    }
}
