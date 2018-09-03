using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

/// <summary>
/// Manages ads and gives out rewards to player for watching an ad.
/// Attached to MarketControl gameobject.
/// 
/// References:
/// - https://www.youtube.com/watch?v=7LFF4S9IYKM
/// 
/// 01.09.2018. Sīmanis Mikoss
/// </summary>
public class ShowAd : MonoBehaviour
{
    int ducatsPerAd = 3;

    [SerializeField]
    Text adNPCText;
    [SerializeField]
    Market market;

    private void Start()
    {
        adNPCText.text = Strings.ad_default;
        //market.gameObject.GetComponent<Market>(); this didn't work
    }

    public void PlayAd()
    {
        Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult});
    }

    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                HandleAdFinished();
                break;
            case ShowResult.Skipped:
                HandleAdSkip();
                break;
            case ShowResult.Failed:
                HandleAdFail();
                break;
        }
    }

    private void HandleAdSkip()
    {
        adNPCText.text = Strings.ad_skipped;
    }

    private void HandleAdFail()
    {
        adNPCText.text = Strings.ad_failed;
    }

    private void HandleAdFinished()
    {
        SwitchDefaultAdText();
        PlayerProfile.Singleton.DucatCurrent += ducatsPerAd;
        market.CheckIfEnoughResources(Resources.Salt, Resources.Ducats, market.saltPriceInDucats);
    }

    private void SwitchDefaultAdText()
    {
        if (adNPCText.text == Strings.ad_default)
            adNPCText.text = Strings.ad_watch_another;
        else
            adNPCText.text = Strings.ad_default;
    }
}
