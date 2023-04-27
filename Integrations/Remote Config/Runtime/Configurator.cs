using System;
using System.Collections.Generic;

namespace Apps.RemoteConfig
{
    public sealed class Configurator : IConfigurator
    {
        private bool _hasMultipleConfig;

        private DataConfig _dataConfig;

        public string TagConfig
        {
            get { return _dataConfig.Tag; }
        }

        private bool _isReady = false;
        public bool IsReady => _isReady;


        private ConfigCollection _collection;
        public IConfigCollection Values => _collection;

        private event Delegates.OnConfigured _onConfigured;
        public event Delegates.OnConfigured OnConfigured
        {
            add
            {
                _onConfigured += value;
            }
            remove
            {
                _onConfigured -= value;
            }
        }

        public bool IsConfiguredSave => _dataConfig.HasValues;
        public bool HasMultipleConfig => _hasMultipleConfig;

        public bool IsEnabled { get; set; }

        public Configurator(bool hasMultipleConfig = true, bool isEnabled = true)
        {
            IsEnabled = isEnabled;
            _hasMultipleConfig = hasMultipleConfig;

            _dataConfig = DataConfig.CreateDataConfig();

            if (IsConfiguredSave)
            {
                _collection = new ConfigCollection(_dataConfig.SaveConfigs.PairsToDictionary());
                _isReady = true;
            }
            else
                _collection = new ConfigCollection();
        }

        private void ResetValues(IDictionary<string, string> values)
        {
            Dictionary<string, string> newvalues = new Dictionary<string, string>();

            string tag = Constants.UndefinedTag;
            string tagKey = Constants.TagKey;
            foreach (KeyValuePair<string, string> pair in values)
            {
                if (tagKey.Equals(pair.Key))
                {
                    tag = pair.Value;
                }
                else
                {
                    newvalues.Add(pair.Key, pair.Value);
                }
            }

            _collection.UpdateValues(newvalues);
            _dataConfig.UpdateValues(tag, newvalues);
        }

        private void InvokeConfigured()
        {
            _onConfigured?.Invoke(_collection);
        }

        public void UpdateConfig(IDictionary<string, string> values)
        {
            if ((!HasMultipleConfig && IsConfiguredSave) || !IsEnabled) 
                return;

            if (values == null) throw new ArgumentNullException("The values has a null value!...");

            ResetValues(values);
        }

        public void SetReadyConfig()
        {
            if (!IsEnabled)
                return;

            _isReady = true;
            InvokeConfigured();
        }
    }
}