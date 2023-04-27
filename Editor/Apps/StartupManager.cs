using UnityEngine;
using UnityEditor;

namespace Apps.editor
{
    [InitializeOnLoad]
    public class StartupManager
    {
        static StartupManager()
        {
            AddDefineSymbolSDK(BuildTargetGroup.Android);
            AddDefineSymbolSDK(BuildTargetGroup.iOS);
        }

        static void AddDefineSymbolSDK(BuildTargetGroup targetGroup)
        {
            string supports = "Support_SDK";
            string result = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            if (!result.Contains(supports))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, result + ";" + supports);
                Debug.Log("Support_SDK is added!!");
            }


            /// Location Usage Description
            PlayerSettings.iOS.locationUsageDescription = "To provide local advertising to user and improve regional app performance.";
        }
    }
}