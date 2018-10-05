using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Deletes game progress - used for expos and playtesting on one device.
/// </summary>
public class ResetProgress : MonoBehaviour
{
	public void Reset()
    {
        GameData.current = new GameData();
    }


}
