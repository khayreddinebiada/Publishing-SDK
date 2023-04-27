using Firebase.Analytics;
using UnityEngine;

namespace Apps.Firebase
{
    public class FirebaseEvents : IADRevenueEvent, IIAPRevenueEvent
    {
        public EventType EventType => EventType.Main;

        public void LogEvent(string eventName, Parameter[] parameters)
        {
            FirebaseAnalytics.LogEvent(eventName, parameters);
        }

        public void IAPEvent(InAppInfo info)
        {
            try
            {
                LogEvent(FirebaseAnalytics.EventPurchase,
                    new Parameter[]
                    {
                        new Parameter("InAppID", info.InAppID),
                        new Parameter("Currency", info.Currency),
                        new Parameter("Price", info.Price),
                        new Parameter("InAppType", info.InAppType),
                        new Parameter("Quantity",info.Quantity),
                    });
            }
            catch (System.Exception e)
            {
                Debug.LogError($"error_events: {e.Message}");
            }
        }

        public void ADRevenueEvent(object obj)
        {
#if Support_IronSource
            try
            {
                IronSourceImpressionData data = (IronSourceImpressionData)obj;
                LogEvent(FirebaseAnalytics.EventPurchase,
                    new Parameter[]
                    {
                        new Parameter("adUnitIdentifier", data.adUnit),
                        new Parameter("currency", "USD"),
                        new Parameter("networkName", data.adNetwork),
                        new Parameter("value", (double)data.revenue),
                        new Parameter("location", data.country),
                    });
            }
            catch (System.Exception e)
            {
                Debug.LogError($"error_events: {e.Message}");
            }
#endif
        }
    }
}