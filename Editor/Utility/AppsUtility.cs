using System.IO;
using UnityEditor;
using UnityEngine;

namespace Apps.editor
{
    public static class AppsUtility
    {
        public static T CreateAsset<T>(string directoryPath, string assetsName) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            AssetDatabase.CreateAsset(asset, directoryPath + assetsName);
            AssetDatabase.SaveAssets();
            return asset;
        }

        public static void SaveData(Object target)
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void PingObject(Object target)
        {
            Selection.activeObject = target;
            EditorGUIUtility.PingObject(target);
        }

        public static T GetAsset<T>(string globalDirectoryPath, string assetName) where T : UnityEngine.ScriptableObject
        {
            T[] t = ScriptableObjectUtility.GetScriptableObjects<T>();
            if (t == null || t.Length <= 0)
            {
                t = new T[]
                {
                    AppsUtility.CreateAsset<T>(globalDirectoryPath, assetName)
                };

                Debug.Log("The appsSettings is created...");
            }

            if (1 < t.Length)
            {
                Debug.LogWarning("You have 2 or plus AppsSettings, please leave just one for be sure that we use the right one!...");
            }

            return t[0];
        }

        public static void PingAsset<T>(T t) where T : UnityEngine.ScriptableObject
        {
            if (t == null)
                throw new System.ArgumentNullException("The AppsSettings has null variable.");

            SaveData(t);
            PingObject(t);
        }
    }
}