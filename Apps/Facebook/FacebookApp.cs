using Facebook.Unity;

namespace Apps.Analytics
{
    public class FacebookApp
    {
        private static bool _isInited = false;

        private static int _isTrackingEnable = -1;
        public static bool IsTrackingEnable
        {
            get => _isTrackingEnable == 1;
            set
            {
                if (_isInited)
                {
#if UNITY_IOS
                    _isTrackingEnable = (value) ? 1 : 0;
                    FB.Mobile.SetAdvertiserIDCollectionEnabled(value);
                    FB.Mobile.SetAdvertiserTrackingEnabled(value);
#else
                    FB.Mobile.SetAdvertiserIDCollectionEnabled(true);
#endif
                }
            }
        }

        public FacebookApp()
        {
            if (FB.IsInitialized == true)
            {
                CallEvents();
            }
            else
            {
                FB.Init(() =>
                {
                    CallEvents();
                });
            }
        }

        public void CallEvents()
        {
            FB.ActivateApp();
            FB.LogAppEvent(AppEventName.ActivatedApp);
#if UNITY_IOS
            if (_isTrackingEnable != -1)
            {
                FB.Mobile.SetAdvertiserIDCollectionEnabled(IsTrackingEnable);
                FB.Mobile.SetAdvertiserTrackingEnabled(IsTrackingEnable);
            }
#else
            FB.Mobile.SetAdvertiserIDCollectionEnabled(true);
#endif
            _isInited = true;
        }
    }
}