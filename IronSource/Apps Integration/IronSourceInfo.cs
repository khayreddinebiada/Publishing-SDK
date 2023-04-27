using System;

namespace Apps.Ads
{
    [Serializable]
    public class IronSourceInfo
    {
        public string AndroidKey = "Entry Android Key";
        public string IOSKey = "Entry IOS Key";

        public IronSourceBannerPosition BannerPosition = IronSourceBannerPosition.BOTTOM;
    }
}