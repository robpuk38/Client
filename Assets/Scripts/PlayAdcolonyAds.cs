﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAdcolonyAds : MonoBehaviour {

    public Text DebugText;
    public GameObject AdcolonyBtn;
    public GameObject Disable;
   // public string AppId = "app45c8b1591bd14f93a2";
   // public string ZoneId = "vz83776cea456c492b95";

    bool CompleteAd = false;
   
    AdColony.InterstitialAd Ad = null;
  


    private void Start()
    {
        ConfigureAds();
    }
    private void ConfigureAds()
    {
        DebugText.text = "Ads Being Configured";
        AdcolonyBtn.GetComponent<Button>().enabled = false;
        AdColony.AppOptions appOptions = new AdColony.AppOptions();
        AdColony.Ads.Configure(SystemConfig.Instance.AdcolonyAppId, appOptions, SystemConfig.Instance.AdcolonyZoneId);

        RequestAd();
        
    }

    private void RequestAd()
    {
        DebugText.text = "Ads Being Request";
        AdColony.AdOptions adOptions = new AdColony.AdOptions();
        adOptions.ShowPostPopup = false;
        adOptions.ShowPrePopup = false;

        AdColony.Ads.RequestInterstitialAd(SystemConfig.Instance.AdcolonyZoneId, adOptions);
        intEvenetHandler();
    }

    

    private void intEvenetHandler()
    {

        AdColony.Ads.OnConfigurationCompleted += (List<AdColony.Zone> zone_) =>
        {
            DebugText.text = "Ads Request Has Been Complete "+ zone_.ToString();
        };

        AdColony.Ads.OnRequestInterstitial += (AdColony.InterstitialAd ad_) => {
            DebugText.text = "Ads Request Has Requested Interstiler Ad "+ ad_.ToString();
           
            Ad = ad_;
            Disable.SetActive(false);
            AdcolonyBtn.GetComponent<Button>().enabled = true;
        };

        AdColony.Ads.OnRequestInterstitialFailed += () =>
        {
            DebugText.text = "Ads Request Has Failed";
            AdcolonyBtn.GetComponent<Button>().enabled = false;
            Disable.SetActive(true);

        };

        AdColony.Ads.OnOpened += (AdColony.InterstitialAd ad_) => {
            DebugText.text = "Ad Has Been Open "+ ad_.ToString();
            CompleteAd = false;
        };

        AdColony.Ads.OnClosed += (AdColony.InterstitialAd ad_) => {
           // DebugText.text = "Ad Has Been Closed " + ad_.ToString();

            
        };

        AdColony.Ads.OnExpiring += (AdColony.InterstitialAd ad_) => {
            DebugText.text = "Ad Is Expiring " + ad_.ToString();
            AdcolonyBtn.GetComponent<Button>().enabled = false;
            Disable.SetActive(true);
        };

        AdColony.Ads.OnIAPOpportunity += (AdColony.InterstitialAd ad_, string iapProductId_, AdColony.AdsIAPEngagementType engagement_) =>
        {
            DebugText.text = "AdColony.Ads.OnIAPOpportunity called";
        };

        AdColony.Ads.OnRewardGranted += (string zoneId, bool success, string name, int amount) =>
        {
            DebugText.text = string.Format("AdColony.Ads.OnRewardGranted ", zoneId, success, name, amount);

            if (CompleteAd == false)
            {
                CompleteAd = true;
                int newcredits = 0;
                int.TryParse(DataManager.Instance.GetUserCredits(), out newcredits);
                int pushcredits = newcredits + 100;
                DataManager.Instance.SetUserCredits(pushcredits.ToString());

                ServerStatusManager.Instance.SendNewDataType(Construct.ONADS);
            }



        };

        AdColony.Ads.OnCustomMessageReceived += (string type, string message) =>
        {
            DebugText.text = string.Format("AdColony.Ads.OnCustomMessageReceived ", type, message);
        };

    }

    public void PlayAd()
    {
        RequestAd();
        if (Ad != null)
        {
            //if your button was disabled or enabled to allow button click
            DebugText.text = "Ad Is Playable";
            AdColony.Ads.ShowAd(Ad);
        }
        
           
            
        
    }
}
