using Apps.Adsview;
using Apps.Exception;
using System;
using UnityEngine;

namespace Apps
{
    public class ADSSimulator : IADS
    {
        public readonly static float WidthScaling = Screen.width / 1080.0f;
        public readonly static float HeightScaling = Screen.height / 1920.0f;

        private BannerBackgroundBehaviour _backgroundBanner;

        public BannerBackgroundBehaviour BackgroundBanner => _backgroundBanner;

        private Action _onClosedInterstitial;
        private Action _onFailedInterstitial;

        private Action _onCompletedRewardedVideo = null;
        private Action _onClosedRewardedVideo = null;
        private Action _onFailedRewardedVideo = null;


        private bool _useBanner;
        public bool UseBanner
        {
            get { return _useBanner; }
            set
            {
                if (value == true)
                    BannerIsLoaded = true;

                _useBanner = value;
                _backgroundBanner.enabled = _useBanner && UseWhiteBackgroundBanner;
            }
        }

        private bool _useInterstitial;
        public bool UseInterstitial
        {
            get { return _useInterstitial; }
            set
            {
                if (value == true)
                    InterstitialIsLoaded = true;

                _useInterstitial = value;
            }
        }

        private bool _useRewardedVideo;
        public bool UseRewardedVideo
        {
            get { return _useRewardedVideo; }
            set
            {
                if (value == true)
                    RewardedVideoIsLoaded = true;

                _useRewardedVideo = value;
            }
        }


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
        public bool BannerIsLoaded { get; private set; } = false;
        public bool InterstitialIsLoaded { get; private set; } = false;
        public bool RewardedVideoIsLoaded { get; private set; } = false;


        private Window _bannerWindow;
        private Window _interstitialWindow;
        private Window _rewardsVideoWindow;

        public bool BannerIsDisplaying => _bannerWindow.isEnable;
        public bool InterstitialIsDisplaying => _interstitialWindow.isEnable;
        public bool RewardsVideoIsDisplaying => _rewardsVideoWindow.isEnable;

        public ADSSimulator(
            string key,
            bool useBanner,
            bool useInterstitial,
            bool useRewardedVideo,
            bool useWhiteBackgroundBanner,
            GameObject appGameObject)
        {
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
            Debug.Log("The ADS is intialized with key: " + key);

            CreateBannerWindow();
            CreateInterstitialWindow();
            CreateRewardsVideoWindow();

            LoadBanner();
            LoadInterstitial();
            LoadRewardedVideo();
        }

        #region banner
        private void CreateBannerWindow()
        {
            if (_bannerWindow != null)
                return;

            GameObject bannerObject = new GameObject("Banner");
            _bannerWindow = new Window(bannerObject);

            GUIBox bannerView = (GUIBox)bannerObject.AddComponent(typeof(GUIBox));
            bannerView.Initialize(
                new GUIContent("Banner"),
                new Rect(
                    0,
                    Screen.height - 80 * HeightScaling,
                    Screen.width,
                    80 * HeightScaling
                )
                );
        }

        public void LoadBanner()
        {
            if (!_useBanner)
                return;

            BannerIsLoaded = true;
        }

        public void DisplayBanner()
        {
            if (!BannerIsLoaded) return;

            _bannerWindow.ShowWindow();
        }

        public void HideBanner()
        {
            _bannerWindow.HideWindow();
        }

        public void CreateBanner()
        {
            CreateBannerWindow();
        }

        public void DestroyBanner()
        {
            _backgroundBanner.enabled = false;
            UnityEngine.Object.Destroy(_bannerWindow.gameObject);
        }
        #endregion

        #region interstitial
        private void CreateInterstitialWindow()
        {
            if (_interstitialWindow != null)
                return;

            GameObject interstitialObject = new GameObject("Interstitial");
            _interstitialWindow = new Window(interstitialObject);

            /// Create background Interstitial
            GUIBox backgroundView = (GUIBox)interstitialObject.AddComponent(typeof(GUIBox));
            backgroundView.Initialize(
                new GUIContent(""),
                new Rect(0, 0, Screen.width, Screen.height)
                );

            /// Create close button Interstitial
            GUIButton close = (GUIButton)interstitialObject.AddComponent(typeof(GUIButton));
            close.Initialize(
                new GUIContent("X"),
                new Rect(
                    Screen.width - 120 * WidthScaling,
                    20 * HeightScaling,
                    100 * WidthScaling,
                    100 * HeightScaling
                    ),
                OnClickClosedInterstitial
                );

            /// Create close button Interstitial
            GUIButton failed = (GUIButton)interstitialObject.AddComponent(typeof(GUIButton));
            failed.Initialize(
                new GUIContent("!"),
                new Rect(
                    Screen.width - 240 * WidthScaling,
                    20 * HeightScaling,
                    100 * WidthScaling,
                    100 * HeightScaling
                    ),
                OnClickFailedInterstitial
                );
        }

        public void LoadInterstitial()
        {
            if (!_useInterstitial)
                return;

            InterstitialIsLoaded = true;
        }

        public bool IsInterstitialAvailable()
        {
            return _useInterstitial && InterstitialIsLoaded;
        }

        public bool ShowInterstitial(string placementName = null, Action onClosed = null, Action onFailed = null)
        {
            if (!IsInterstitialAvailable())
                return false;

            _interstitialWindow.ShowWindow(placementName);
            InterstitialIsLoaded = false;

            _onClosedInterstitial = onClosed;
            _onFailedInterstitial = onFailed;
            AdPlacement = placementName;

            return true;
        }

        private void OnClickClosedInterstitial()
        {
            _interstitialWindow.HideWindow();
            _onClosedInterstitial?.Invoke();
            LoadInterstitial();
        }

        private void OnClickFailedInterstitial()
        {
            _interstitialWindow.HideWindow();
            _onFailedInterstitial?.Invoke();
            LoadInterstitial();
        }
        #endregion

        #region rewards video
        private void CreateRewardsVideoWindow()
        {
            if (_rewardsVideoWindow != null)
                return;

            GameObject rewardsVideoObject = new GameObject("RewardsVideo");
            _rewardsVideoWindow = new Window(rewardsVideoObject);

            /// Create background RewardsVideo
            GUIBox backgroundView = (GUIBox)rewardsVideoObject.AddComponent(typeof(GUIBox));
            backgroundView.Initialize(
                new GUIContent(""),
                new Rect(0, 0, Screen.width, Screen.height)
                );

            /// Create cancel button RewardsVideo
            GUIButton closed = (GUIButton)rewardsVideoObject.AddComponent(typeof(GUIButton));
            closed.Initialize(
                new GUIContent("X"),
                new Rect(
                    Screen.width - 120 * WidthScaling,
                    20 * HeightScaling,
                    100 * WidthScaling,
                    100 * HeightScaling
                    ),
                OnClickClosedRewardsVideo
                );

            /// Create close button Interstitial
            GUIButton failed = (GUIButton)rewardsVideoObject.AddComponent(typeof(GUIButton));
            failed.Initialize(
                new GUIContent("!"),
                new Rect(
                    Screen.width - 240 * WidthScaling,
                    20 * HeightScaling,
                    100 * WidthScaling,
                    100 * HeightScaling
                    ),
                OnClickFailedRewardsVideo
                );

            /// Create completed button RewardsVideo
            GUIButton completedButtonView = (GUIButton)rewardsVideoObject.AddComponent(typeof(GUIButton));
            completedButtonView.Initialize(
                new GUIContent("Completed"),
                new Rect(
                    Screen.width / 2 - 75 * WidthScaling,
                    20 * HeightScaling,
                    250 * WidthScaling,
                    100 * HeightScaling
                    ),
                OnClickCompletedRewardsVideo
                );


        }

        public void LoadRewardedVideo()
        {
            if (!_useRewardedVideo)
                return;

            RewardedVideoIsLoaded = true;
        }

        public bool IsRewardedVideoAvailable()
        {
            return _useRewardedVideo && RewardedVideoIsLoaded;
        }

        public bool ShowRewardedVideo(string placementName = null, Action onCompleted = null, Action onClosed = null, Action onFailed = null)
        {
            if (!IsRewardedVideoAvailable())
                return false;


            _rewardsVideoWindow.ShowWindow(placementName);
            RewardedVideoIsLoaded = false;

            _onCompletedRewardedVideo = onCompleted;
            _onClosedRewardedVideo = onClosed;
            _onFailedRewardedVideo = onFailed;
            AdPlacement = placementName;

            return true;
        }

        private void OnClickCompletedRewardsVideo()
        {
            _rewardsVideoWindow.HideWindow();
            _onCompletedRewardedVideo?.Invoke();
            LoadRewardedVideo();
        }

        private void OnClickClosedRewardsVideo()
        {
            _rewardsVideoWindow.HideWindow();
            _onClosedRewardedVideo?.Invoke();
            LoadRewardedVideo();
        }

        private void OnClickFailedRewardsVideo()
        {
            _rewardsVideoWindow.HideWindow();
            _onFailedRewardedVideo?.Invoke();
            LoadRewardedVideo();
        }
        #endregion
    }
}
