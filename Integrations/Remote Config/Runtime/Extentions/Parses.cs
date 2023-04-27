using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Dictionary = System.Collections.Generic.Dictionary<string, string>;

namespace Apps.RemoteConfig
{
    public static class Parses
    {
        private enum ParseType { Int, Float }

        public static Dictionary PairsToDictionary(this SaveConfigs source)
        {
            string[] keys = source.Keys;
            string[] values = source.Values;

            if (keys.Length != values.Length)
                throw new InvalidOperationException("The keys and Values has not the same Length!...");

            Dictionary dictionary = new Dictionary();

            for (int i = 0; i < keys.Length; i++)
            {
                dictionary.Add(keys[i], values[i]);
            }
            return dictionary;
        }

        private static bool TryParse(this IConfigCollection collection, string key, out float result, ParseType type)
        {
            if (collection.TryGetValue(key, out string value))
            {
                switch (type)
                {
                    case ParseType.Int:
                        if (int.TryParse(value, out int intValue))
                        {
                            result = intValue;
                            return true;
                        }
                        break;
                    case ParseType.Float:
                        if (float.TryParse(value, out float floatValue))
                        {
                            result = floatValue;
                            return true;
                        }
                        break;
                }
            }
            result = default(float);
            return false;
        }

        private static float Parse(this IConfigCollection collection, string key, ParseType type)
        {
            if (collection.TryGetValue(key, out string value))
            {
                switch (type)
                {
                    case ParseType.Int:
                        if (int.TryParse(value, out int intValue))
                            return intValue;
                        throw new FormatException($"You can't Parse the value {value}");
                    case ParseType.Float:
                        if (float.TryParse(value, out float floatValue))
                            return floatValue;
                        throw new FormatException($"You can't Parse the value {value}");

                }
            }
            throw new KeyNotFoundException(key);
        }

        public static int ParseInt(this IConfigCollection collection, string key)
        {
            return (int)Parse(collection, key, ParseType.Int);
        }

        public static float ParseFloat(this IConfigCollection collection, string key)
        {
            return Parse(collection, key, ParseType.Float);
        }

        public static bool TryParseInt(this IConfigCollection collection, string key, out int value)
        {
            bool isParsed = TryParse(collection, key, out float floatValue, ParseType.Int);
            value = (int)floatValue;
            return isParsed;
        }

        public static bool TryParseFloat(this IConfigCollection collection, string key, out float value)
        {
            return TryParse(collection, key, out value, ParseType.Float);
        }
    }
}