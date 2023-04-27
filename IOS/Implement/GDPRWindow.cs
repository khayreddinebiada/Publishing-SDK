#if UNITY_IOS
using System;
using UnityEngine;

namespace apps.ios
{
    public sealed class GDPRWindow : IGDPRWindow
    {
        private static Action _onAccepted = null;
        private static string _terms = null;
        private static string _privacy = null;

        private bool _isInited = false;

        public GDPRWindow(string terms, string privacy)
        {
            _terms = terms;
            _privacy = privacy;
        }

        public void Show(Action onAccepted)
        {
            if (_isInited == true)
                throw new Exception("The window already called you can't call it twise!...");

            _onAccepted = onAccepted;

            CreateAlertView();
            _isInited = true;
        }

        private static void CreateAlertView()
        {
            KTAlertView.sharedInstance.AlertViewReturned += AlertViewDelegate;
            KTAlertView.sharedInstance.ShowAlertViewCS(
                "Notice",
                "By clicking \" accept\" I confirm that I have read and accept Terms and Conditions and Privacy policy",
                "Accept",
                new string[] { "Privacy", "Terms and Conditions" },
                KTAlertView.GDPRTag
                );
        }

        private static void AlertViewDelegate(int tag, int clickedIndex)
        {
            if (tag != KTAlertView.GDPRTag)
                return;

            switch (clickedIndex)
            {
                case 0:
                    _onAccepted?.Invoke();
                    break;
                case 1:
                    Application.OpenURL(_privacy);
                    CreateAlertView();
                    break;
                case 2:
                    Application.OpenURL(_terms);
                    CreateAlertView();
                    break;
            }

            KTAlertView.sharedInstance.AlertViewReturned -= AlertViewDelegate;
        }
    }
}
#endif