using Engine.Data;
using System.Linq;
using UnityEngine;
using Dictionary = System.Collections.Generic.Dictionary<string, string>;

namespace Apps.RemoteConfig
{
    public struct DataConfig
    {
        private FieldKey<SaveConfigs> _values;

        public bool HasValues => _values.hasValue;
        public string Tag => _values.value.Tag;

        public string[] Keys => _values.value.Keys;
        public string[] Values => _values.value.Values;
        public SaveConfigs SaveConfigs => _values.value;

        public DataConfig(FieldKey<SaveConfigs> values)
        {
            _values = values;
        }

        public static DataConfig CreateDataConfig()
        {
            return CreateDataConfig(new FieldKey<SaveConfigs>(Constants.ValuesKey, Constants.ConfigFile));
        }

        public static DataConfig CreateDataConfig(FieldKey<SaveConfigs> values)
        {
            return new DataConfig(values);
        }

        public void UpdateValues(string tag, Dictionary values)
        {
            _values.value = new SaveConfigs(tag, values.Keys.ToArray(), values.Values.ToArray());
        }
    }
}
