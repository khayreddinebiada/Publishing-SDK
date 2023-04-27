using Apps.Exception;
using System;
using UnityEngine;

namespace Apps.Ads
{
    public class IronSourceADS : IADS
    {
        private readonly IronSourceInfo _info;

        private BannerBackgroundBehaviour _backgroundBanner;

        private bool _useBanner;
        public bool UseBanner
        {
            get { return _useBanner; }
            set { _useBanner = value; }
        }

        private bool _useInterstitial;
        public bool UseInterstitial
        {
            get { return _useInterstitial; }
            set
            {
                if (value == true)
                    LoadInterstitial();

                _useInterstitial = value;
            }
        }

        private bool _useRewardedVideo;
        public bool UseRewardedVideo
        {
            get { return _useRewardedVideo; }
            set { _useRewardedVideo = value; }
        }

        private Action _onClosedInterstitial = null;
        private Action _onFailedInterstitial = null;

        private Action _onCompletedRewardedVideo = null;
        private Action _onClosedRewardedVideo = null;
        private Action _onFailedRewardedVideo = null;


        private bool _useWhiteBackgroundBanner;
        public bool UseWhiteBackgroundBanner
        {
            get { return _useWhiteBackgroundBanner; }
            set
            {
                _useWhiteBackgroundBanner = value;
                _backgroundBanner.enabled = _useBanner && _useWhiteBackgroundBanner;
            }
        }

        public string AdPlacement { get; private set; } = "";

        public IronSourceADS(
            IronSourceInfo info,
            bool useBanner,
            bool useInterstitial,
            bool useRewardedVideo,
            bool useWhiteBackgroundBanner,
            GameObject appGameObject)
        {
            _info = info ?? throw new NullReferenceException("IronSourceInfo has a null value!...");

#if UNITY_EDITOR
            string key = "Unsupported platfrom";
#elif UNITY_ANDROID
            string key = _info.AndroidKey;
#elif UNITY_IOS
            string key = _info.IOSKey;
#endif
            if (key == null || key.CompareTo("") == 0)
                throw new ArgumentEmptryOrNullException();

            _backgroundBanner = appGameObject.AddComponent<BannerBackgroundBehaviour>();

            UseBanner = useBanner;
            UseInterstitial = useInterstitial;
            UseRewardedVideo = useRewardedVideo;
            UseWhiteBackgroundBanner = UseBanner && useWhiteBackgroundBanner;



            Intialize(key);
        }

        private void Intialize(string key)
        {
            IronSource.Agent.validateIntegration();
            IronSource.Agent.init(key);

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            IronSourceEvents.onImpressionDataReadyEvent += ImpressionDataAdReadyEventIronSourceAnalytics;
            IronSourceEvents.onBannerAdLoadedEvent += OnBannerAdLoadedEvent;

            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += OnRewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
        }


        #region call back
        private void ImpressionDataAdReadyEventIronSourceAnalytics(IronSourceImpressionData impressionData)
        {
            EventsLogger.ADRevenueEvent(impressionData);
        }

        private void OnBannerAdLoadedEvent()
        {
            DisplayBanner();
        }

        private void InterstitialAdOpenedEvent()
        {
            EventsLogger.AdEvent(EventADSName.video_ads_started, AdType.interstitial, AdPlacement, EventADSResult.start);
        }

        private void InterstitialAdShowFailedEvent(IronSourceError error)
        {
            LoadInterstitial();
            EventsLogger.AdEvent(EventADSName.video_ads_started, AdType.interstitial, AdPlacement, EventADSResult.fail);
            _onFailedInterstitial?.Invoke();
        }

        private void InterstitialAdClickedEvent()
        {
            EventsLogger.AdEvent(EventADSName.video_ads_watch, AdType.interstitial, AdPlacement, EventADSResult.clicked);
        }

        private void InterstitialAdClosedEvent()
        {
            LoadInterstitial();
            EventsLogger.AdEvent(EventADSName.video_ads_watch, AdType.interstitial, AdPlacement, EventADSResult.watched);
            _onClosedInterstitial?.Invoke();
        }


        private void RewardedVideoAdOpenedEvent()
        {
            EventsLogger.AdEvent(EventADSName.video_ads_started, AdType.rewarded, AdPlacement, EventADSResult.start);
        }

        private void RewardedVideoAdShowFailedEvent(IronSourceError obj)
        {
            LoadRewardedVideo();
            EventsLogger.AdEvent(EventADSName.video_ads_started, AdType.rewarded, AdPlacement, EventADSResult.fail);
            _onFailedRewardedVideo?.Invoke();
        }

        private void RewardedVideoAdClosedEvent()
        {
            LoadRewardedVideo();
            _onClosedRewardedVideo?.Invoke();
        }

        private void OnRewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            EventsLogger.AdEvent(EventADSName.video_ads_watch, AdType.rewarded, AdPlacement, EventADSResult.watched);
            _onCompletedRewardedVideo?.Invoke();
        }

        private void RewardedVideoAdClickedEvent(IronSourcePlacement obj)
        {
            EventsLogger.AdEvent(EventADSName.video_ads_watch, AdType.rewarded, AdPlacement, EventADSResult.clicked);
        }
        #endregion

        public void LoadBanner()
        {
            if (!UseBanner)
                return;

            IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, _info.BannerPosition);
        }


        public void DisplayBanner()
        {
            if (!UseBanner) return;

            IronSource.Agent.displayBanner();
        }

        public void HideBanner()
        {
            IronSource.Agent.hideBanner();
        }

        public void DestroyBanner()
        {
            IronSource.Agent.destroyBanner();
        }

        public void LoadInterstitial()
        {
            if (!UseInterstitial || IronSource.Agent.isInterstitialReady())
                return;

            IronSource.Agent.loadInterstitial();
        }

        public bool IsInterstitialAvailable()
        {
            if (UseInterstitial && !IronSource.Agent.isInterstitialReady())
                LoadInterstitial();

            return UseInterstitial && IronSource.Agent.isInterstitialReady();
        }

        public bool ShowInterstitial(string placementName = null, Action onClose = null, Action onFailed = null)
        {
            if (!IsInterstitialAvailable())
                return false;

            IronSource.Agent.showInterstitial();

            AdPlacement = placementName;

            _onClosedInterstitial = onClose;
            _onFailedInterstitial = onFailed;

            return true;
        }

        public void LoadRewardedVideo()
        {
            if (!UseRewardedVideo || IronSource.Agent.isRewardedVideoAvailable()) return;
            
            IronSource.Agent.loadRewardedVideo();
        }

        public bool IsRewardedVideoAvailable() =>
            UseRewardedVideo && IronSource.Agent.isRewardedVideoAvailable();

        public bool ShowRewardedVideo(string placementName = null, Action onCompleted = null, Action onClose = null, Action onFailed = null)
        {
            if (!IsRewardedVideoAvailable())
                return false;

            IronSource.Agent.showRewardedVideo();

            AdPlacement = placementName;

            _onCompletedRewardedVideo = onCompleted;
            _onClosedRewardedVideo = onClose;
            _onFailedRewardedVideo = onFailed;

            return true;
        }
    }
}