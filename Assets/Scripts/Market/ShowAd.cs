using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// References:
/// - https://www.youtube.com/watch?v=7LFF4S9IYKM
/// 
/// 01.09.2018. Sīmanis Mikoss
/// </summary>
public class ShowAd : MonoBehaviour
{
    int ducatsPerAd = 3;

    public void PlayAd()
    {
        Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult});
    }

    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("ad finished");
                PlayerProfile.Singleton.DucatCurrent += ducatsPerAd;
                break;
            case ShowResult.Skipped:
                Debug.Log("ad skipped");
                break;
            case ShowResult.Failed:
                Debug.Log("ad failed");
                break;
        }
    }
}
