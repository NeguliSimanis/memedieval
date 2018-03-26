using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomName : MonoBehaviour {

    [SerializeField]
    InputField inputField;
    Button randomNameButton;

    #region possible names
    string [] names =
    {
        "Hildergard",
        "William",
        "Godfrey",
        "Eleanor",
        "Hereward",
        "Lupus",
        "Ranulf",
        "Marie",
        "Simon",
        "Sacristan",
        "Geoffrey",
        "Joan",
        "Jean",
        "Marie",
        "Iseult",
        "Tristan",
        "Lohengrin",
        "Parzival",
        "Enide",
        "Hugh",
        "Guy",
        "Gerald",
        "Lachlan",
        "Elyas",
        "Dante",
        "Levi",
        "Eva",
        "Heloïse ",
        "Anselm"
    };
    #endregion

    #region possible surnames
    string[] surnames =
    {
        "Von Bingen",
        "Magnus",
        "of Aquitaine",
        "Lupus",
        "the Bald",
        "De Montfort",
        "Ouzel",
        "Borzof",
        "Smotrof",
        "of Gaunt",
        "the Venerable",
        "Tell",
        "of Valois",
        "The Bruce",
        "Petrarch",
        "Caedmon",
        "Tyndale",
        "de Perigors",
        "of Torroja",
        "of Canet",
        "Marescalci",
        "of Belmonte",
        "von Schluck",
        "Jeger",
        "Boez",
        "Abelard",
        "Martel",
    };
    #endregion

    void Start ()
    {
        randomNameButton = this.gameObject.GetComponent<Button>();
        randomNameButton.onClick.AddListener(RandomizeName);
	}

    void RandomizeName()
    {
        inputField.text = Generate();
    }

    string Generate()
    {
        string fullName;
        string name = names[Random.Range(0, names.Length)];
        string surname = surnames[Random.Range(0, surnames.Length)];
        fullName = name + " " + surname;
        return fullName;
    }
}
