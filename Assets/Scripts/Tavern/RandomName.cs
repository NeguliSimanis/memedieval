using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomName : MonoBehaviour {

    /*
     * Generates a random full name from a given list of names and surnames
     * List of names available here: https://docs.google.com/spreadsheets/d/1bU8xhprB7hKEfMUuGSFNypsKsXxkt5fWZ_A3n-66lDU/edit#gid=0
     */

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
        "Heloise ",
        "Anselm",
        "Lambert",
        "Raphael",
        "Cassius",
        "Gwendolynn",
        "Garett",
        "Garth",
        "Isolde",
        "Sibyl",
        "Drake",
        "Cadwaladr",
        "Thurstan",
        "Hadrian",
        "Brom",
        "Ulric",
        "Helewys",
        "Isolde"
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
        "of Warwick",
        "Holinshed",
        "Peronell",
        "Muller",
        "Drake",
        "ap Gruffydd",
        "Gwynedd",
        "de Clare",
        "of Aragon",
        "Ceredigion",
        "Regiomontanus",
        "of Bayeux",
        "Montagu",
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
