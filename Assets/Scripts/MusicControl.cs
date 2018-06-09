using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicControl : MonoBehaviour {



    private GameObject musicObject;
	private Health enemyCastleHealth;
    private Health playerCastleHealth;
    private FMODUnity.StudioEventEmitter music;

	[SerializeField] private FMODUnity.StudioEventEmitter victory;
	[SerializeField] private FMODUnity.StudioEventEmitter lose;

    void Awake()
    {

        
    }



    void Start () 
	{


//        DontDestroyOnLoad(gameObject);

        music = GetComponent<FMODUnity.StudioEventEmitter> ();

		music.SetParameter ("Transition", 0f);

        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }
	
	// Update is called once per frame
	void Update ()
    {

        
        BattleMonitorForChange();

		
    }
//    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
//    {
//        //throw new System.NotImplementedException();
//
//        if (scene.name == "Castle")
//        {
//            return;
//        }
//
//        else
//        {
//            Destroy(gameObject);
//            Debug.Log("Music Cleared");
//
//        }
//    }
//    private void OnDestroy()
//    {
//        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
//    }
    private void BattleMonitorForChange()
    {
        try
        {
            if (GameObject.FindGameObjectWithTag("EnemyCastle").activeInHierarchy)
            {

                enemyCastleHealth = GameObject.FindGameObjectWithTag("EnemyCastle").GetComponent<Health>();


                if (enemyCastleHealth.CurrentHealth > enemyCastleHealth.MaximumHealth * 0.90f && enemyCastleHealth.CurrentHealth < enemyCastleHealth.MaximumHealth)
                    music.SetParameter("Transition", 1f);
                if (enemyCastleHealth.CurrentHealth > enemyCastleHealth.MaximumHealth * 0.60f && enemyCastleHealth.CurrentHealth < enemyCastleHealth.MaximumHealth * 0.89f)
                    music.SetParameter("Transition", 2f);
                if (enemyCastleHealth.CurrentHealth > enemyCastleHealth.MaximumHealth * 0.10f && enemyCastleHealth.CurrentHealth < enemyCastleHealth.MaximumHealth * 0.59f)
                    music.SetParameter("Transition", 3f);
                if (enemyCastleHealth.CurrentHealth <= 0)
					Debug.Log("Koolt2");
					victory.Play();

            }
            if (GameObject.FindGameObjectWithTag("Player castle").activeInHierarchy)
            {
                playerCastleHealth = GameObject.FindGameObjectWithTag("Player castle").GetComponent<Health>();

                if (playerCastleHealth.CurrentHealth <= 0 && enemyCastleHealth.CurrentHealth > 0)
                {
					Debug.Log("Koolt");
					//lose.Play();
                }
            }
        }
        catch (System.NullReferenceException)
        {
            return;
        }
    }



}
