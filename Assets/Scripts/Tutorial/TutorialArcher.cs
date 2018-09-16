using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArcher : MonoBehaviour {

    [SerializeField] Tutorial tutorial;
    [SerializeField] Animator deathAnimator;
    bool isDead = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player unit")
        {
            tutorial.DefeatTutorialEnemy();
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;
        isDead = true;

        deathAnimator.SetTrigger("Die");

        /*Vector3 deathLocation = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        GameObject deathObjectParent = Instantiate(new GameObject(), deathLocation, Quaternion.identity);
        deathObjectParent.name = "EnemyTutorialArcherDeathAnimation";
        GameObject deathObject = Instantiate(deathAnimation, deathObjectParent.transform);
        deathObject.SetActive(true);

        gameObject.SetActive(false);*/
    }
}
