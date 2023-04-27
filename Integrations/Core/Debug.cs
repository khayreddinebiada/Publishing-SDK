using System;
using UnityEngine;

namespace Apps
{
    public static class Debuger
    {
        public static void Log(string message) =>
            Debug.Log(message);

        public static void Log(Type type, string message) =>
            Debug.Log($"[{type.Name}] {message}");

        public static void Warning(Type type, string message) =>
            Debug.LogWarning($"[{type.Name}] {message}");

        public static void Error(Type type, string message) =>
            Debug.LogError($"[{type.Name}] {message}");
    }
}