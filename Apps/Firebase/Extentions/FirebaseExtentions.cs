using Firebase.RemoteConfig;
using System.Collections.Generic;

namespace Apps.Firebase
{
    public static class FirebaseExtentions
    {
        public static IDictionary<string, string> ToStringDictionary(this IDictionary<string, ConfigValue> values)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var item in values)
            {
                result.Add(item.Key, item.Value.StringValue);
            }
            return result;
        }
    }
}