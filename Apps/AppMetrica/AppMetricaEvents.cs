using Apps.KPIs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Apps.Analytics
{
    public class AppMetricaEvents : IStandardEvent, IProgressEvent, IIAPRevenueEvent, IADRevenueEvent, IImpressionEvent, IApplicationPause
    {
        public EventType EventType { get; } = EventType.Main;

        private bool m_ActualPauseStatus;

        private readonly AppMetricaInfo m_Info;

        private IYandexAppMetrica m_Metrica;
        private IYandexAppMetrica metrica
        {
            get
            {
                if (m_Metrica == null)
                {
#if UNITY_EDITOR
                    if (m_Metrica == null)
                    {
                        m_Metrica = new YandexAppMetricaDummy();
                    }
#elif UNITY_IPHONE || UNITY_IOS
                    if (m_Metrica == null && Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        m_Metrica = new YandexAppMetricaIOS();
                    }
#elif UNITY_ANDROID
                    if (m_Metrica == null && Application.platform == RuntimePlatform.Android)
                    {
                        m_Metrica = new YandexAppMetricaAndroid();
                    }
#endif
                }

                return m_Metrica;
            }
        }

        public AppMetricaEvents(AppMetricaInfo info)
        {
            m_Info = info;

            SetupMetrica();

            RegisterLogCallback();

            metrica.ResumeSession();
        }

        #region Setup
        private void SetupMetrica()
        {
            YandexAppMetricaConfig configuration = new YandexAppMetricaConfig(m_Info.ApiKey)
            {
                SessionTimeout = (int)m_Info.SessionTimeoutSec,
                Logs = m_Info.Logs,
                HandleFirstActivationAsUpdate = m_Info.HandleFirstActivationAsUpdate,
                StatisticsSending = m_Info.StatisticsSending
            };

#if !APP_METRICA_TRACK_LOCATION_DISABLED
            configuration.LocationTracking = m_Info.LocationTracking;
            if (m_Info.LocationTracking)
            {
                Input.location.Start();
            }
#else
            configuration.LocationTracking = false;
#endif

            metrica.ActivateWithConfiguration(configuration);
        }

        private void RegisterLogCallback()
        {
            if (m_Info.ExceptionsReporting)
            {
#if UNITY_5 || UNITY_5_3_OR_NEWER
                Application.logMessageReceived += HandleLog;
#else
			    Application.RegisterLogCallback(HandleLog);
#endif
            }
        }

        private void HandleLog(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                metrica.ReportErrorFromLogCallback(condition, stackTrace);
            }
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (m_ActualPauseStatus != pauseStatus)
            {
                m_ActualPauseStatus = pauseStatus;
                if (pauseStatus)
                {
                    metrica.PauseSession();
                }
                else
                {
                    metrica.ResumeSession();
                }
            }
        }
        #endregion

        public void CustomEvent(string eventName)
        {
            if (eventName == null) throw new NullReferenceException("The string eventName has a null value!...");
            if (eventName == string.Empty) throw new ArgumentException("The string eventName has a an empty value!...");
            if (eventName[0] == ':' || eventName[eventName.Length - 1] == ':') throw new FormatException("Ivalide format the ':' should be not the first or last on the eventName!...");


            int indexOf = eventName.IndexOf(':');

            if (indexOf < 0)
                metrica.ReportEvent($"eventName");
            else
                metrica.ReportEvent(
                    eventName.Substring(0, indexOf),
                    DecomposeEventName(
                        eventName.Substring(indexOf + 1)));
        }

        public void SessionEvent(string sessionName, SessionStatue statue)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();

            Dictionary<string, object> momenet = new Dictionary<string, object>();
            momenet.Add(sessionName, PlayTimeInfo.TimeRange);

            values.Add("name", momenet);
            values.Add("state", statue.ToString());

            metrica.ReportEvent("Session", values);
        }

        public void IAPEvent(InAppInfo info)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();

            Dictionary<string, object> momenet = new Dictionary<string, object>();
            momenet.Add(info.InAppID, PlayTimeInfo.TimeRange);

            values.Add("inapp_id", momenet);
            values.Add("currency", info.Currency);
            values.Add("price", info.Price);
            values.Add("inapp_type", info.InAppType);

            metrica.ReportEvent("payment_succeed", values);
        }

        public void ProgressStartedEvent(ProgressStartInfo progressInfo)
        {
            metrica.ReportEvent("level_start", progressInfo.ToDictionary());
            metrica.SendEventsBuffer();
        }

        public void ProgressFailedEvent(ProgressFailedInfo progressInfo)
        {
            metrica.ReportEvent("level_failed", progressInfo.ToDictionary());
            metrica.SendEventsBuffer();
        }

        public void ProgressCompletedEvent(ProgressCompletedInfo progressInfo)
        {
            metrica.ReportEvent("level_finish", progressInfo.ToDictionary());
            metrica.SendEventsBuffer();
        }

        public void ErrorEvent(ErrorSeverity severity, string message)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();

            Dictionary<string, object> momenet = new Dictionary<string, object>();
            momenet.Add(severity.ToString(), PlayTimeInfo.TimeRange);

            keys.Add("error_severity", momenet);
            keys.Add("message", message.ToString());

            metrica.ReportEvent("error_events", keys);
            metrica.SendEventsBuffer();
        }

        public void AdImpressionEvent(EventADSName eventADSName, AdType adType, string placement, EventADSResult result)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();

            Dictionary<string, object> momenet = new Dictionary<string, object>();
            momenet.Add(adType.ToString(), PlayTimeInfo.TimeRange);

            keys.Add("ad_type", momenet);
            keys.Add("placement", placement);
            keys.Add("result", result);
            keys.Add("connection", (Application.internetReachability == NetworkReachability.NotReachable) ? 0 : 1);

            m_Metrica.ReportEvent(eventADSName.ToString(), keys);
            m_Metrica.SendEventsBuffer();
        }

        private static string DecomposeEventName(string eventName)
        {
            return DecomposeEventName(eventName.Split(':'));
        }

        /// <summary>
        /// To send the revenue of the ads.
        /// </summary>
        /// <param name="mediation"> The mediation name ex: Ironsource, Applovin... </param>
        /// <param name="impressionData"> The data of the impression </param>
        public void ADRevenueEvent(object impressionData)
        {
#if Support_IronSource
            try
            {
                IronSourceImpressionData data = (IronSourceImpressionData)impressionData;
                YandexAppMetricaAdRevenue values = new YandexAppMetricaAdRevenue((double)data.revenue, "USD");

                values.AdPlacementName = data.placement;
                values.AdUnitId = data.adUnit;

                metrica.ReportAdRevenue(values);
            }
            catch (System.Exception e)
            {
                Dictionary<string, object> keys = new Dictionary<string, object>();

                keys.Add("error_severity", "error");
                keys.Add("message", e.Message.ToString());

                metrica.ReportEvent("error_events", keys);
            }
#endif
        }

        private static string DecomposeEventName(string[] parts)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < parts.Length - 1; i++)
            {
                result.Append("{\"");
                result.Append($"{parts[i]}");
                result.Append("\":");
            }

            result.Append("{\"");
            result.Append($"{parts[parts.Length - 1]}");
            result.Append("\":\"null\"");

            result.Append('}', parts.Length);

            return result.ToString();
        }
    }
}