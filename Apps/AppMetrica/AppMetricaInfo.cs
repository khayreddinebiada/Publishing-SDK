namespace Apps.Analytics
{
    [System.Serializable]
    public class AppMetricaInfo
    {
        public string ApiKey;

        public bool ExceptionsReporting = true;

        public uint SessionTimeoutSec = 50;

        public bool LocationTracking = false;

        public bool Logs = true;

        public bool HandleFirstActivationAsUpdate;

        public bool StatisticsSending = true;
    }

}