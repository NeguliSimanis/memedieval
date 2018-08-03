using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScreenSleep
{
    public static DisableScreenSleep current;

	public void DisableSleep ()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
