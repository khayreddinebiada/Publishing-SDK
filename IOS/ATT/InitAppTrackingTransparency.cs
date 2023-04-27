#if UNITY_IOS
using Balaso;
using System;
using static Balaso.AppTrackingTransparency;

namespace apps.ios
{
    public static class InitAppTrackingTransparency
    {
        public delegate void OnATTPassedDelegate(AuthorizationStatus status);

        public static AuthorizationStatus _status;
        public static AuthorizationStatus Status => _status;


        private static event OnATTPassedDelegate _onATTPassed;
        public static event OnATTPassedDelegate OnATTPassed
        {
            add
            {
                _onATTPassed += value;
            }
            remove
            {
                _onATTPassed -= value;
            }
        }

        public static void Intiliaze(OnATTPassedDelegate onATTPassed)
        {
            _onATTPassed += onATTPassed;

            Version currentVersion = Version.Parse(UnityEngine.iOS.Device.systemVersion);
            if (currentVersion.CompareTo(Version.Parse("14.5")) < 0)
            {
                InvokeATTPassed(AuthorizationStatus.AUTHORIZED);
                UnityEngine.Debug.Log($"Less Then 14.5: {AuthorizationStatus.AUTHORIZED}");
                return;
            }

            AuthorizationStatus status = TrackingAuthorizationStatus;
            if (_status != AuthorizationStatus.AUTHORIZED)
            {
                AppTrackingTransparency.OnAuthorizationRequestDone += OnAuthorizationRequestDone;
                RequestTrackingAuthorization();
            }
            else
            {
                InvokeATTPassed(status);
            }

            UnityEngine.Debug.Log($"AuthorizationStatus: {status}");
        }

        private static void InvokeATTPassed(AuthorizationStatus status)
        {
            _status = status;
            _onATTPassed?.Invoke(_status);
        }

        private static void OnAuthorizationRequestDone(AuthorizationStatus status)
        {
            AppTrackingTransparency.OnAuthorizationRequestDone -= OnAuthorizationRequestDone;

            UnityEngine.Debug.Log($"OnAuthorizationRequestDone: {status}");

            _status = status;
            InvokeATTPassed(status);
        }
    }
}
#endif