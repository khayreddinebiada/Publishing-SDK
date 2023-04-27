using Apps.Analytics;
using UnityEngine;

namespace Apps
{
    public enum ShowADSType { Simulator, Debug, Non }

    [CreateAssetMenu(fileName = "AppsSettings", menuName = "Apps/AppsSettings", order = 1)]
    public class AppsSettings : ScriptableObject
    {
        public const string GlobalDirectoryPath = "Assets/_SDK/Resources/";

        public bool AutoInitialize = true;

        public bool IntegrateFacebook = true;
        public string AppLabels = "";
        public string AppFacebookID = "Entry Facebook ID...";
        public string ClientTokens = "";


        public bool IntegrateADS = false;
        public ShowADSType ShowADSType = ShowADSType.Simulator;
        public ADSInfo AdsInfo;

        public bool IntegrateGameAnalytics = true;
        public bool IntegrateAppMetrica = true;
        public AppMetricaInfo AppMetricaInfo;

        public bool IntegrateTenjin = true;
        public string TenjinApiKey = "YAZ1QBH6S2WVW1C2DRSSOF1NNVHDSMSH";

        public string Terms = "https://verariumgames.com/privacy";
        public string Privacy = "https://verariumgames.com/privacy";
    }
}
