using System.Collections;
using UnityEngine;

namespace Apps.Ads
{
    public class AutoAdsReloader
    {
        private float _time;
        private bool _isStarted;

        public bool IsStarted => _isStarted;

        public AutoAdsReloader(float time)
        {
            _time = time;
        }

        public IEnumerator StartLoad()
        {
            while (true)
            {
                LoadAds();

                yield return new WaitForSeconds(_time);
            }
        }

        private void LoadAds()
        {
            ADSManager.LoadInterstitial();
            ADSManager.LoadRewardedVideo();
        }
    }
}