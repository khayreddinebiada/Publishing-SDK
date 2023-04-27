using System;
using UnityEngine;

namespace Apps
{

    public class ADSDebugger : IADS
    {
        public string AdPlacement { get; private set; }
        public bool IsDebuging { get; set; }

        public bool UseBanner { get; set; }
        public bool UseInterstitial { get; set; }
        public bool UseRewardedVideo { get; set; }

        public ADSDebugger(string key, bool isDebuging)
        {
            IsDebuging = isDebuging;
            if (isDebuging) Debug.Log("The ADS is intialized with key: " + key);
        }

        public void LoadBanner()
        {
            if (UseBanner && IsDebuging) Debug.Log("The Banner ad is Loaded.");
        }

        public void DisplayBanner()
        {
            if (UseBanner && IsDebuging) Debug.Log("The Banner ad is Displayed.");
        }

        public void CreateBanner()
        {
            if (UseBanner && IsDebuging) Debug.Log("The Banner ad is Created.");
        }

        public void DestroyBanner()
        {
            if (IsDebuging) Debug.Log("The Banner ad is Destroyed.");
        }

        public void HideBanner()
        {
            if (IsDebuging) Debug.Log("The Banner ads is Hided.");
        }

        public void LoadInterstitial()
        {
            if (UseInterstitial && IsDebuging) Debug.Log("The Interstitial ad is Loaded.");
        }

        public bool IsInterstitialAvailable()
        {
            return UseInterstitial;
        }

        public bool ShowInterstitial(string placementName = null, Action onClose = null, Action onFailed = null)
        {
            AdPlacement = placementName;

            if (UseInterstitial && IsDebuging) Debug.Log("Show Interstitial ad.");
            return true;
        }

        public void LoadRewardedVideo()
        {
            if (UseRewardedVideo && IsDebuging) Debug.Log("The RewardsVideo ad is Loaded.");
        }

        public bool IsRewardedVideoAvailable()
        {
            return UseRewardedVideo;
        }

        public bool ShowRewardedVideo(string placementName = null, Action onCompleted = null, Action onClose = null, Action onFailed = null)
        {
            AdPlacement = placementName;

            if (UseRewardedVideo && IsDebuging)
            {
                Debug.Log("Show RewardsVideo ad.");
                Debug.Log("RewardsVideo ad is completed.");
            }
            return true;
        }
    }
}
