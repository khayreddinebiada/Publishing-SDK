#if UNITY_IOS
using UnityEngine;

namespace apps.ios
{
    public static class InitGDPR
    {
        private const string KeyIsAccepted = "GDPRAccepted";
        public delegate void OnGDPRPassedDelegate(GDPRStatus status);

        private static IGDPRWindow _window;
        private static GDPRStatus _status;

        public static GDPRStatus Status => _status;
        private static event OnGDPRPassedDelegate _onGDPRPassed;


        public static event OnGDPRPassedDelegate OnGDPRPassed
        {
            add
            {
                _onGDPRPassed += value;
            }
            remove
            {
                _onGDPRPassed -= value;
            }
        }

        private static bool IsAccepted
        {
            get => PlayerPrefs.GetInt(KeyIsAccepted) == 1000;
            set => PlayerPrefs.SetInt(KeyIsAccepted, 1000);
        }

        public static void Initialize(IGDPRWindow window, OnGDPRPassedDelegate onGDPRPassed)
        {
            _window = window ?? throw new System.ArgumentNullException("The window has a null value!...");
            OnGDPRPassed += onGDPRPassed;

            bool isAccepted = IsAccepted;

            if (!isAccepted)
            {
                _status = GDPRStatus.DoesNotApply;
                _window.Show(OnAccepted);
            }
            else
            {
                _status = GDPRStatus.Applies;
                _onGDPRPassed?.Invoke(_status);
            }
        }

        private static void OnAccepted()
        {
            IsAccepted = true;
            _status = GDPRStatus.Applies;
            _onGDPRPassed?.Invoke(_status);
        }
    }
}
#endif