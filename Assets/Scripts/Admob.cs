using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;

public class Admob : MonoBehaviour
{
    private const string BANNER_ID = "ca-app-pub-1026512834381359/2378317290";
    private BannerView _banner;
    
    private void Awake() => MobileAds.Initialize(initStatuc => { });

    private void OnEnable()
    {
        _banner = new BannerView(BANNER_ID, AdSize.Banner, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest.Builder().Build();       
        StartCoroutine(ShowBanner(adRequest));
    }

    IEnumerator ShowBanner(AdRequest adRequest)
    {       
        yield return new WaitForSeconds(3.0f);
        _banner.LoadAd(adRequest);
    }
}
