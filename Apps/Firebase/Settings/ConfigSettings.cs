using Engine.Attribute;
using UnityEngine;

namespace Apps.RemoteConfig
{
    [CreateAssetMenu(fileName = Name, menuName = "Apps/ConfigSettings", order = 1)]
    [TemplateSettings(Path, Name)]
    public class ConfigSettings : ScriptableObject
    {
        public const string Path = "Assets/_SDK/Resources/";
        public const string Name = "ConfigSettings.asset";

        public bool IsEnabled = true;
        public bool SendConfigEvents = true;
        public bool HasMultipleUpdates = true;
        public float TimeValidateConfig = 10;
    }
}