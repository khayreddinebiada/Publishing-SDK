using System;
using System.Collections.Generic;

namespace Apps
{
    public static class EventsLogger
    {
        private static Stack<IEvent> _events = new Stack<IEvent>();

        public static void PushEvent(IEvent ev)
        {
            if (ev == null) throw new ArgumentNullException("The event object has a null value!...");

            _events.Push(ev);
        }
        
        /// <summary>
        /// To send a custom event.
        /// </summary>
        /// <param name="eventName"> The event name that we will send. </param>
        /// <param name="value"> The value of event, example: The score.</param>
        public static void CustomEvent(string eventName, EventType type = EventType.Main)
        {
            ForEach<IStandardEvent>((logger) => logger.CustomEvent($"{eventName}"), type);
        }

        /// <summary>
        /// To send a session event.
        /// </summary>
        /// <param name="sessionName"> The session name that we will send. </param>
        /// <param name="statue"> The session statue that we will send it can be started or completed. </param>
        public static void SessionEvent(string sessionName, SessionStatue statue, EventType type = EventType.Main)
        {
            ForEach<IStandardEvent>((logger) => logger.SessionEvent(sessionName, statue), type);
        }

        /// <summary>
        /// To send some error events.
        /// </summary>
        /// <param name="severity"> The error type. </param>
        /// <param name="message"> The message content in the error. </param>
        public static void ErrorEvent(ErrorSeverity severity, string message, EventType type = EventType.Main)
        {
            ForEach<IStandardEvent>((logger) => logger.ErrorEvent(severity, message), type);
        }

        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        public static void ProgressStartEvent(ProgressStartInfo progressInfo, EventType type = EventType.Main)
        {
            ForEach<IProgressEvent>((logger) => logger.ProgressStartedEvent(progressInfo), type);
        }

        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        public static void ProgressFailedEvent(ProgressFailedInfo progressInfo, EventType type = EventType.Main)
        {
            ForEach<IProgressEvent>((logger) => logger.ProgressFailedEvent(progressInfo), type);
        }
        
        /// <summary>
        /// To send a ProgressEvent event.
        /// </summary>
        public static void ProgressCompletedEvent(ProgressCompletedInfo progressInfo, EventType type = EventType.Main)
        {
            ForEach<IProgressEvent>((logger) => logger.ProgressCompletedEvent(progressInfo), type);
        }

        /// <summary>
        /// To send some products buying in the game.
        /// </summary>
        /// <param name="productIAPID"> product ID of the IAP. </param>
        /// <param name="price"> The price that spended on this product. </param>
        public static void IAPEvent(InAppInfo info, EventType type = EventType.Main)
        {
            ForEach<IIAPRevenueEvent>((logger) => logger.IAPEvent(info), type);
        }

        /// <summary>
        /// To send the revenue of the ads.
        /// </summary>
        /// <param name="mediation"> The mediation name ex: Ironsource, Applovin... </param>
        /// <param name="impressionData"> The data of the impression </param>
        public static void ADRevenueEvent(object impressionData, EventType type = EventType.Main)
        {
            ForEach<IADRevenueEvent>((logger) => logger.ADRevenueEvent(impressionData), type);
        }

        /// <summary>
        /// Send events on show some ADS.
        /// </summary>
        /// <param name="adType"> The ads type example Rewardedvideo. </param>
        /// <param name="placementName"> The placement name of this ad. </param>
        public static void AdEvent(EventADSName eventADSName, AdType adType, string placement, EventADSResult result, EventType type = EventType.Main)
        {
            ForEach<IImpressionEvent>((logger) => logger.AdImpressionEvent(eventADSName, adType, placement, result), type);
        }

        static void ForEach<TResut>(Action<TResut> action, EventType type = EventType.Main) where TResut : IEvent
        {
            if (action == null) throw new ArgumentNullException("The action has a null value!...");

            foreach (object obj in _events)
            {
                if (obj is TResut)
                {
                    TResut ev = (TResut)obj;
                    if (ev != null && ev.EventType == type)
                    {
                        action.Invoke(ev);
                    }
                }
            }
        }
    }
}
