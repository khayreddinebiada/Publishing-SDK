#if UNITY_IOS
using apps.analytics;
using System;
using static Balaso.AppTrackingTransparency;

namespace apps.ios
{
    public sealed class IOSInitialize
    {
        private static bool _isInitialized;
        public static bool IsInitialized => _isInitialized;

        private static event Action<AuthorizationStatus> _onCompleted;
        public static event Action<AuthorizationStatus> OnCompleted
        {
            add
            {
                _onCompleted += value;
            }
            remove
            {
                _onCompleted -= value;
            }
        }

        public IOSInitialize(string terms, string privacy, Action<AuthorizationStatus> onCompleted = null)
        {
            OnCompleted += onCompleted;

            IGDPRWindow window = new GDPRWindow(terms, privacy);

            InitGDPR.Initialize(window, OnGDPRPassed);
        }

        private void OnGDPRPassed(GDPRStatus status)
        {
            if (status == GDPRStatus.Applies)
            {
                InitAppTrackingTransparency.Intiliaze(OnATTPassed);
            }
        }

        private void OnATTPassed(AuthorizationStatus status)
        {
            UnityEngine.Debug.Log($"OnATTPassed: {status}");

            _onCompleted?.Invoke(status);
            _isInitialized = true;
        }
    }
}
#endif