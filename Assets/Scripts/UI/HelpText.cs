using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpText : MonoBehaviour {

    [SerializeField]
    Button nextButton;
    [SerializeField]
    Button prevButton;

    [SerializeField]
    Text subtitleText;
    [SerializeField]
    Text bodyText;

    int manualLength = 0;
    int currentManualID = 0;

    List<HelpTextComponent> gameManual = new List<HelpTextComponent>();

    void Start ()
    {
        LoadHelpTexts();      
        UpdateHelpText();
        SetupButtons();
        Debug.Log("manual length " + manualLength);
        Debug.Log("current manual id " + currentManualID);
    }

    void UpdateHelpText()
    {
        subtitleText.text = gameManual[currentManualID].subtitle;
        bodyText.text = gameManual[currentManualID].textBody;
    }

    void SetupButtons()
    {

        if (currentManualID < manualLength)
        {
            nextButton.interactable = true;
        }
        prevButton.onClick.AddListener(Prev);
        nextButton.onClick.AddListener(Next);

    }

    void LoadHelpTexts()
    {
        gameManual.Add(new HelpTextComponent("Basics", "Your tasks is to take back the kingdom from the hands of traitors. This will require an army of loyal champions.\n\nEach castle you take back will reveal the path to a new one."));
        gameManual.Add(new HelpTextComponent("Champions", "Champions are the backbone of your army. If you take a champion to battle, his skills strengthen your forces. Each champion has an ability that can be used in battle."));
        gameManual.Add(new HelpTextComponent("Units", "Archers deal additional damage against Peasants.\n\nPeasants deal additional damage against Knights.\n\nKnights deal additional damage against Archers."));
        gameManual.Add(new HelpTextComponent("Battle", "Click on a unit to make them run faster for a shorter duration.\nClick on a champion once to start charging their ability.\nClick on arrows flying from the enemy castle to break them.\n\nIf your champion dies in battle, you can no longer recruit regular units of the same type."));
        gameManual.Add(new HelpTextComponent("Various", "The bartender serves mead that costs 2 ducats. Each drink will increase the damage you deal in the next battle, but also reduce your health."));
        manualLength = gameManual.Count;
    }

    void Next()
    {
        currentManualID++;
        Debug.Log("next pressed manual id " + currentManualID);
        UpdateHelpText();
        CheckNext();
        CheckPrev();
    }

    void CheckNext()
    {
        if (currentManualID > manualLength - 2)
        {
            nextButton.interactable = false;
        }
        else
            nextButton.interactable = true;
    }

    void CheckPrev()
    {
        if (currentManualID > 0)
        {
            prevButton.interactable = true;
        }
        else
            prevButton.interactable = false;
    }

    void Prev()
    {
        currentManualID--;
        UpdateHelpText();
        CheckNext();
        CheckPrev();
    }
}

public class HelpTextComponent
{
    public string subtitle;
    public string textBody;

    public HelpTextComponent (string newSubtitle, string newTextBody)
    {
        subtitle = newSubtitle;
        textBody = newTextBody;
    }
}
