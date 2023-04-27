using UnityEditor;
using UnityEngine;

namespace Apps.editor
{
    [InitializeOnLoad]
    public class SymbolDefinitionIronSource
    {
        static SymbolDefinitionIronSource()
        {
            AddDefineSymbolSDK(BuildTargetGroup.Android);
            AddDefineSymbolSDK(BuildTargetGroup.iOS);
        }

        static void AddDefineSymbolSDK(BuildTargetGroup targetGroup)
        {
            string supports = "Support_IronSource";
            string result = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            if (!result.Contains(supports))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, result + ";" + supports);
                Debug.Log("Support_ADS is added!!");
            }
        }
    }
}