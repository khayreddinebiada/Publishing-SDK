using System;

namespace Apps
{
    public interface IADS
    {
        /// <summary>
        /// For enable and disable use banner.
        /// </summary>
        string AdPlacement { get; }

        /// <summary>
        /// For enable and disable use banner.
        /// </summary>
        bool UseBanner { get; set; }
        /// <summary>
        /// For enable and disable use interstitial.
        /// </summary>
        bool UseInterstitial { get; set; }
        /// <summary>
        /// For enable and disable use RewardedVideo.
        /// </summary>
        bool UseRewardedVideo { get; set; }

        /// <summary>
        /// To Destroy banner.
        /// </summary>
        void DestroyBanner();
        void LoadBanner();

        /// <summary>
        /// To Display banner.
        /// </summary>
        void DisplayBanner();
        /// <summary>
        /// To hide banner.
        /// </summary>
        void HideBanner();

        /// <summary>
        /// To start (re)load Interstitial.
        /// </summary>
        void LoadInterstitial();
        /// <summary>
        /// To start (re)load RewardedVideo.
        /// </summary>
        void LoadRewardedVideo();

        /// <summary>
        /// To check if the interstitial is available and ready to show.
        /// </summary>
        bool IsInterstitialAvailable();
        /// <summary>
        /// To check if the Rewardedideo is available and ready to show.
        /// </summary>
        bool IsRewardedVideoAvailable();

        /// <summary>
        /// To show Interstitial with placement name.
        /// </summary>
        /// <param name="placementName"> The placement name what we will show to understand the current ad. </param>
        /// <param name="onClose"> The event executed on use close the ad. </param>
        /// <returns> Return true if the ad is showed </returns>
        bool ShowInterstitial(string placementName = null, Action onClose = null, Action onFailed = null);
        /// <summary>
        /// To show RewardedVideo with placement name.
        /// </summary>
        /// <param name="placementName"> The placement name what we will show to understand the current ad. </param>
        /// <param name="onCompleted"> The event is executed when user end all ad and on rewarded statue. </param>
        /// <param name="onClose"> The event executed on use close the ad. </param>
        /// <returns> Return true if the ad is showed </returns>
        bool ShowRewardedVideo(string placementName = null, Action onCompleted = null, Action onClose = null, Action onFailed = null);
    }
}