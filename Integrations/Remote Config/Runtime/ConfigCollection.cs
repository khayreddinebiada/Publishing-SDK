using System;
using System.Collections;
using System.Collections.Generic;
using KeyValuePair = System.Collections.Generic.KeyValuePair<string, string>;
using Dictionary = System.Collections.Generic.Dictionary<string, string>;

namespace Apps.RemoteConfig
{
    public sealed class ConfigCollection : IConfigCollection
    {
        private Dictionary _values;

        public string this[string key]
        {
            get
            {
                return _values[key];
            }
        }

        public IEnumerable<string> Keys => _values.Keys;
        public IEnumerable<string> Values => _values.Values;
        public int Count => _values.Count;

        public ConfigCollection()
        {
            _values = new Dictionary();
        }

        public ConfigCollection(Dictionary values)
        {
            _values = values ?? throw new ArgumentNullException("The values has a null value!...");
        }

        public bool TryGetValue(string key, out string value)
        {
            return _values.TryGetValue(key, out value);
        }

        public bool ContainsKey(string key)
        {
            return _values.ContainsKey(key);
        }

        public IEnumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator<KeyValuePair> IEnumerable<KeyValuePair>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public void UpdateValues(Dictionary values)
        {
            _values = values ?? throw new ArgumentNullException($"The Dictionary has a null value!...");
        }
    }
}