namespace Apps
{
    [System.Serializable]
    public class ADSInfo
    {
#if Support_IronSource
        public Ads.IronSourceInfo IronSourceInfo;
#endif

        [UnityEngine.Header("Banner")]
        public bool UseBanner = true;
        public bool UseWhiteBackground = true;


        [UnityEngine.Header("interstitial")]
        public bool UseInterstitial = true;
        public float ShowInterstitialEvery = 30;

        [UnityEngine.Header("rewardedVideo")]
        public bool UseRewardedVideo = true;
    }
}