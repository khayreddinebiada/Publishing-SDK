using ConfigSettings = Apps.RemoteConfig.ConfigSettings;
using System.Collections.Generic;
using Firebase.RemoteConfig;
using Apps.RemoteConfig;
using UnityEngine;
using Engine.DI;

namespace Apps.Firebase
{
    public static class FirebaseInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        public static void InializeConfig()
        {
            ConfigSettings settings = Resources.Load<ConfigSettings>("ConfigSettings");

            if (settings == null)
                settings = ScriptableObject.CreateInstance<ConfigSettings>();

            IConfigurator configurator = new Configurator(settings.HasMultipleUpdates, settings.IsEnabled);
            DIContainer.RegisterAsSingle<IConfigServices>(configurator);

            float timeToCompleted = Time.time;

            FirebaseServices services = new FirebaseServices(
                /// On Initialize Firebase.
                () =>
                {
                    InitializeEvents();

                },
                /// On Success Config.
                () =>
                {
                    RegisterConfigs(
                        configurator,
                        FirebaseRemoteConfig.DefaultInstance.AllValues.ToStringDictionary(),
                        settings.SendConfigEvents,
                        timeToCompleted -= Time.time);
                },
                /// On Failed Config.
                (statue) =>
                {
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    values.Add(Constants.TagKey, Constants.UndefinedTag);

                    RegisterConfigs(configurator, values, settings.SendConfigEvents, timeToCompleted -= Time.time);
                }
            );
        }

        private static void RegisterConfigs(IConfigurator configurator, IDictionary<string, string> values, bool sendConfigEvents, float timeToCompleted)
        {
            configurator.UpdateConfig(values);

            if (sendConfigEvents)
            {
                SendConfigEvent(configurator.TagConfig, timeToCompleted);
            }

            configurator.RegisterConfigurator();
        }

        private static void SendConfigEvent(string tagConfig, float timeToCompleted)
        {
            timeToCompleted -= Time.time;
            EventsLogger.CustomEvent($"Config:Tag:{tagConfig}");
            EventsLogger.CustomEvent($"Config:Time:{(int)timeToCompleted}");
        }

        private static void RegisterConfigurator(this IConfigurator configurator)
        {
            DIContainer.RegisterAsSingle(configurator.Values);
            configurator.SetReadyConfig();
        }

        public static void InitializeEvents()
        {
            FirebaseEvents events = new FirebaseEvents();
            EventsLogger.PushEvent(events);
        }
    }
}
