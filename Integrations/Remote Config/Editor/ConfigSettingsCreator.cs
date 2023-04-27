using Apps.editor;
using UnityEditor;

namespace Apps.RemoteConfig.Editor
{
    public static class ConfigSettingsCreator
    {
        [MenuItem("DSOneGames/Ping ConfigSettings", false, 102)]
        public static void ConfigSettingsPing()
        {
            AppsUtility.PingAsset(AppsUtility.GetAsset<ConfigSettings>(ConfigSettings.Path, "ConfigSettings.asset"));
        }
    }
}