using Apps.Analytics;
using Apps.KPIs;
using System;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_IOS
using static Balaso.AppTrackingTransparency;
#endif

namespace Apps
{
    public static class AppsIntegration
    {
        public const string AppSettingsName = "AppsSettings";

        private readonly static List<object> _applications = new List<object>();
        private readonly static List<IApplicationPause> _applicationsPause = new List<IApplicationPause>();

        private static bool _isInited;
        private static GameObject _objectHelper;
        private static IntegrationHelper _integrationHelper;

        public static bool IsInited => _isInited;

        [RuntimeInitializeOnLoadMethod]
        public static void AutoInitialize()
        {
            AppsSettings settings = Resources.Load<AppsSettings>(AppSettingsName);
            if (settings.AutoInitialize) Initialize(settings);
        }

        public static void Initialize()
        {
            Initialize(Resources.Load<AppsSettings>(AppSettingsName));
        }

        public static void Initialize(AppsSettings settings)
        {
            if (_isInited == true) return;

            InstintiateObjectHelper();

#if UNITY_IOS
            ios.IOSInitialize iOS = new ios.IOSInitialize(settings.Terms, settings.Privacy, (status) =>
            {
                MakeInitialize(settings);

                if (status == AuthorizationStatus.AUTHORIZED)
                {
                    FacebookApp.isTrackingEnable = true;
                    TenjinManager.IOSConnect();
                }

                InitializeADSManager(settings);
                OnInitializeCompleted();
            });
#elif UNITY_ANDROID
            InitializeADSManager(settings);
            MakeInitialize(settings);
            OnInitializeCompleted();
#endif
        }

        private static void MakeInitialize(AppsSettings settings)
        {
            if (settings == null) throw new System.NullReferenceException("AppsSettings not found in resource folder!");

            // Integrate Facebook
            // .
            TryCatch(() =>
            {
                if (settings.IntegrateFacebook)
                {
                    FacebookApp facebook = new FacebookApp();
                    _applications.Add(facebook);
                }
            });

            // Integrate Tenjin
            // .
            TryCatch(() =>
            {
                if (settings.IntegrateTenjin)
                {
                    TenjinServices tenjin = new TenjinServices();

                    TenjinServices.Initiailize(settings.TenjinApiKey);
                    EventsLogger.PushEvent(tenjin);
                    _applicationsPause.Add(tenjin);
                }
            });

            // Integrate GameAnalytics
            // .
            TryCatch(() =>
            {
                if (settings.IntegrateGameAnalytics)
                {
                    GameAnalyticsEvents GA = new GameAnalyticsEvents();

                    _objectHelper.AddComponent(typeof(GameAnalyticsSDK.Events.GA_SpecialEvents));
                    _objectHelper.AddComponent(typeof(GameAnalyticsSDK.GameAnalytics));

                    EventsLogger.PushEvent(GA);
                    _applications.Add(GA);
                }
            });

            // Integrate AppMetrica
            // .
            TryCatch(() =>
            {
                if (settings.IntegrateAppMetrica)
                {
                    AppMetricaEvents appMetrica = new AppMetricaEvents(settings.AppMetricaInfo);

                    EventsLogger.PushEvent(appMetrica);

                    _applicationsPause.Add(appMetrica);
                    _applications.Add(appMetrica);
                }
            });

            InitializeKPIs();
        }

        private static void TryCatch(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(Action));
            try
            {
                action.Invoke();
            }
            catch (System.Exception e)
            {
                Debug.Log($"{nameof(e)}, {e.Message}");
            }
        }

        private static void OnInitializeCompleted()
        {
            Debug.Log("The apps is initialized...");
            _isInited = true;
        }

        private static void InitializeKPIs()
        {
            PlayTimeInfo.Initialize();
            RetentionInfo.Initialize();
            RetentionInfo.SendLoginEvent();
        }

        private static void InitializeADSManager(AppsSettings settings)
        {
            try
            {
                if (settings.IntegrateADS)
                {
                    IADS adsMaker = null;

                    ADSInfo adsInfo = settings.AdsInfo;
                    bool useBanner = adsInfo.UseBanner;
                    bool useInterstitial = adsInfo.UseInterstitial;
                    bool useRewardedVideo = adsInfo.UseRewardedVideo;
                    bool useWhiteBackground = adsInfo.UseWhiteBackground;

#if Support_IronSource && !UNITY_EDITOR
                adsMaker = new Ads.IronSourceADS(adsInfo.IronSourceInfo, useBanner, useInterstitial, useRewardedVideo, useWhiteBackground, _objectHelper);
#elif UNITY_EDITOR
                    switch (settings.ShowADSType)
                    {
                        case ShowADSType.Simulator:
                            adsMaker = new ADSSimulator("####### Simulator ######", useBanner, useInterstitial, useRewardedVideo, useWhiteBackground, _objectHelper);
                            break;
                        case ShowADSType.Debug:
                        case ShowADSType.Non:
                            adsMaker = new ADSDebugger("####### Debuger ######", settings.ShowADSType == ShowADSType.Debug);
                            break;
                    }
#else
                adsMaker = new ADSDebugger("####### Debuger ######", settings.ShowADSType == ShowADSType.Debug);
#endif

                    ADSManager.Initialize(adsMaker, _integrationHelper, settings.AdsInfo.ShowInterstitialEvery);
                    _applications.Add(adsMaker);
                }
            }
            catch(System.Exception e)
            {
                Debug.Log($"{nameof(e)}, {e.Message}");
            }
        }

        private static void InstintiateObjectHelper()
        {
            _objectHelper = new GameObject("Integrations Helper");
            _integrationHelper = (IntegrationHelper)_objectHelper.AddComponent(typeof(IntegrationHelper));
            UnityEngine.Object.DontDestroyOnLoad(_objectHelper);
        }

        public static void OnApplicationPause(bool pause)
        {
            foreach (IApplicationPause Application in _applicationsPause)
            {
                if (Application != null && !Application.Equals(null)) Application.OnApplicationPause(pause);
            }
        }
    }
}
