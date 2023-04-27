#if UNITY_IOS
using apps.ios;
#endif

namespace Apps
{
    public class TenjinServices : IApplicationPause, IIAPRevenueEvent
    {
        public EventType EventType { get; } = EventType.Main;

        private static BaseTenjin _instance;
        private static string _key;

        public static void Initiailize(string key)
        {
            _key = key;

            _instance = Tenjin.getInstance(_key);

#if UNITY_ANDROID
            AndroidConnect();
#endif
        }

        public static void AndroidConnect()
        {
            _instance.SetAppStoreType(AppStoreType.googleplay);
            MakeConnet();
        }

        public static void IOSConnect()
        {
            MakeConnet();
        }

        private static void MakeConnet()
        {
            _instance.Connect();
        }

        void IApplicationPause.OnApplicationPause(bool pauseStatus)
        {
#if UNITY_IOS
            if (InitAppTrackingTransparency.Status == Balaso.AppTrackingTransparency.AuthorizationStatus.AUTHORIZED)
            {
                if (!pauseStatus)
                {
                    MakeConnet();
                }
            }
#elif UNITY_ANDROID
            if (!pauseStatus)
            {
                MakeConnet();
            }
#endif
        }

        public void IAPEvent(InAppInfo info)
        {
#if UNITY_ANDROID
            _instance.Transaction(info.InAppID, info.Currency, 1, info.Price, null, info.Receipt, info.SignatureAndTransactionID);
#elif UNITY_IOS
            _instance.Transaction(info.InAppID, info.Currency, 1, info.Price, info.SignatureAndTransactionID, info.Receipt, null);
#endif
        }
    }
}