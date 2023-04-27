using Apps.Exception;
using UnityEngine;
using Engine.Data;
using Apps.Ads;
using System;

namespace Apps
{
    public static class ADSManager
    {
        private const string NoAdsKey = "NoAds";
        private readonly static FieldKey<int> _isNoAds = new FieldKey<int>(NoAdsKey, DataSaveInfo.FileName, 0);


        public static bool IsInited { get; private set; }
        public static bool IsNoAds => _isNoAds.value != 0;

        private static float _lastTimerAds = 0;
        private static float _showInterstitialEvery = 45;

        private static AutoAdsReloader _autoAdsReloader;
        private static IADS _ADSMaker;


        /// <summary>
        /// To enable and disable the Interstitial ads. 
        /// </summary>
        public static bool EnableInterstitial
        {
            get { return _ADSMaker.UseInterstitial; }
            set { _ADSMaker.UseInterstitial = value; }
        }

        /// <summary>
        /// To enable and disable the Banner ads. 
        /// </summary>
        public static bool EnableBanner
        {
            get { return _ADSMaker.UseBanner; }
            set { _ADSMaker.UseBanner = value; }
        }

        /// <summary>
        /// To enable and disable the RewardedVideo ads. 
        /// </summary>
        public static bool EnableRewardedVideo
        {
            get { return _ADSMaker.UseRewardedVideo; }
            set { _ADSMaker.UseRewardedVideo = value; }
        }

        /// <summary>
        /// Intialize the ADSManager.
        /// </summary>
        /// <param name="adsMaker"> The IADS that will execute ADS. </param>
        /// <param name="showInterstitialEvery"> We will show ADS every amount of time in case of auto interstitial.
        /// And we will block the ADS that will called and has less then this time amount. </param>
        public static void Initialize(IADS adsMaker, MonoBehaviour monoHelper, float showInterstitialEvery = 0)
        {
            if (IsInited)
                throw new ReinitializedException();

            if (adsMaker == null)
                throw new ArgumentNullException();

            _ADSMaker = adsMaker;
            _lastTimerAds = Time.time;
            _showInterstitialEvery = showInterstitialEvery;

            StopBannerAndInterstitials();
            LoadAds();

            _autoAdsReloader = new AutoAdsReloader(20);
            monoHelper.StartCoroutine(_autoAdsReloader.StartLoad());

            IsInited = true;
        }

        private static void DeactivateBannerAndInterstitials()
        {
            _ADSMaker.UseBanner = false;
            _ADSMaker.UseInterstitial = false;
        }

        private static void LoadAds()
        {
            LoadBanner();
            LoadInterstitial();
            LoadRewardedVideo();
        }

        private static void StopBannerAndInterstitials()
        {
            if (IsNoAds)
            {
                DeactivateBannerAndInterstitials();
            }
        }

        public static void RemoveAds()
        {
            _isNoAds.value = 1;
            DeactivateBannerAndInterstitials();
            _ADSMaker.HideBanner();
            _ADSMaker.DestroyBanner();
        }

        public static void LoadBanner()
        {
            if (IsInited)
                _ADSMaker.LoadBanner();
        }

        public static bool AllowShowBanner()
        {
            return IsInited && _ADSMaker.UseBanner;
        }

        public static bool DisplayBanner()
        {
            if (!AllowShowBanner())
                return false;

            _ADSMaker.DisplayBanner();
            return true;
        }

        public static void HideBanner()
        {
            if (IsInited) _ADSMaker.HideBanner();
        }

        public static void LoadInterstitial()
        {
            if (IsInited) _ADSMaker.LoadInterstitial();
        }

        public static bool AllowShowInterstitial()
        {
            if (!IsInited || !_ADSMaker.UseInterstitial) return false;

            if ( !_ADSMaker.IsInterstitialAvailable())
                _ADSMaker.LoadInterstitial();

            return _ADSMaker.IsInterstitialAvailable();
        }

        public static bool ShowInterstitial(string placementName, Action onClosed = null, Action onFailed = null)
        {
            if (placementName == null || placementName.CompareTo("") == 0)
                throw new ArgumentEmptryOrNullException();

            bool isAvailable = _ADSMaker.IsInterstitialAvailable();

            EventsLogger.AdEvent(
                EventADSName.video_ads_available,
                AdType.interstitial,
                placementName,
                (isAvailable) ? EventADSResult.start : EventADSResult.not_available);

            if (!AllowShowInterstitial())
                return false;

            _ADSMaker.ShowInterstitial(placementName, onClosed, onFailed);
            return true;
        }

        private static bool IsTimeToShowInterstitial(float timerToShow) =>
            _lastTimerAds + timerToShow <= Time.time;

        private static bool MakeAutoInterstitial(string placementName, float timerToShow, Action onClosed = null, Action onFailed = null)
        {
            if (placementName == null || placementName.CompareTo("") == 0)
                throw new ArgumentEmptryOrNullException();

            if (IsTimeToShowInterstitial(timerToShow))
            {
                if (ShowInterstitial(placementName, onClosed, () => OnFailedToShowInterstitital(onFailed, timerToShow)))
                {
                    EventsLogger.CustomEvent($"ADS:TimeBetween:{(int)(Time.time - _lastTimerAds)}s");
                    _lastTimerAds = Time.time;
                    return true;
                }
            }

            return false;
        }

        public static bool AutoShowInterstitial(string placementName, Action onClosed = null, Action onFailed = null)
        {
            return MakeAutoInterstitial(placementName, _showInterstitialEvery, onClosed, onFailed);
        }

        public static bool AutoShowInterstitial(string placementName, float timerToShow, Action onClosed = null, Action onFailed = null)
        {
            return MakeAutoInterstitial(placementName, timerToShow, onClosed, onFailed);
        }

        private static void OnFailedToShowInterstitital(Action onFailed, float timerToShow)
        {
            _lastTimerAds = Time.time - timerToShow;
            onFailed?.Invoke();
        }

        public static void LoadRewardedVideo()
        {
            if (IsInited)
                _ADSMaker.LoadRewardedVideo();
        }

        /// <summary>
        /// To check it we can use RewardedVideo.
        /// </summary>
        public static bool AllowShowRewardedVideo()
        {
            if (!IsInited || !_ADSMaker.UseRewardedVideo) return false;

            if (!_ADSMaker.IsRewardedVideoAvailable())
                _ADSMaker.LoadRewardedVideo();

            return _ADSMaker.IsRewardedVideoAvailable();
        }

        /// <summary>
        /// To show Interstitial with placement name.
        /// </summary>
        /// <param name="placementName"> The placement name what we will show to understand the current ad. </param>
        /// <param name="onRewarded"> The event executed on use close the ad. </param>
        /// <returns> Return true if the ad is showed </returns>
        public static bool ShowRewardedVideo(string placementName, Action onRewarded, Action onClosed = null, Action onFailed = null)
        {
            if (placementName == null || placementName.CompareTo("") == 0)
                throw new ArgumentEmptryOrNullException();

            if (!_ADSMaker.IsRewardedVideoAvailable())
                return false;

            _ADSMaker.ShowRewardedVideo(placementName, onRewarded, onClosed, onFailed);
            return true;
        }
    }
}
