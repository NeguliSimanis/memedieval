using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CastleHUB : MonoBehaviour {

    public Button TavernButton;
    public Text statusText;

    [SerializeField]
    private string battleSceneName = "Test scene";
    private string defaultText="";

    private string noChampions = "We have no armies to do battle, my liege.";


    [SerializeField]
    GameObject ChampSelectButtonPrefab;
    [SerializeField]
    GameObject ChampSelect;
    private List<GameObject> _champButtons = new List<GameObject>();
    [SerializeField]
    GameObject championSelectionText;

    

	void Start ()
    {
        var p = PlayerProfile.Singleton;
        if(string.IsNullOrEmpty(defaultText))
        {
            defaultText = statusText.text;
        }
        var s = PlayerProfile.Singleton.lastGameStatus;
        if(s>0)
        {
            statusText.text = "Most glorious victory, oh great leader. You get 10 SALT! What is Your next action?";
            PlayerProfile.Singleton.SaltCurrent += 10;
        }
        if(s<0)
        {
            statusText.text = "Valiant defeat! What is Your next action?";
        }
        PlayerProfile.Singleton.lastGameStatus = 0;

        for(int i = 0; i < p.champions.Count; i++)
        {
            var champ = p.champions[i];
            if (champ.properties.isDead)
            {
                p.champions.Remove(champ);
                Destroy(champ.gameObject);
            }
        }
	}
	

    public void onTavernHubButtonClick()
    {
        SceneManager.LoadScene("Tavern");
    }


    private void DisplayChampions()
    {
        statusText.enabled = false;
        championSelectionText.SetActive(true);

        foreach (Champion champ in PlayerProfile.Singleton.champions)
        {
            var buttonp = Instantiate(ChampSelectButtonPrefab);
            var button = buttonp.GetComponent<Image>();
            button.gameObject.SetActive(true);
            button.transform.SetParent(ChampSelect.transform);
            var rect = new Rect(0, 0, champ.properties.LoadPictureAsTexture2D().width, champ.properties.LoadPictureAsTexture2D().height);
            button.sprite = Sprite.Create(champ.properties.LoadPictureAsTexture2D(), rect, Vector2.zero);

            //buttonp.GetComponent<Button>().onClick.AddListener(() => { ChangeChamp(champ); });
            button.GetComponentInChildren<Text>().text = champ.properties.Name;
            _champButtons.Add(button.gameObject);
        }
    }

    private void ChooseChampions()
    {

    }

    public void onBattleClick()
    {
        if (PlayerProfile.Singleton.champions.Count == 0)
        {
            statusText.text = noChampions;
            return;
        }

        DisplayChampions();
        //SceneManager.LoadScene(battleSceneName);
    }

   
}
