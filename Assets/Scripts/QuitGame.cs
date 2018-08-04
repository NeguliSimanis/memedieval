using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuitGame : MonoBehaviour {

	public void Quit()
    {
        Application.Quit();
    }

    public void QuitAfterSave()
    {
        SaveLoad gameSaver = new SaveLoad();
        gameSaver.Save();
        Quit();
    }
}
