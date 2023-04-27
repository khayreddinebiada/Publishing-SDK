namespace Apps.RemoteConfig
{
    [System.Serializable]
    public struct SaveConfigs
    {
        public string Tag;
        public string[] Keys;
        public string[] Values;

        public SaveConfigs(string tag, string[] keys, string[] values)
        {
            Tag = tag;
            Keys = keys;
            Values = values;
        }
    }
}